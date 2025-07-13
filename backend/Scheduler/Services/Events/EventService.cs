using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Event;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services.Events;

public class EventService
{
    private readonly TeacherRepository _teacherRepository;
    private readonly PlanRepository planRepository;
    private readonly SquadRepository _squadRepository;
    private readonly ScheduleRepository scheduleRepository;
    private readonly AudienceRepository audienceRepository;

    public EventService(
        TeacherRepository teacherRepository,
        ScheduleRepository scheduleRepository,
        AudienceRepository audienceRepository,
        PlanRepository planRepository,
        SquadRepository squadRepository)
    {
        _teacherRepository = teacherRepository;
        this.planRepository = planRepository;
        _squadRepository = squadRepository;
        this.scheduleRepository = scheduleRepository;
        this.audienceRepository = audienceRepository;
    }

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

        existingEvent.TeacherId = updatedEvent.TeacherId;
        existingEvent.AudienceId = updatedEvent.AudienceId;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.Number = updatedEvent.Number;
        existingEvent.LessonId = updatedEvent.LessonId;
        existingEvent.SquadId = updatedEvent.SquadId;

        scheduleRepository.SaveSchedulePage(schedule);

        return existingEvent.Number != null ? CheckForConflict(schedule, existingEvent.Number.Value) : new CheckConflictResponse();
    }
    
    public GetEventsByScheduleResponse GetEventsBySchedule(Guid scheduleId, StudyYear studyYear)
    {
        var schedule = scheduleRepository.GetSchedulePage(scheduleId, studyYear);
        return ConvertToResponse(schedule);
    }

    private GetEventsByScheduleResponse ConvertToResponse(SchedulePage schedulePage)
    {
        var teacherNames = _teacherRepository.GetAll()
            .ToDictionary(k => k.Id, t => $"{t.Rank} {t.Name}");

        var audienceNames = audienceRepository
            .GetAll()
            .ToDictionary(k => k.Id, t => t.Name);

        var squadNames = _squadRepository
            .GetAll()
            .ToDictionary(k => k.Id, t => t.Name);

        var lessonNames = planRepository
            .FindLessons()
            .ToDictionary(k => k.Id, l => l.Name);

        return new GetEventsByScheduleResponse
        {
            ScheduleId = schedulePage.ScheduleId,
            Squads = ConvertToSquads(schedulePage
                    .Events.Where(e => e.Date != null && e.Number != null)
                    .ToList(),
                teacherNames, audienceNames, squadNames, lessonNames, schedulePage.Dates).ToList(),
            NoName = schedulePage.Events
                .Where(e => e.Date == null && e.Number == null)
                .Select(e => ConvertToEvent(e, teacherNames, audienceNames, squadNames, lessonNames))
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
                group.Select(ev => new { 
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
        return $"ВНИМАНИЕ!!! Конфликт с занятиями {string.Join(",", conflictEventIds)} {GetTimeByLessonNumber(lessonNumber)}";
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
        Dictionary<Guid, string> squadNames,
        Dictionary<Guid, string> lessonNames,
        List<DateOnly> dates)
    {
        var eventBySquad = new Dictionary<Guid, List<EventsResponse>>();

        foreach (var e in @event)
        {
            if (!e.SquadId.HasValue) continue;
            var response = ConvertToEvent(e, teacherNames, audienceNames, squadNames, lessonNames);

            if (eventBySquad.ContainsKey(e.SquadId!.Value))
                eventBySquad[e.SquadId!.Value].Add(response);
            else
                eventBySquad[e.SquadId!.Value] = [response];
        }
        
        foreach (var pair in eventBySquad)
        {
            var eventsDictionary = pair.Value
                .GroupBy(events => events.Date)
                .OrderBy(v => v.Key)
                .ToDictionary(e => e.Key.Value,
                    e => e.ToList());
            foreach (var date in dates.Where(date => !eventsDictionary.ContainsKey(date)))
            {
                eventsDictionary[date] = [];
            }
            
            yield return new GetSquadResponse
            {
                Id = pair.Key,
                Name = squadNames[pair.Key],
                Events = eventsDictionary
            };
        }
    }
    private EventsResponse ConvertToEvent(Event @event, 
        Dictionary<Guid, string> teacherNames, 
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, string> squadNames,
        Dictionary<Guid, string> lessonNames)
    {
        return new EventsResponse
        {
            Id = @event.Id,
            AudienceName = @event.AudienceId.HasValue ? audienceNames.GetValueOrDefault(@event.AudienceId.Value) : null,
            Date = @event.Date,
            Number = @event.Number,
            TeacherName = @event.TeacherId.HasValue ? teacherNames.GetValueOrDefault(@event.TeacherId.Value) : null,
            SquadName = @event.SquadId.HasValue ? squadNames.GetValueOrDefault(@event.SquadId.Value) : null,
            LessonName = @event.LessonId.HasValue ? lessonNames.GetValueOrDefault(@event.LessonId.Value) : null
        };
    }
}