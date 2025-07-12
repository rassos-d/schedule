using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services;

public class EventService
{
    private readonly PlanRepository planRepository;
    private readonly GeneralRepository generalRepository;
    private readonly ScheduleRepository scheduleRepository;
    
    public EventService(GeneralRepository generalRepository, ScheduleRepository scheduleRepository, PlanRepository planRepository)
    {
        this.planRepository = planRepository;
        this.generalRepository = generalRepository;
        this.scheduleRepository = scheduleRepository;
    }
    
    public EventsResponse Get(Guid id, Guid scheduleId)
    {
        var schedule = scheduleRepository.GetSchedule(scheduleId);
        
        
    }

    public GetScheduleResponse GetEventsBySchedule(Guid scheduleId)
    {
        var schedule = scheduleRepository.GetSchedule(scheduleId);
        return ConvertToResponse(schedule);
    }

    private GetScheduleResponse ConvertToResponse(StudyYearPage studyYearPage)
    {
        var teacherNames = studyYearPage
            .Events
            .Select(e => generalRepository.Teachers.Get(e.TeacherId!.Value))
            .ToDictionary(k => k.Id, t => $"{t.Rank} {t.Name}");

        var audienceNames = studyYearPage
            .Events
            .Select(e => generalRepository.Audiences.Get(e.AudienceId!.Value))
            .ToDictionary(k => k.Id, t => t.Name);

        var squadNames = studyYearPage
            .Events
            .Select(e => generalRepository.Squads.Get(e.SquadId!.Value))
            .ToDictionary(k => k.Id, t => t.Name);

        var lessonNames = planRepository.FindLessons()
            .ToDictionary(k => k.Id, l => l.Name);


        return new GetScheduleResponse
        {
            Name = studyYearPage.Name,
            Squads = ConvertToSquads(studyYearPage.Events, teacherNames, audienceNames, squadNames, lessonNames).ToList(),
            NoName = studyYearPage.Events
                .Where(e => e.Date == null && e.EventNumber == null)
                .Select(e => ConvertToEvent(e, teacherNames, audienceNames, squadNames, lessonNames))
                .ToList()
        };
    }

    private IEnumerable<GetSquadResponse> ConvertToSquads(List<Event> @event, 
        Dictionary<Guid, string> teacherNames, 
        Dictionary<Guid, string> audienceNames,
        Dictionary<Guid, string> squadNames,
        Dictionary<Guid, string> lessonNames)
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
            yield return new GetSquadResponse
            {
                Id = pair.Key,
                Name = squadNames[pair.Key],
                Events = pair.Value
                    .GroupBy(events => events.Date)
                    .OrderBy(v => v.Key)
                    .ToDictionary(e => e.Key.Value,
                        e => e.ToList())
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
            AudienceName = @event.AudienceId.HasValue ? audienceNames.GetValueOrDefault(@event.AudienceId.Value) : null,
            Date = @event.Date,
            EventNumber = @event.EventNumber,
            TeacherName = @event.TeacherId.HasValue ? teacherNames.GetValueOrDefault(@event.TeacherId.Value) : null,
            SquadName = @event.SquadId.HasValue ? squadNames.GetValueOrDefault(@event.SquadId.Value) : null,
            LessonName = @event.LessonId.HasValue ? lessonNames.GetValueOrDefault(@event.LessonId.Value) : null
        };
    }
}