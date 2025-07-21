using Microsoft.EntityFrameworkCore;
using Scheduler.Models;
using Scheduler.SqlEntities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public async Task SaveDirection(Direction direction)
    {
        db.Add(direction);
        await db.SaveChangesAsync();
    }

    public async Task<Direction?> GetDirectionWithFullInfo(Guid id)
    {
        var direction = await db
            .Directions.Include(x=> x.Subjects)
            .ThenInclude(x => x.Themes)
            .ThenInclude(x => x.Lessons)
            .FirstOrDefaultAsync(x => x.Id == id);
        return direction;
    }

    public async Task<List<DirectionInfo>> GetDirectionShortInfos()
    {
        var directions = await db
            .Directions.Select(x => new DirectionInfo(x.Id, x.Name))
            .ToListAsync();
        return directions;
    }

    public async Task DeleteDirection(Guid id)
    {
        await db.Directions.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}