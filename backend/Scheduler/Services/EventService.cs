using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities;

namespace Scheduler.Services;

public class EventService
{
    private readonly PlanRepository planRepository;
    private readonly GeneralRepository generalRepository;
    private readonly ScheduleRepository scheduleRepository;
    
    public EventService(GeneralRepository generalRepository, ScheduleRepository scheduleRepository, PlanRepository planRepository)
    {
        planRepository = planRepository;
        generalRepository = generalRepository;
        scheduleRepository = scheduleRepository;
    }
    public EventResponse Get(Guid id, Guid scheduleId)
    {
        var schedule = scheduleRepository.GetSchedule(scheduleId);
        throw new NotImplementedException();
    }

    public GetScheduleResponse GetEventsBySchedule(Guid scheduleId)
    {
        throw new NotImplementedException();

    }

    private GetScheduleResponse ConvertToResponse(Schedule schedule)
    {
        var teacherNames = schedule
            .Events
            .Select(e => generalRepository.Teachers.Get(e.TeacherId!.Value))
            .ToDictionary(k => k.Id, t => $"{t.Rank} {t.Name}");

        var audienceNames = schedule
            .Events
            .Select(e => generalRepository.Audiences.Get(e.AudienceId!.Value))
            .ToDictionary(k => k.Id, t => t.Name);

        var squadNames = schedule
            .Events
            .Select(e => generalRepository.Squads.Get(e.SquadId!.Value))
            .ToDictionary(k => k.Id, t => t.Name);

        var lessonNames = planRepository.FindLessons()
            .ToDictionary(k => k.Id, l => l.Name);


        return new GetScheduleResponse
        {
            Name = schedule.Name,
            NoName = schedule.Events
                .Where(e => e.Date == null && e.EventNumber == null)
                .Select(e => ConvertToEvent(e, teacherNames, audienceNames, squadNames, lessonNames))
                .ToList()
        };
    }

    private GetSquadResponse ConvertToSquad(List<Event> @event, Dictionary<Guid, string> squadNames)
    {
        var eventBySquad = new Dictionary<Guid, List<Event>>();

        foreach (var e in @event)
        {
            eventBySquad[e.SquadId!.Value].Add(e);
        }
        throw new NotImplementedException();
    }
    private EventResponse ConvertToEvent(Event @event, 
        Dictionary<Guid, string> teacherNames, 
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, string> squadNames,
        Dictionary<Guid, string> lessonNames)
    {
        return new EventResponse
        {
            AudienceName = @event.AudienceId.HasValue ? audienceNames.GetValueOrDefault(@event.AudienceId.Value) : null,
            Date = @event.Date,
            EventNumber = @event.EventNumber,
            TeacherName = @event.TeacherId.HasValue ? teacherNames.GetValueOrDefault(@event.TeacherId.Value) : null,
            SquadName = @event.SquadId.HasValue ? squadNames.GetValueOrDefault(@event.SquadId.Value) : null,
            LessonName = @event.LessonId.HasValue ? lessonNames.GetValueOrDefault(@event.LessonId.Value) : null
        };
    }
}