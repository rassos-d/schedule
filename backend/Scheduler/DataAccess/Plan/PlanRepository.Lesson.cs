using System.Text.Json;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveLesson(Lesson lesson)
    {
        string filePath = Path.Combine(DirectoryPath, $"lesson_{lesson.Id}.json");
        string json = JsonSerializer.Serialize(lesson, JsonOptions);
        File.WriteAllText(filePath, json);
    }

    public Lesson? GetLesson(Guid id)
    {
        string filePath = Path.Combine(DirectoryPath, $"lesson_{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Lesson>(json, JsonOptions);
    }

    public List<Lesson> GetLessonsByTheme(Guid themeId)
    {
        return Directory.GetFiles(DirectoryPath, "lesson_*.json")
            .Select(file => JsonSerializer.Deserialize<Lesson>(File.ReadAllText(file), JsonOptions))
            .Where(l => l != null && l.ThemeId == themeId)
            .ToList()!;
    }

    public bool DeleteLesson(Guid id)
    {
        string filePath = Path.Combine(DirectoryPath, $"lesson_{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}