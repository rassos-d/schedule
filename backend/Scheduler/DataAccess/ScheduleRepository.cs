using System.Text.Json;
using Scheduler.Entities;
using Scheduler.Models;

namespace Scheduler.DataAccess;

public class ScheduleRepository
{
    private readonly string _directoryPath;
    private readonly Dictionary<Guid, Schedule> _schedulesCache = new();
    private readonly JsonSerializerOptions _jsonOptions;
    
    private const string SchedulesFileName = "schedules.json";

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

    private Schedule? LoadSchedule(Guid id)
    {
        var json = ReadFile($"{id}.json");
        return JsonSerializer.Deserialize<Schedule>(json, _jsonOptions);
    }

    public Schedule? GetSchedule(Guid id)
    {
        if (_schedulesCache.TryGetValue(id, out var cachedSchedule))
        {
            return cachedSchedule;
        }

        var schedule = LoadSchedule(id);
        if (schedule == null)
        {
            return null;
        }
        
        _schedulesCache[id] = schedule;

        return schedule;
    }

    public List<ScheduleInfo>? GetAllScheduleInfos()
    {
        var json = ReadFile(SchedulesFileName);
        return JsonSerializer.Deserialize<List<ScheduleInfo>>(json, _jsonOptions);
    }

    public void SaveSchedule(Schedule schedule)
    {
        var schedulesJson = ReadFile(SchedulesFileName);
        var schedules = JsonSerializer.Deserialize<List<ScheduleInfo>>(schedulesJson, _jsonOptions);
        schedules!.Add(new ScheduleInfo(schedule.Id, schedule.Name));
        WriteFile(SchedulesFileName, schedules);
        
        _schedulesCache[schedule.Id] = schedule;
        WriteFile($"{schedule.Id}.json", _schedulesCache);
    }

    public bool DeleteSchedule(Guid id)
    {
        _schedulesCache.Remove(id);
        var filePath = Path.Combine(_directoryPath, $"{id}.json");
        if (File.Exists(filePath) == false)
        {
            return false;
        }

        File.Delete(filePath);
        return true;
    }
    
    private string ReadFile(string path)
    {
        var filePath = Path.Combine(_directoryPath, path);
        return File.Exists(filePath) == false 
            ? string.Empty 
            : File.ReadAllText(filePath);
    }
    
    private void WriteFile(string path, object text)
    {
        var filePath = Path.Combine(_directoryPath, path);
        File.WriteAllText(filePath, JsonSerializer.Serialize(text, _jsonOptions));
    }
}