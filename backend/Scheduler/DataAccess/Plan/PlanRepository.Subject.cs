using Scheduler.Entities.Plan;

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
               ?? GetAllSubjects().FirstOrDefault(s => s.Id == id);
    }

    public List<Subject> GetAllSubjects()
    {
        var directions = GetAllDirectionInfos();
        var notCachedDirections = directions.ExceptBy(Directions.Select(d => d.Id), d => d.Id);
        foreach (var direction in notCachedDirections)
        {
            GetDirection(direction.Id);
        }
        
        return Directions.SelectMany(d => d.Subjects).ToList();
    }

    public List<Subject> GetSubjectsByDirection(Guid directionId)
    {
        var direction = GetDirection(directionId);
        return direction?.Subjects ?? [];
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
    }
}