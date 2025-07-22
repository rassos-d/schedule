using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Schedule;
using Scheduler.Entities.Schedule;
using Scheduler.Exceptions;
using Scheduler.Export;
using Scheduler.Extensions;
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
        var schedule = new Entities.Schedule.Schedule {Name = dto.Name, Pages = [] };
        // var scheduleInfos = repo.GetAllScheduleInfos();
        // if (scheduleInfos.Any(s => string.Equals(s.Name, dto.Name, StringComparison.CurrentCultureIgnoreCase)))
        //     throw new EntityAlreadyExistExceptions("Календарь с таким именем уже существует");
        
        foreach (var pageDto in dto.Pages)
        {
            var dates = GetDatesForDayOfWeek(pageDto.Start, pageDto.End);
            var page = new SchedulePage 
            { 
                ScheduleId = schedule.Id,
                Semester = GetSemester(pageDto.StudyYear, pageDto.Semester),
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

    public void FullUpdate(ScheduleUpdateDto dto)
    {
        var schedule = repo.GetSchedule(dto.Id);

        foreach (var pageDto in dto.Pages)
        {
            var existsPage = repo.GetSchedulePage(dto.Id, pageDto.StudyYear);
            
            var dates = GetDatesForDayOfWeek(pageDto.Start, pageDto.End);
            var page = new SchedulePage 
            { 
                ScheduleId = schedule.Id,
                Semester = GetSemester(pageDto.StudyYear, pageDto.Semester),
                StudyYear = pageDto.StudyYear,
                Squads = pageDto.Squads,
                Dates = dates,
                Events = existsPage.Events.ToList()
            };
            schedule.Pages.Add(page);
        }

        repo.SaveSchedule(schedule);
    }
    
    public List<int> GetStudyYears(Guid scheduleId)
    {
        return repo.GetStudyYears(scheduleId);
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
    
    public SchedulePage GetPage(Guid scheduleId, StudyYear studyYear)
    {
        return repo.GetSchedulePage(scheduleId,  studyYear);
    }

    public void Delete(Guid scheduleId)
    {
        repo.DeleteSchedule(scheduleId);
    }
    
    public void DeleteSchedulePage(Guid scheduleId, StudyYear studyYear)
    {
        var schedule = repo.GetSchedule(scheduleId);

        schedule.Pages.RemoveAll(page => page.StudyYear == studyYear);

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
            Pages = schedule.Pages.Select(page => new SchedulePageCreateDto
            {
                StudyYear = page.StudyYear,
                Semester = page.Semester.ToViewSem(),
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

    private static Semester GetSemester(StudyYear studyYear, int sem)
    {
        if (studyYear == StudyYear.First && sem == 0)
        {
            return Semester.First;
        }

        if (studyYear == StudyYear.Second && sem == 1)
        {
            return Semester.Second;
        }

        if (studyYear == StudyYear.Second && sem == 0)
        {
            return Semester.Third;
        }

        if (studyYear == StudyYear.Third && sem == 1)
        {
            return Semester.Fourth;
        }

        if (studyYear == StudyYear.Third && sem == 0)
        {
            return Semester.Fiveth;
        }

        return 0;
    }
}