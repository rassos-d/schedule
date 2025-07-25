using Scheduler.DataAccess;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Schedule;
using Scheduler.Entities.Schedule;
using Scheduler.Export;
using Scheduler.Models;

namespace Scheduler.Services.Schedule;

public class ScheduleService(ScheduleRepository repo, EventGenerator eventGenerator, ExcelExportService export)
{
    public List<ScheduleInfo> Find()
    {
        return repo.GetAllScheduleInfos();
    }

    public Guid Create(ScheduleCreateDto dto)
    {
        var pagesByDates = dto.Pages.GroupBy(p => p.Start.DayOfWeek);
        var schedule = new Entities.Schedule.Schedule {Name = dto.Name, Semester = dto.Semester, Pages = [] };
        // var scheduleInfos = repo.GetAllScheduleInfos();
        // if (scheduleInfos.Any(s => string.Equals(s.Name, dto.Name, StringComparison.CurrentCultureIgnoreCase)))
        //     throw new EntityAlreadyExistExceptions("Календарь с таким именем уже существует");
        
        foreach (var pagesGroup in pagesByDates)
        {
            var minStartDate = pagesGroup.Min(p => p.Start);
            var maxEndDate = pagesGroup.Max(p => p.End);
            var dates = GetDatesForDayOfWeek(minStartDate, maxEndDate);
            var page = new SchedulePage 
            { 
                ScheduleId = schedule.Id,
                Squads = pagesGroup.SelectMany(p => p.Squads).ToList(),
                Dates = dates            
            };
            schedule.Pages.Add(page);
            eventGenerator.Generate(page);
        }

        repo.SaveSchedule(schedule);
        
        return schedule.Id;
    }

    public void FullUpdate(ScheduleUpdateDto dto)
    {
        var schedule = repo.GetSchedule(dto.Id);
        schedule.Pages = [];
        foreach (var pageDto in dto.Pages)
        {
            var existsPage = schedule.Pages.FirstOrDefault(page => page.ScheduleId == dto.Id);
            
            var dates = GetDatesForDayOfWeek(pageDto.Start, pageDto.End);
            var page = new SchedulePage 
            { 
                ScheduleId = schedule.Id,
                Squads = pageDto.Squads,
                Dates = dates,
                Events = existsPage is null ? [] : existsPage.Events.ToList()
            };
            schedule.Pages.Add(page);
            eventGenerator.Generate(page);
        }

        repo.SaveSchedule(schedule);
    }
    
    public List<StudyYearsDto> GetStudyYears(Guid scheduleId)
    {
        var schedule = repo.GetSchedule(scheduleId);
        return schedule.Pages.Select(p => new StudyYearsDto((int) p.StudyYear, p.Dates.Min().DayOfWeek.ToRussian())).ToList();
    }

    public string ExportExcel(Guid scheduleId)
    {
        return export.Save(scheduleId);
    }

    public void Update(EntityNameUpdateDto dto)
    {
        var schedule = new ScheduleInfo(dto.Id, dto.Name);
        repo.UpdateSchedule(schedule);
    }
    
    public SchedulePage GetPage(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        return repo.GetSchedulePage(scheduleId,  dayOfWeek);
    }

    public void Delete(Guid scheduleId)
    {
        repo.DeleteSchedule(scheduleId);
    }
    
    public void DeleteSchedulePage(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        var schedule = repo.GetSchedule(scheduleId);

        schedule.Pages.RemoveAll(page => page.DayOfWeek == dayOfWeek);

        repo.SaveSchedule(schedule);
    }
    
    public string GetName(Guid id)
    {
        return repo.GetSchedule(id).Name;
    }

    public ScheduleCreateDto GetUpdateInfo(Guid scheduleId)
    {
        var schedule = repo.GetSchedule(scheduleId);
        return new ScheduleCreateDto
        {
            Name = schedule.Name,
            Semester = schedule.Semester,
            Pages = schedule.Pages.Select(page => new SchedulePageCreateDto
            {
                DayOfWeek = page.DayOfWeek,
                Squads = page.Squads,
                Start = page.Dates.Min(),
                End = page.Dates.Max()
            }).ToList()
        };
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