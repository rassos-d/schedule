using Scheduler.DataAccess.Base;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository : BaseRepository
{
    protected readonly List<Direction> Directions = [];
    protected IEnumerable<Subject> Subjects => Directions.SelectMany(d => d.Subjects);
    
    public PlanRepository() : base("plan")
    {
    }

    public override void SaveChanges()
    {
        foreach (var direction in Directions)
        {
            WriteFile($"{direction.Id}.json", direction);
        }
    }
}