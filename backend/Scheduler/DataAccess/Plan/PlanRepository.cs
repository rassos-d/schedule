using Scheduler.DataAccess.Base;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository : BaseRepository
{
    private readonly List<Direction> _directions = [];
    private IEnumerable<Subject> Subjects => _directions.SelectMany(d => d.Subjects);
    
    public PlanRepository() : base("plan")
    {
        var directions = GetAllDirectionInfos();
        foreach (var direction in directions)
        {
            GetDirection(direction.Id);
        }
    }

    protected override void SaveChanges(Guid? id = null)
    {
        if (id is not null)
        {
            var direction = _directions.FirstOrDefault(x => x.Id == id);
            if (direction is not null)
            {
                WriteFile($"{direction.Id}.json", direction);
            }
            return;
        }
        
        foreach (var direction in _directions)
        {
            WriteFile($"{direction.Id}.json", direction);
        }
    }
}