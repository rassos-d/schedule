using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;

namespace Scheduler.Export;

public class ExcelExportService
{
    private readonly GeneralRepository _generalRepository;
    private readonly PlanRepository _planRepository;
    private readonly ScheduleRepository _scheduleRepository;

    public ExcelExportService(
        GeneralRepository generalRepository,
        PlanRepository planRepository,
        ScheduleRepository scheduleRepository
        )
    {
        _generalRepository = generalRepository;
        _planRepository = planRepository;
        _scheduleRepository = scheduleRepository;
    }

    public void Save(Guid scheduleId)
    {
        var schedule = _scheduleRepository.GetSchedule(scheduleId);
        
        foreach (var e in schedule.Events)
        {
            
        }
    }
}