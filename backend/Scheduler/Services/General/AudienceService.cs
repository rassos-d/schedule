using Scheduler.DataAccess.General;
using Scheduler.Dto;
using Scheduler.Entities.General;
using Scheduler.Services.Schedule;

namespace Scheduler.Services.General;

public class AudienceService(AudienceRepository generalRepo, ScheduleService scheduleService, SquadService squadService)
{
    public List<Audience> Find()
    {
        return generalRepo.GetAll();
    }

    public Audience Create(EntityWithNameCreateDto request)
    {
        var audience = new Audience { Name = request.Name };
        generalRepo.Upsert(audience);
        generalRepo.SaveChanges();
        return audience;
    }

    public void Update(Audience audience)
    {
        generalRepo.Upsert(audience);
        generalRepo.SaveChanges();
    }

    public void Delete(Guid id)
    {
        generalRepo.Delete(id);
        generalRepo.SaveChanges();
        
        scheduleService.DeleteFromAllEvents(e =>
        {
            if (e.AudienceId == id) 
                e.AudienceId = null;
        });
        
        squadService.DeleteFromAllSquads(s =>
        {
            if (s.FixedAudienceId == id)
                s.FixedAudienceId = null;
        });
    }
}