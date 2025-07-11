using System.Text.Json;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveSubject(Subject subject)
    {
        string filePath = Path.Combine(_directoryPath, $"subject_{subject.Id}.json");
        string json = JsonSerializer.Serialize(subject, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public Subject? GetSubject(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"subject_{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Subject>(json, _jsonOptions);
    }

    public List<Subject> GetAllSubjects()
    {
        return Directory.GetFiles(_directoryPath, "subject_*.json")
            .Select(file => JsonSerializer.Deserialize<Subject>(File.ReadAllText(file), _jsonOptions))
            .Where(s => s != null)
            .ToList()!;
    }

    public List<Subject> GetSubjectsByDirection(Guid directionId)
    {
        return GetAllSubjects().Where(s => s.DirectionId == directionId).ToList();
    }

    public bool DeleteSubject(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"subject_{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}