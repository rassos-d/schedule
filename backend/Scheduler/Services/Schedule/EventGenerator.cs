using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Constants;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services.Schedule;

public class EventGenerator(ScheduleRepository scheduleRepo, SquadRepository squadRepo, PlanRepository planRepo)
{
    public void Generate(SchedulePage page)
    {
        int[] lessonNumbers = { 1, 2, 4, 5 };
        var currentNumberIndex = 0;
        var currentDateIndex = 0;

        foreach(var squadId in page.Squads)
        {
            var squad = squadRepo.Get(squadId);
            var themes = planRepo.FindThemesForSemester(squad.DirectionId.Value, page.Semester);
            var lessons = themes.SelectMany(x => x.Lessons);
            foreach(var lesson in lessons)
            {
                var @event = new Event
                {
                    ScheduleId = page.ScheduleId,
                    SquadId = squadId,
                    AudienceId = squad.FixedAudienceId,
                    TeacherId = squad.DaddyId,
                    LessonId = lesson.Id,
                    ThemeId = lesson.ThemeId,
                    SubjectId = lesson.SubjectId,
                    Date = page.Dates[currentDateIndex],
                    Number = lessonNumbers[currentNumberIndex]
                };

                page.Events.Add(@event);

                if(currentNumberIndex == 3)
                {
                    currentNumberIndex = 0;
                    currentDateIndex++;
                    if(currentDateIndex >= page.Dates.Count)
                    {
                        currentDateIndex = 0;
                    }
                }
            }
        }
    }
}

