using Scheduler.Dto;
using Scheduler.SqlEntities.Plan;

namespace Scheduler.SqlEntities.General;

public class Teacher
{
    public required string Name { get; set; }

    public required string Rank { get; set; }
    
    public List<VacationPeriod> Vacations { get; set; } = [];
    
    public List<Subject> Subjects { get; set; } = [];
}