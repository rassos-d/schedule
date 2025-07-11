using System.Text.Json;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveTheme(Theme theme)
    {
        string filePath = Path.Combine(_directoryPath, $"theme_{Guid.NewGuid()}.json");
        string json = JsonSerializer.Serialize(theme, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public List<Theme> GetThemesBySubject(Guid subjectId)
    {
        return Directory.GetFiles(_directoryPath, "theme_*.json")
            .Select(file => JsonSerializer.Deserialize<Theme>(File.ReadAllText(file), _jsonOptions))
            .Where(t => t != null && t.SubjectId == subjectId)
            .ToList()!;
    }
}