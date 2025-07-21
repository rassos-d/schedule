using Microsoft.EntityFrameworkCore;
using Scheduler.DataAccessSql.Base;
using Scheduler.Dto.Constants;
using Scheduler.Models;
using Scheduler.SqlEntities.Schedule;

namespace Scheduler.DataAccess;

public class ScheduleRepository(DataContext db)
{
    public async Task<List<StudyYear>> GetStudyYears(Guid scheduleId)
    {
        var years = await db
            .Schedules.Where(x => x.Id == scheduleId)
            .Select(x => x.Pages.Select(y => y.StudyYear).ToList())
            .FirstAsync();
        return years;
    }

    public async Task SaveSchedule(Schedule schedule)
    {
        var entity = await db.Schedules.FirstAsync(x => x.Id == schedule.Id);
        entity.Name = schedule.Name;
        await db.SaveChangesAsync();
    }

    public async Task<Schedule> GetSchedule(Guid scheduleId)
    {
        var schedule = await db
            .Schedules.Include(x => x.Pages)
            .FirstAsync(x => x.Id == scheduleId);
        return schedule;
    }

    public async Task SaveSchedulePage(SchedulePage schedulePage)
    {
        var page = await db
            .SchedulePages.Where(x => x.Id == schedulePage.Id)
            .FirstOrDefaultAsync();

        if (page is null)
        {
            db.Add(schedulePage);
            await db.SaveChangesAsync();
            return;
        }
        
        page.StudyYear =  schedulePage.StudyYear;
        page.Dates = schedulePage.Dates;
        page.Semester = schedulePage.Semester;
        page.Squads = schedulePage.Squads;
        page.Events = schedulePage.Events;
        await db.SaveChangesAsync();
    }

    public async Task UpdateSchedule(ScheduleInfo scheduleInfo)
    {
        await db
            .Schedules.ExecuteUpdateAsync(x => x.SetProperty(y => y.Name, scheduleInfo.Name)
            );
    }


    public async Task<SchedulePage> GetSchedulePage(Guid id, StudyYear studyYear)
    {
        var page = await db
            .SchedulePages.FirstOrDefaultAsync(x => x.ScheduleId == id && x.StudyYear == studyYear
            );
        return page;
    }

    public async Task<List<ScheduleInfo>> GetAllScheduleInfos()
    {
        var schedules = await db
            .Schedules.Select(x => new ScheduleInfo(x.Id, x.Name))
            .ToListAsync();

        return schedules;
    }

    public async Task DeleteSchedule(Guid id)
    {
        await db.SchedulePages.Where(x => x.ScheduleId == id).ExecuteDeleteAsync();
        await db.Schedules.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}