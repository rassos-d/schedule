using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Dto.Constants;
using Scheduler.Entities.Schedule;
using Scheduler.Models;

namespace Scheduler.DataAccess;

public class ScheduleRepository : BaseRepository
{
    private readonly Dictionary<Guid, Schedule> _schedulesCache = new();


    private const string SchedulesFileName = "schedules.json";

    public ScheduleRepository() : base("schedules")
    { }
    
    public void SaveSchedule(Schedule schedule)
    {
        WriteSchedule(schedule);
        foreach (var schedulePage in schedule.Pages)
        {
            WriteSchedulePage(schedulePage);
        }
        
        _schedulesCache[schedule.Id] = schedule;
    }

    public void SaveSchedulePage(SchedulePage schedulePage)
    {
        var schedule = _schedulesCache[schedulePage.ScheduleId];
        var page = schedule.Pages.FirstOrDefault(x => x.StudyYear == schedulePage.StudyYear);
        if (page is not null)
        {
            schedule.Pages.Remove(page);
        }
        
        schedule.Pages.Add(schedulePage);
        WriteSchedulePage(schedulePage);
    }

    public void UpdateSchedule(ScheduleInfo scheduleInfo)
    {
        if (_schedulesCache.TryGetValue(scheduleInfo.Id, out var schedule))
        {
            schedule.Name = scheduleInfo.Name;
            WriteSchedule(schedule);
        }
    }
    

    public SchedulePage GetSchedulePage(Guid id, StudyYear studyYear)
    {
        if (_schedulesCache.TryGetValue(id, out var schedule))
        {
            var page = schedule.Pages.FirstOrDefault(p => p.StudyYear == studyYear);
            if (page is not null)
            {
                return page;
            }
            
            page = LoadSchedulePage(id, studyYear);
            schedule.Pages.Add(page); 
            return page;   
        }

        GetAllScheduleInfos();
        var pageNew = LoadSchedulePage(id, studyYear);
        _schedulesCache[id].Pages.Add(pageNew);
        return pageNew;
    }

    public List<ScheduleInfo> GetAllScheduleInfos()
    {
        var json = ReadFile(SchedulesFileName);
        var schedulesList = JsonSerializer.Deserialize<List<ScheduleInfo>>(json, JsonOptions) ?? [];
        foreach (var schedule in schedulesList)
        {
            if (_schedulesCache.TryGetValue(schedule.Id, out _))
            {
                continue;
            }
            
            _schedulesCache[schedule.Id] = new Schedule {Id = schedule.Id, Name = schedule.Name};
        }
        
        return schedulesList;
    }

    public void DeleteSchedule(Guid id)
    {
        _schedulesCache.Remove(id);
        var scheduleDir = Path.Combine(DirectoryPath, id.ToString());
        Directory.Delete(scheduleDir);

        var schedules = GetAllScheduleInfos();
        var schedule = schedules.FirstOrDefault(s => s.Id == id);
        if (schedule == null)
        {
            return;
        }
        
        schedules.Remove(schedule);
        WriteFile(SchedulesFileName, schedules);
    }

    protected override void SaveChanges(Guid? id = null)
    {
        if (id is not null)
        {
            var schedule = _schedulesCache.GetValueOrDefault(id.Value);
            if (schedule is not null)
            {
                WriteFile($"{schedule.Id}.json", schedule);
            }
            return;
        }
        
        foreach (var schedule in _schedulesCache)
        {
            WriteFile($"{schedule.Key}.json", schedule.Value);
        }
    }

    private void WriteSchedule(Schedule scheduleInfo)
    {
        var schedules = GetAllScheduleInfos();
        var schedule = schedules.FirstOrDefault(s => s.Id == scheduleInfo.Id);
        if (schedule is null)
        {
            schedules.Add(new ScheduleInfo(scheduleInfo.Id, scheduleInfo.Name));
        }
        else
        {
            schedule.Name = scheduleInfo.Name;
        }
        
        WriteFile(SchedulesFileName, schedules);
    }

    private void WriteSchedulePage(SchedulePage schedulePage)
    {
        var scheduleDir = Path.Combine(DirectoryPath, schedulePage.ScheduleId.ToString());
        if (Directory.Exists(scheduleDir) == false)
        {
            Directory.CreateDirectory(scheduleDir);
        }
        
        var pagePath = Path.Combine($"{schedulePage.ScheduleId}/{schedulePage.StudyYear}.json");
        WriteFile(pagePath, schedulePage);
    }
    
    private SchedulePage LoadSchedulePage(Guid scheduleId, StudyYear studyYear)
    {
        var directory = Path.Combine(DirectoryPath, scheduleId.ToString());
        if (Directory.Exists(directory) == false)
        {
            Directory.CreateDirectory(directory);
        }

        var path = $"{scheduleId}/{studyYear}.json";
        var json = ReadFile(path);
        return JsonSerializer.Deserialize<SchedulePage>(json, JsonOptions)!;
    }
    
}