using System.Text.Json;

public class ScheduleRepository
{
    private readonly string _directoryPath;
    private readonly Dictionary<Guid, Schedule> _schedulesCache = new();
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

    private Schedule LoadSchedule(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"{id}.json");
        if (!File.Exists(filePath)) return null;
        
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Schedule>(json, _jsonOptions);
    }

    public Schedule GetSchedule(Guid id)
    {
        if (_schedulesCache.TryGetValue(id, out var cachedSchedule))
            return cachedSchedule;

        var schedule = LoadSchedule(id);
        if (schedule != null)
            _schedulesCache[id] = schedule;

        return schedule;
    }

    public List<ScheduleInfo> GetAllScheduleInfos()
    {
        return Directory.GetFiles(_directoryPath, "*.json")
            .Select(file => 
            {
                var json = File.ReadAllText(file);
                return JsonSerializer.Deserialize<Schedule>(json, _jsonOptions);
            })
            .Select(s => new ScheduleInfo { Id = s.Id, Name = s.Name })
            .ToList();
    }

    public void SaveSchedule(Schedule schedule)
    {
        _schedulesCache[schedule.Id] = schedule;
        string filePath = Path.Combine(_directoryPath, $"{schedule.Id}.json");
        File.WriteAllText(filePath, JsonSerializer.Serialize(schedule, _jsonOptions));
    }

    public bool DeleteSchedule(Guid id)
    {
        _schedulesCache.Remove(id);
        string filePath = Path.Combine(_directoryPath, $"{id}.json");
        if (!File.Exists(filePath)) return false;
        
        File.Delete(filePath);
        return true;
    }
}

public class ScheduleInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}