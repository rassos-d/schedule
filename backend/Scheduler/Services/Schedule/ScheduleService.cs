using Scheduler.DataAccess;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Schedule;
using Scheduler.Entities.Schedule;
using Scheduler.Models;

namespace Scheduler.Services.Schedule;

public class ScheduleService(ScheduleRepository repo, EventGenerator eventGenerator)
{
    public List<ScheduleInfo> Find()
    {
        return repo.GetAllScheduleInfos();
    }

    public Guid Create(ScheduleCreateDto dto)
    {
        var schedule = new Entities.Schedule.Schedule {Name = dto.Name, Pages = [] };
        foreach (var pageDto in dto.Pages)
        {
            var dates = GetDatesForDayOfWeek(pageDto.Start, pageDto.End);
            var page = new SchedulePage 
            { 
                ScheduleId = schedule.Id,
                Semester = pageDto.Semester,
                StudyYear = pageDto.StudyYear,
                Squads = pageDto.Squads,
                Dates = dates            
            };
            schedule.Pages.Add(page);
            eventGenerator.Generate(page);
        }

        repo.SaveSchedule(schedule);
        
        return schedule.Id;
    }

    public void Update(EntityNameUpdateDto dto)
    {
        var schedule = new ScheduleInfo(dto.Id, dto.Name);
        repo.UpdateSchedule(schedule);
    }
    
    public SchedulePage GetPage(Guid scheduleId, StudyYear studyYear)
    {
        return repo.GetSchedulePage(scheduleId,  studyYear);
    }

    public void Delete(Guid scheduleId)
    {
        repo.DeleteSchedule(scheduleId);
    }

    private static List<DateOnly> GetDatesForDayOfWeek(DateOnly startDate, DateOnly endDate)
    {
        var result = new List<DateOnly>();
        var currentDate = startDate;
        while (currentDate <= endDate)
        {
            result.Add(currentDate);
            currentDate = currentDate.AddDays(7); 
        }

        return result;
    }
}