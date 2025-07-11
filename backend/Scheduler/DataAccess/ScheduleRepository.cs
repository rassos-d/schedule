using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Entities;
using Scheduler.Models;

namespace Scheduler.DataAccess;

public class ScheduleRepository : BaseRepository
{
    private readonly Dictionary<Guid, Schedule> _schedulesCache = new();
   
    
    private const string SchedulesFileName = "schedules.json";

    public ScheduleRepository() : base("schedules")
    {
    }

    private Schedule? LoadSchedule(Guid id)
    {
        var json = ReadFile($"{id}.json");
        return JsonSerializer.Deserialize<Schedule>(json, JsonOptions);
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
        return JsonSerializer.Deserialize<List<ScheduleInfo>>(json, JsonOptions);
    }

    public void SaveSchedule(Schedule schedule)
    {
        var schedulesJson = ReadFile(SchedulesFileName);
        var schedules = JsonSerializer.Deserialize<List<ScheduleInfo>>(schedulesJson, JsonOptions);
        schedules!.Add(new ScheduleInfo(schedule.Id, schedule.Name));
        WriteFile(SchedulesFileName, schedules);
        
        _schedulesCache[schedule.Id] = schedule;
        WriteFile($"{schedule.Id}.json", _schedulesCache);
    }

    public bool DeleteSchedule(Guid id)
    {
        _schedulesCache.Remove(id);
        var filePath = Path.Combine(DirectoryPath, $"{id}.json");
        if (File.Exists(filePath) == false)
        {
            return false;
        }

        File.Delete(filePath);
        return true;
    }

    public override void SaveChanges()
    {
        foreach (var schedule in _schedulesCache)
        {
            WriteFile($"{schedule.Key}.json", schedule.Value);
        }
    }
}