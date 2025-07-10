using System.Text.Json;
using Scheduler.Models.Plan;

namespace Scheduler.Data;

public class PlanRepository
{
    private readonly string _directoryPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public PlanRepository(string basePath = "data")
    {
        _directoryPath = Path.Combine(basePath, "plan");
        Directory.CreateDirectory(_directoryPath);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    #region Direction CRUD
    public void SaveDirection(Direction direction)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{direction.Id}.json");
        string json = JsonSerializer.Serialize(direction, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public Direction? GetDirection(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Direction>(json, _jsonOptions);
    }

    public List<Direction> GetAllDirections()
    {
        return Directory.GetFiles(_directoryPath, "direction_*.json")
            .Select(file => JsonSerializer.Deserialize<Direction>(File.ReadAllText(file), _jsonOptions))
            .Where(d => d != null)
            .ToList()!;
    }

    public bool DeleteDirection(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
    #endregion

    #region Subject CRUD
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
    #endregion

    #region Theme CRUD
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
    #endregion

    #region Lesson CRUD
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
    #endregion
}