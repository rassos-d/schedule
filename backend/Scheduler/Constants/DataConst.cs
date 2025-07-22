using Scheduler.Entities.Plan;

namespace Scheduler.Constants;

public static class DataConst
{
    public static Subject SummingUp = new()
    {
        Id = Guid.Parse("4CDF1240-9276-4AC7-892C-4E4D9A3CF9DB"),
        Name = "Подведение итогов",
    };
    
    public static List<Subject> SummingUpList => [SummingUp];
}