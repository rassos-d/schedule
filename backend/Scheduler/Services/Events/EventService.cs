using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Event;
using Scheduler.Dto.General.Squad;
using Scheduler.Dto.Plan.Lesson;
using Scheduler.Dto.Plan.Theme;
using Scheduler.Entities;
using Scheduler.Entities.General;
using Scheduler.Entities.Plan;
using Scheduler.Entities.Schedule;
using Scheduler.Exceptions;
using Scheduler.Extensions;

namespace Scheduler.Services.Events;

public class EventService(
    TeacherRepository teacherRepository,
    ScheduleRepository scheduleRepository,
    AudienceRepository audienceRepository,
    PlanRepository planRepository,
    SquadRepository squadRepository)
{
    public AddEventRequest AddEvent(Guid scheduleId, StudyYear studyYear, Event newEvent)
    {
        var schedulePage = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        // if (schedulePage == null)
        //     throw new EntityNotFoundException("Учебный год не найден.");
        //
        // if (schedulePage.Events.Any(e => e.Date == newEvent.Date && e.Number == newEvent.Number))
        //     throw new EntityNotFoundException("Пара с таким временем уже создана");

        schedulePage.Events.Add(newEvent);
        scheduleRepository.SaveSchedulePage(schedulePage);

        return new AddEventRequest(newEvent.Id, CheckForConflict(schedulePage, newEvent.Id));
    }

    public EventsResponse Get(Guid scheduleId, StudyYear studyYear, Guid id)
    {
        var schedule = scheduleRepository.GetSchedulePage(scheduleId, studyYear);

        throw new NotImplementedException();
        // return ConvertToResponse();
    }

    public CheckConflictResponse? UpdateEvent(Guid scheduleId, StudyYear studyYear, Guid eventId, Event updatedEvent)
    {
        var schedule = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        var existingEvent = schedule.Events.FirstOrDefault(e => e.Id == eventId);
        if (existingEvent == null)
            return null;

        existingEvent.ScheduleId = scheduleId;
        existingEvent.LessonType = updatedEvent.LessonType ?? existingEvent.LessonType;
        existingEvent.TeacherId = updatedEvent.TeacherId ?? existingEvent.TeacherId;
        existingEvent.AudienceId = updatedEvent.AudienceId ?? existingEvent.AudienceId;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.Number = updatedEvent.Number;
        existingEvent.LessonId = updatedEvent.LessonId ?? existingEvent.LessonId;
        existingEvent.SquadId = updatedEvent.SquadId ?? existingEvent.SquadId;

        scheduleRepository.SaveSchedulePage(schedule);

        return CheckForConflict(schedule, existingEvent.Id);
    }

    public GetEventsByScheduleResponse GetEventsBySchedule(Guid scheduleId, StudyYear studyYear)
    {
        var schedule = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        return ConvertToResponse(schedule);
    }

    public void DeleteEvent(Guid scheduleId, StudyYear studyYear, Guid id)
    {
        var page = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        var @event = page.Events.FirstOrDefault(x => x.Id == id);
        if (@event == null)
        {
            return;
        }

        page.Events.Remove(@event);
        scheduleRepository.SaveSchedulePage(page);
    }

    private GetEventsByScheduleResponse ConvertToResponse(SchedulePage schedulePage)
    {
        var teacherNames = teacherRepository.GetAll()
            .ToDictionary(k => k.Id, t => $"{t.Rank} {t.Name}");

        var audienceNames = audienceRepository
            .GetAll()
            .ToDictionary(k => k.Id, t => t.Name);

        var squads = squadRepository
            .GetAll()
            .Where(x => schedulePage.Squads.Contains(x.Id))
            .ToDictionary(k => k.Id);

        var lessons = planRepository.FindLessons().ToDictionary(k => k.Id);

        var schedule = scheduleRepository
            .GetAllScheduleInfos()
            .First(x => x.Id == schedulePage.ScheduleId);
        return new GetEventsByScheduleResponse
        {
            ScheduleId = schedulePage.ScheduleId,
            Name = schedule.Name,
            Semester = schedulePage.Semester.ToViewSem(),
            Squads = ConvertToSquads(
                    schedulePage
                        .Events.Where(e => e is { Date: not null, Number: not null })
                        .ToList(),
                    teacherNames,
                    audienceNames,
                    squads,
                    lessons,
                    schedulePage.Dates
                )
                .ToList(),
            NoName = schedulePage.Events
                .Where(e => e.Date == null && e.Number == null)
                .Select(e => ConvertToEvent(e, teacherNames, audienceNames, squads, lessons))
                .ToList(),
            Conflicts = CheckForConflict(schedulePage)
        };
    }

    private CheckConflictResponse CheckForConflict(SchedulePage schedulePage, Guid? updatedEvent = null)
    {
        var teachers = teacherRepository.GetAll()
            .ToDictionary(t => t.Id, t => t);

        var audienceNames = audienceRepository
            .GetAll()
            .ToDictionary(a => a.Id, a => a.Name);

        var groupsByTime = schedulePage
            .Events
            .Where(e => e is { Date: not null, Number: not null })
            .GroupBy(e => (e.Date, e.Number))
            .Where(g => g.Count() > 1);

        var conflictEvents = new List<Event>();
        foreach (var timeGroup in groupsByTime)
        {
            var teacherConflicts = timeGroup
                .Where(g => g.TeacherId.HasValue)
                .GroupBy(e => e.TeacherId!.Value)
                .Where(g => g.Count() > 1).ToList();

            var roomConflicts = timeGroup
                .Where(e => e.AudienceId.HasValue)
                .GroupBy(e => e.AudienceId)
                .Where(g => g.Count() > 1).ToList();

            if (teacherConflicts.Count > 0)
                conflictEvents.AddRange(teacherConflicts.SelectMany(g => g.Select(e => e)));
            if (roomConflicts.Count > 0)
                conflictEvents.AddRange(roomConflicts.SelectMany(g => g.Select(e => e)));
        }

        conflictEvents = conflictEvents.Distinct().ToList();
        foreach (var e in schedulePage.Events.Where(e => e.Date != null && e.Number != null))
        {
            if (e.TeacherId is not null && teachers.TryGetValue(e.TeacherId.Value, out var teacher))
            {
                if (teacher.Vacations.Any(v => v.StartDate <= e.Date && e.Date <= v.EndDate))
                    conflictEvents.Add(e);
            }
        }
        
        return new CheckConflictResponse
        {
            ConflictEventIds = conflictEvents.ConvertAll(e => e.Id),
            Message = CreateMessage(schedulePage.Events, conflictEvents, updatedEvent, teachers, audienceNames)
        };
    }

    private string? CreateMessage(List<Event> events, List<Event> conflictEvents, Guid? updatedEventId, Dictionary<Guid, Teacher> teachers,
        Dictionary<Guid, string> audiences)
    {
        if (!updatedEventId.HasValue)
            return null;
        var updatedEvent = events.First(e => e.Id == updatedEventId);
        if (updatedEvent.Date == null && updatedEvent.Number == null)
            return null;
        var isConflictWithTeacher = conflictEvents
            .Count(e => e.TeacherId == updatedEvent.TeacherId && e.Date == updatedEvent.Date &&
                        e.Number == updatedEvent.Number) > 1;

        var isConflictWithAudience = conflictEvents
            .Count(e => e.AudienceId == updatedEvent.AudienceId && e.Date == updatedEvent.Date &&
                        e.Number == updatedEvent.Number) > 1;

        var isTeacherInVacation = updatedEvent.TeacherId.HasValue
                                     && teachers.TryGetValue(updatedEvent.TeacherId.Value, out var teacher)
                                     && teacher.Vacations.Any(vacation =>
                                         vacation.StartDate <= updatedEvent.Date &&
                                         vacation.EndDate >= updatedEvent.Date);
        if (!isConflictWithAudience && !isTeacherInVacation && !isConflictWithTeacher)
            return null;

        var message = string.Empty;
        message += isConflictWithTeacher && updatedEvent.TeacherId.HasValue && teachers.TryGetValue(updatedEvent.TeacherId.Value, out var teacher1)
            ? $"Преподаватель {teacher1.Name} занят во время {GetTimeByLessonNumber(updatedEvent.Number.Value)}."
            : "";

        message += isConflictWithAudience && updatedEvent.AudienceId.HasValue && audiences.TryGetValue(updatedEvent.AudienceId.Value, out var audienceName)
            ? $"Аудитория {audienceName} занята во время {GetTimeByLessonNumber(updatedEvent.Number.Value)}."
            : "";

        message += isTeacherInVacation && updatedEvent.TeacherId.HasValue && teachers.TryGetValue(updatedEvent.TeacherId.Value, out var teacher2)
            ? $"Преподаватель {teacher2.Name} находится в отпуске."
            : "";
        
        return "ВНИМАНИЕ!!! " + message;
    }

    private string GetTimeByLessonNumber(int lessonNumber)
    {
        return lessonNumber switch
        {
            1 => "8:30 - 10:00",
            2 => "10:15 - 11:45",
            3 => "12:00 - 12:40",
            4 => "13:30 - 15:00",
            5 => "15:15 - 16:45",
            _ => throw new ArgumentOutOfRangeException(nameof(lessonNumber), lessonNumber, null)
        };
    }

    private IEnumerable<GetSquadResponse> ConvertToSquads(List<Event> @event,
        Dictionary<Guid, string> teacherNames,
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, Squad> squads,
        Dictionary<Guid, Lesson> lessons,
        List<DateOnly> dates
    )
    {
        var eventBySquad = squads.ToDictionary(
            x => x.Key,
            _ => new List<EventsResponse>()
        );

        foreach (var e in @event)
        {
            if (e.SquadId.HasValue == false)
            {
                continue;
            }

            var response = ConvertToEvent(e, teacherNames, audienceNames, squads, lessons);

            if (eventBySquad.ContainsKey(e.SquadId!.Value))
                eventBySquad[e.SquadId!.Value].Add(response);
            else
                eventBySquad[e.SquadId!.Value] = [response];
        }

        foreach (var pair in eventBySquad)
        {
            var squad = squads.GetValueOrDefault(pair.Key);
            if (squad is null) continue;
            var direction = squad.DirectionId is not null
                ? planRepository.GetDirection(squad.DirectionId!.Value)
                : null;

            var eventsDictionary = pair.Value
                .GroupBy(events => events.Date)
                .OrderBy(v => v.Key)
                .ToDictionary(e => e.Key!.Value,
                    e => e.ToList());

            foreach (var date in dates.Where(date => !eventsDictionary.ContainsKey(date)))
                eventsDictionary[date] = [];

            yield return new GetSquadResponse
            {
                Id = pair.Key,
                Name = squad.Name,
                Daddy = ConvertToResponse(squad.DaddyId, teacherNames.GetValueOrDefault(squad.DaddyId ?? Guid.Empty)),
                Direction = ConvertToResponse(direction?.Id, direction?.Name),
                Audience = ConvertToResponse(squad.FixedAudienceId,
                    audienceNames.GetValueOrDefault(squad.FixedAudienceId ?? Guid.Empty)),
                Events = eventsDictionary
            };
        }
    }

    private EventsResponse ConvertToEvent(Event @event,
        Dictionary<Guid, string> teacherNames,
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, Squad> squads,
        Dictionary<Guid, Lesson> lessons)
    {
        var lesson = @event.LessonId.HasValue ? lessons.GetValueOrDefault(@event.LessonId.Value) : null;
        var theme = lesson is not null ? planRepository.GetTheme(lesson.ThemeId) : null;
        var subject = theme is not null ? planRepository.GetSubject(theme.SubjectId) : null;
        return new EventsResponse
        {
            Id = @event.Id,
            Audience = @event.AudienceId.HasValue
                ? ConvertToResponse(@event.AudienceId.Value, audienceNames.GetValueOrDefault(@event.AudienceId.Value))
                : null,
            Date = @event.Date,
            Number = @event.Number,
            Teacher = @event.TeacherId.HasValue
                ? ConvertToResponse(@event.TeacherId.Value, teacherNames.GetValueOrDefault(@event.TeacherId.Value))
                : null,
            Squad = @event.SquadId.HasValue
                ? ConvertToResponse(@event.SquadId.Value, squads.GetValueOrDefault(@event.SquadId.Value)?.Name)
                : null,
            Lesson = new LessonGetDto { Id = @event.LessonId, Name = lesson?.Name, Number = lesson?.Number, LessonType = lesson?.Type, Type = lesson?.Type.GetView()},
            Theme = new ThemeGetDto { Id = @event.ThemeId, Name = theme?.Name, Number = theme?.Number },
            Subject = ConvertToResponse(subject?.Id, subject?.GetShortName()),
        };
    }

    private EntityWithNameGetDto? ConvertToResponse(Guid? id, string? name)
    {
        if (id is null || name is null)
            return null;
        return new EntityWithNameGetDto
        {
            Id = id.Value,
            Name = name
        };
    }
}