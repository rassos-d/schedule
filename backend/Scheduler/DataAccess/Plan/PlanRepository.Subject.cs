using Scheduler.Constants;
using Scheduler.Entities.Plan;
using Scheduler.Extensions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveSubject(Subject subject)
    {
        var direction = GetDirection(subject.DirectionId);
        if (direction is null)
        {
            return;
        }
        
        direction.Subjects.Add(subject);
        SaveChanges();
    }

    public Subject? GetSubject(Guid id)
    {
        return Subjects.FirstOrDefault(s => s.Id == id) 
               ?? FindSubjects().FirstOrDefault(s => s.Id == id);
    }

    public List<Subject> FindSubjects(Guid? directionId = null)
    {
        if (directionId.HasValue)
        {
            var direction = GetDirection(directionId.Value);
            return direction?.Subjects.AddSummingUp() ?? DataConst.SummingUpList;
        }
        
        var directions = GetAllDirectionInfos();
        var notCachedDirections = directions.ExceptBy(_directions.Select(d => d.Id), d => d.Id);
        foreach (var direction in notCachedDirections)
        {
            GetDirection(direction.Id);
        }
        
        return _directions.SelectMany(d => d.Subjects).ToList().AddSummingUp();
    }

    public void DeleteSubject(Guid id)
    {
        var subject = GetSubject(id);
        if (subject == null)
        {
            return;
        }
        
        var direction = _directions.First(x => x.Id == subject.DirectionId);
        direction.Subjects.Remove(subject);
        SaveChanges();
    }
}