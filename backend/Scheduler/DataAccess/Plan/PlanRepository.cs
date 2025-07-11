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

    protected override void SaveChanges(Guid? id = null)
    {
        if (id is not null)
        {
            var direction = Directions.FirstOrDefault(x => x.Id == id);
            if (direction is not null)
            {
                WriteFile($"{direction.Id}.json", direction);
            }
            return;
        }
        
        foreach (var direction in Directions)
        {
            WriteFile($"{direction.Id}.json", direction);
        }
    }
}