using System.Text.Json;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveTheme(Theme theme)
    {
        var subject = GetSubject(theme.SubjectId);
        subject.Themes.Add(theme);
    }

    public List<Theme> GetThemesBySubject(Guid subjectId)
    {
        
        return Directory.GetFiles(DirectoryPath, "theme_*.json")
            .Select(file => JsonSerializer.Deserialize<Theme>(File.ReadAllText(file), JsonOptions))
            .Where(t => t != null && t.SubjectId == subjectId)
            .ToList()!;
    }
}