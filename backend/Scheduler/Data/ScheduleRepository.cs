public class ScheduleRepository
{
    private readonly string _directoryPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public ScheduleRepository(string basePath = "data")
    {
        _directoryPath = Path.Combine(basePath, "schedules");
        Directory.CreateDirectory(_directoryPath);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    public void SaveSchedule(Guid id, List<Event> events)
    {
        string filePath = Path.Combine(_directoryPath, $"{id}.json");
        string json = JsonSerializer.Serialize(events, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public List<Event>? GetSchedule(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Event>>(json, _jsonOptions);
    }

    public List<Guid> GetAllScheduleIds()
    {
        return Directory.GetFiles(_directoryPath, "*.json")
            .Select(f => Guid.Parse(Path.GetFileNameWithoutExtension(f)))
            .ToList();
    }

    public bool DeleteSchedule(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }

    public List<Event> GetEventsByTeacher(Guid teacherId)
    {
        return GetAllScheduleIds()
            .SelectMany(id => GetSchedule(id) ?? new List<Event>())
            .Where(e => e.TeacherId == teacherId)
            .ToList();
    }

    public List<Event> GetEventsBySquad(Guid squadId)
    {
        return GetAllScheduleIds()
            .SelectMany(id => GetSchedule(id) ?? new List<Event>())
            .Where(e => e.SquadId == squadId)
            .ToList();
    }

    public List<Event> GetEventsByAudience(Guid audienceId)
    {
        return GetAllScheduleIds()
            .SelectMany(id => GetSchedule(id) ?? new List<Event>())
            .Where(e => e.AudienceId == audienceId)
            .ToList();
    }
}