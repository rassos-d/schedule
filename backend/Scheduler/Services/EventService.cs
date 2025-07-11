using Scheduler.DataAccess;

namespace Scheduler.Services;

public class EventService
{
    public EventService(GeneralRepository generalRepository, ScheduleRepository scheduleRepository)
    {
        
    }
    public EventResponse Get(Guid id, Guid scheduleId)
    {
        throw new NotImplementedException();
    }

}