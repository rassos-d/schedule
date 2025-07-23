using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.Dto.Constants;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities.General;

namespace Scheduler.Services.General;

public class SquadService(SquadRepository squadRepository, ScheduleRepository scheduleRepository)
{
    public List<Squad> Find(StudyYear? studyYear)
    {
        var squads = squadRepository.GetAll();
        if (studyYear is not null)
        {
            squads = squads.Where(s => s.StudyYear == studyYear).ToList();
        }
        return squads;
    }

    public Guid Create(SquadRequest dto)
    {
        var squad = new Squad { Name = dto.Name, DirectionId = dto.DirectionId };
        squadRepository.Upsert(squad);
        squadRepository.SaveChanges();
        return squad.Id;
    }
    
    public bool Update(SquadUpdateDto dto)
    {
        var squad = squadRepository.Get(dto.Id);

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
            UpdateEventInAllSchedules(dto.Id, dto.DaddyId.Data!.Value);
        }

        if (dto.FixedAudienceId is not null)
        {
            squad.FixedAudienceId = dto.FixedAudienceId.Data;
        }

        if (dto.StudyYear is not null)
        {
            squad.StudyYear = dto.StudyYear.Data;
        }
        
        squadRepository.Upsert(squad);
        squadRepository.SaveChanges();
        return true;
    }

    public void Delete(Guid id)
    {
        squadRepository.Delete(id);
    }

    private void UpdateEventInAllSchedules(Guid squadId, Guid daddyId)
    {
        var scheduleInfos = scheduleRepository.GetAllScheduleInfos();
        try
        {
            foreach (var scheduleInfo in scheduleInfos)
            {
                foreach (var studyYear in scheduleRepository.GetStudyYears(scheduleInfo.Id))
                {
                    var schedulePage = scheduleRepository.GetSchedulePage(scheduleInfo.Id, (StudyYear)studyYear);
                    if (schedulePage.Squads.Contains(squadId))
                    {
                        foreach (var e in schedulePage.Events)
                        {
                            if (e.SquadId == squadId)
                            {
                                e.TeacherId ??= daddyId;
                            }
                        }
                    }

                    scheduleRepository.SaveSchedulePage(schedulePage);
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

}