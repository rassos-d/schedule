using Scheduler.Constants;
using Scheduler.Entities.Plan;
using Scheduler.Extensions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void UpdateSubjectColors()
    {
        foreach (var directionInfo in GetAllDirectionInfos())
        {
            var usageColors = Colors.All().ToList();
            var direction = GetDirection(directionInfo.Id);
            if (direction is null)
                continue;
            logger.LogInformation("Add colors for direction {0}", direction.Name);

            foreach (var subject in direction.Subjects)
            {
                logger.LogInformation("Add color for subject {0}", subject.Name);
                if (subject.Color == null || !Colors.All().Contains(subject.Color))
                {
                    var color = GetRandomColor(usageColors);
                    subject.Color = color;
                    usageColors.Remove(color);
                }
            }
            SaveChanges();
        }
    }
    
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
    
    private static string GetRandomColor(List<string> colors)
    {
        var random = new Random();
        if (colors.Count == 0)
            colors = Colors.All().ToList();
        var index = random.Next(colors.Count);
        return Colors.All()[index];
    }
}