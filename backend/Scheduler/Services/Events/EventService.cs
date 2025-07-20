using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Event;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities;
using Scheduler.Entities.General;
using Scheduler.Entities.Plan;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services.Events;

public class EventService(
    TeacherRepository teacherRepository,
    ScheduleRepository scheduleRepository,
    AudienceRepository audienceRepository,
    PlanRepository planRepository,
    SquadRepository squadRepository)
{
    public SimpleDto<Guid>? AddEvent(Guid scheduleId, StudyYear studyYear, Event newEvent)
    {
        var schedulePage = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        if (schedulePage == null)
        {
            return null;
        }

        schedulePage.Events.Add(newEvent);
        scheduleRepository.SaveSchedulePage(schedulePage);

        return new SimpleDto<Guid>(newEvent.Id);
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
        {
            return null;
        }

        existingEvent.ScheduleId = scheduleId;
        existingEvent.TeacherId = updatedEvent.TeacherId ?? existingEvent.TeacherId;
        existingEvent.AudienceId = updatedEvent.AudienceId ?? existingEvent.AudienceId;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.Number = updatedEvent.Number;
        existingEvent.LessonId = updatedEvent.LessonId ?? existingEvent.LessonId;
        existingEvent.SquadId = updatedEvent.SquadId ?? existingEvent.SquadId;

        scheduleRepository.SaveSchedulePage(schedule);

        return existingEvent.Number != null
            ? CheckForConflict(schedule, existingEvent.Number.Value)
            : new CheckConflictResponse();
    }

    public GetEventsByScheduleResponse GetEventsBySchedule(Guid scheduleId, StudyYear studyYear)
    {
        var schedule = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        return ConvertToResponse(schedule);
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
            Squads = ConvertToSquads(
                    schedulePage
                        .Events.Where(e => e is { Date: not null, Number: not null })
                        .ToList(),
                    teacherNames,
                    audienceNames,
                    squads,
                    lessons
                )
                .ToList(),
            NoName = schedulePage.Events
                .Where(e => e.Date == null && e.Number == null)
                .Select(e => ConvertToEvent(e, teacherNames, audienceNames, squads, lessons))
                .ToList()
        };
    }

    private CheckConflictResponse CheckForConflict(SchedulePage schedulePage, int lessonNumber)
    {
        var groupsByTime = schedulePage
            .Events
            .GroupBy(e => (e.Date, EventNumber: e.Number))
            .Where(g => g.Count() > 1);

        var conflictGroups = new List<IGrouping<(DateOnly? Date, int? EventNumber), Event>>();

        foreach (var timeGroup in groupsByTime)
        {
            var teacherConflicts = timeGroup
                .GroupBy(e => e.TeacherId)
                .Where(g => g.Count() > 1);

            var roomConflicts = timeGroup
                .GroupBy(e => e.AudienceId)
                .Where(g => g.Count() > 1);

            if (teacherConflicts.Any() || roomConflicts.Any())
            {
                conflictGroups.Add(timeGroup);
            }
        }

        var conflictEvents = conflictGroups
            .SelectMany(group =>
                group.Select(ev => new
                {
                    Event = ev,
                    GroupKey = group.Key
                }))
            .Select(e => e.Event.Id)
            .ToList();
        return new CheckConflictResponse
        {
            ConflictEventIds = conflictEvents,
            Message = CreateMessage(conflictEvents, lessonNumber)
        };
    }

    private string CreateMessage(List<Guid> conflictEventIds, int lessonNumber)
    {
        return
            $"ВНИМАНИЕ!!! Конфликт с занятиями {string.Join(",", conflictEventIds)} {GetTimeByLessonNumber(lessonNumber)}";
    }

    private string GetTimeByLessonNumber(int lessonNumber)
    {
        return lessonNumber switch
        {
            1 => "8:00 - 9:40",
            2 => "10:00 - 11:30",
            3 => "12:00 - 12:45",
            4 => "13:00 - 14:30",
            5 => "15:00 - 16:30",
        };
    }

    private IEnumerable<GetSquadResponse> ConvertToSquads(List<Event> @event,
        Dictionary<Guid, string> teacherNames,
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, Squad> squads,
        Dictionary<Guid, Lesson> lessons
    )
    {
        var eventBySquad = new Dictionary<Guid, List<EventsResponse>>();

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
            var squad = squads[pair.Key];
            var direction = squad.DirectionId is not null
                ? planRepository.GetDirection(squad.DirectionId!.Value)
                : null;
            var eventsDictionary = pair.Value
                .GroupBy(events => events.Date)
                .OrderBy(v => v.Key)
                .ToDictionary(e => e.Key!.Value,
                    e => e.ToList());
            yield return new GetSquadResponse
            {
                Id = pair.Key,
                Name = squad.Name,
                DaddyName = squad.DaddyId is null ? null : teacherNames[squad.DaddyId!.Value],
                DirectionName = direction?.Name,
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
            Audience = @event.AudienceId.HasValue ? ConvertToResponse(@event.AudienceId.Value, audienceNames.GetValueOrDefault(@event.AudienceId.Value)) : null,
            Date = @event.Date,
            Number = @event.Number,
            Teacher = @event.TeacherId.HasValue ? ConvertToResponse(@event.TeacherId.Value, teacherNames.GetValueOrDefault(@event.TeacherId.Value)) : null,
            Squad = @event.SquadId.HasValue ? ConvertToResponse(@event.SquadId.Value, squads.GetValueOrDefault(@event.TeacherId.Value).Name) : null,
            Lesson = ConvertToResponse(@event.LessonId.Value, lesson.Name),
            LessonType = lesson.Type,
            Theme = ConvertToResponse(@event.ThemeId.Value, theme.Name),
            Subject = ConvertToResponse(subject.Id, subject.Name),
        };
    }

    private EntityNameResponse? ConvertToResponse(Guid? id, string? name)
    {
        if (id is null || name is null)
            return null;
        return new EntityNameResponse
        {
            Id = id.Value,
            Name = name
        };
    }
}