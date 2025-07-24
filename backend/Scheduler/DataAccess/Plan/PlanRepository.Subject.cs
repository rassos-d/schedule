using Scheduler.Constants;
using Scheduler.Entities.Plan;
using Scheduler.Extensions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void CreateSubject(Subject subject)
    {
        var direction = GetDirection(subject.DirectionId);
        if (direction is null)
        {
            return;
        }
        direction.Subjects.Add(subject);
        SaveChanges();
    }

    public void UpdateSubject(Subject updatedSubject)
    {
        var direction = GetDirection(updatedSubject.DirectionId);
        if (direction is null)
        {
            return;
        }
        direction.Subjects.RemoveAll(s => s.Id == updatedSubject.Id);
        direction.Subjects.Add(updatedSubject);
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
        var notCachedDirections = directions.ExceptBy(Directions.Select(d => d.Id), d => d.Id);
        foreach (var direction in notCachedDirections)
        {
            GetDirection(direction.Id);
        }
        
        var subjects = Directions.SelectMany(d => d.Subjects).ToList().AddSummingUp();
        return subjects;
    }

    public void DeleteSubject(Guid id)
    {
        var subject = GetSubject(id);
        if (subject == null)
        {
            return;
        }
        
        var direction = Directions.First(x => x.Id == subject.DirectionId);
        direction.Subjects.Remove(subject);
        SaveChanges();
    }
}