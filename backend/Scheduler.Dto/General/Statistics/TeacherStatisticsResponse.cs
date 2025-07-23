namespace Scheduler.Dto.General.Statistics;

public class TeacherStatisticsResponse
{
    public Guid TeacherId { get; set; }
    
    public string Name { get; set; }
    
    public int StudyHourCount { get; set; }
}