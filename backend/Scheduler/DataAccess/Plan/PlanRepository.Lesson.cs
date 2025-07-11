using System.Text.Json;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveLesson(Lesson lesson)
    {
        Lessons.Add(lesson);
        WriteFile($"{lesson.Id}.json", lesson);
        
        var lessons = GetAllLessons();
        lessons.Add(lesson);
        
        WriteFile(LessonsPath, lessons);
    }

    public Lesson? GetLesson(Guid id)
    {
        return GetAllLessons().FirstOrDefault(lesson => lesson.Id == id);
        // string filePath = Path.Combine(LessonsPath, $"lesson_{id}.json");
        // if (!File.Exists(filePath)) return null;
        // string json = File.ReadAllText(filePath);
        // return JsonSerializer.Deserialize<Lesson>(json, JsonOptions);
    }

    public List<Lesson> GetLessonsByTheme(Guid themeId)
    {
        return GetAllLessons()
            .Where(l => l.ThemeId == themeId)
            .ToList()!;
    }

    public bool DeleteLesson(Guid id)
    {
        // string filePath = Path.Combine(LessonsPath, $"lesson_{id}.json");
        // if (!File.Exists(filePath)) return false;
        // File.Delete(filePath);
        // return true;
        
        var lesson = GetLesson(id);
        Lessons.Remove(lesson);
        
        WriteFile(LessonsPath, Lessons);

        return true;
    }
    
    public List<Lesson> GetAllLessons()
    {
        var json = ReadFile(LessonsPath);
        return JsonSerializer.Deserialize<List<Lesson>>(json, JsonOptions) ?? [];
    }
}