using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;
using Scheduler.Models;

namespace Scheduler.DataAccess;

public class ScheduleRepository : BaseRepository
{
    private readonly Dictionary<Guid, StudyYearPage> _schedulesCache = new();


    private const string SchedulesFileName = "schedules.json";

    public ScheduleRepository() : base("schedules")
    { }

    private StudyYearPage? LoadSchedule(Guid id)
    {
        var json = ReadFile($"{id}.json");
        return JsonSerializer.Deserialize<StudyYearPage>(json, JsonOptions);
    }

    public StudyYearPage? GetSchedule(Guid id)
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

    public List<ScheduleInfo> GetAllScheduleInfos()
    {
        var json = ReadFile(SchedulesFileName);
        return JsonSerializer.Deserialize<List<ScheduleInfo>>(json, JsonOptions) ?? [];
    }

    public void SaveSchedule(StudyYearPage studyYearPage)
    {
        var schedules = GetAllScheduleInfos();
        schedules.Add(new ScheduleInfo(studyYearPage.Id, studyYearPage.Name));
        WriteFile(SchedulesFileName, schedules);

        _schedulesCache[studyYearPage.Id] = studyYearPage;

        WriteFile($"{studyYearPage.Id}.json", _schedulesCache[studyYearPage.Id]);
    }

    public void DeleteSchedule(Guid id)
    {
        _schedulesCache.Remove(id);
        var filePath = Path.Combine(DirectoryPath, $"{id}.json");
        if (File.Exists(filePath) == false)
        {
            return;
        }

        File.Delete(filePath);

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
    

}