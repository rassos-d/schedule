using System.Text.Json;
using Scheduler.Models.Plan;

namespace Scheduler.Data.Plan;

public partial class PlanRepository
{
    public void SaveLesson(Lesson lesson)
    {
        string filePath = Path.Combine(_directoryPath, $"lesson_{lesson.Id}.json");
        string json = JsonSerializer.Serialize(lesson, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public Lesson? GetLesson(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"lesson_{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Lesson>(json, _jsonOptions);
    }

    public List<Lesson> GetLessonsByTheme(Guid themeId)
    {
        return Directory.GetFiles(_directoryPath, "lesson_*.json")
            .Select(file => JsonSerializer.Deserialize<Lesson>(File.ReadAllText(file), _jsonOptions))
            .Where(l => l != null && l.ThemeId == themeId)
            .ToList()!;
    }

    public bool DeleteLesson(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"lesson_{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}