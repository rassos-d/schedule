using Scheduler.DataAccess.General;
using Scheduler.Dto;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities.General;

namespace Scheduler.Services.General;

public class SquadService(SquadRepository repo)
{
    public List<Squad> Find() => repo.GetAll();
    public Guid Create(EntityWithNameCreateDto dto)
    {
        var squad = new Squad { Name = dto.Name };
        repo.Upsert(squad);
        repo.SaveChanges();
        return squad.Id;
    }
    
    public bool Update(SquadUpdateDto dto)
    {
        var squad = repo.Get(dto.Id);

        if (squad == null)
        {
            return false;
        }
        
        if (dto.Name is not null && dto.Name.Length > 0)
        {
            squad.Name = dto.Name;
        }

        if (dto.DirectionId is not null)
        {
            squad.DirectionId = dto.DirectionId.Data;
        }

        if (dto.DaddyId is not null)
        {
            squad.DaddyId = dto.DaddyId.Data;
        }

        if (dto.FixedAudienceId is not null)
        {
            squad.FixedAudienceId = dto.FixedAudienceId.Data;
        }

        if (dto.StudyYear is not null)
        {
            squad.StudyYear = dto.StudyYear.Data;
        }
        
        repo.Upsert(squad);
        repo.SaveChanges();
        return true;
    }

    public void Delete(Guid id)
    {
        repo.Delete(id);
    }
}