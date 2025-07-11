using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveSubject(Subject subject)
    {
        var direction = GetDirection(subject.DirectionId);
        direction.Subjects.Add(subject);
    }

    public Subject? GetSubject(Guid id)
    {
        return Directions.SelectMany(d => d.Subjects).FirstOrDefault(s => s.Id == id);
    }

    public List<Subject> GetAllSubjects()
    {
        var allDirectionFiles = Directory.GetFiles(DirectionsPath).Select(d => );
        return Directions.SelectMany(d => d.Subjects).ToList();
    }

    public List<Subject> GetSubjectsByDirection(Guid directionId)
    {
        var direction = GetDirection(directionId);
        return direction.Subjects;
    }

    public bool DeleteSubject(Guid id)
    {
        var direction = Directions.FirstOrDefault(d => d.Subjects.Select(s => s.Id).Contains(id));
        if (direction is null)
        {
            return false;
        }
        
        var subject = GetSubject(id);
        if (subject == null)
        {
            return false;
        }
        direction.Subjects.Remove(subject);
        return true;
    }
}