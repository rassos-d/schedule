using Scheduler.DataAccess;
using Scheduler.Dto.General.Statistics;

namespace Scheduler.Services.Statistics;

public class StatisticsService(ScheduleRepository scheduleRepository)
{
    public StatisticsResponse GetStatistics(Guid schedulerId)
    {
        throw new NotImplementedException();
    }
}