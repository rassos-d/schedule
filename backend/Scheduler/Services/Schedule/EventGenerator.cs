using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services.Schedule;

public class EventGenerator(SquadRepository squadRepo, PlanRepository planRepo)
{
    public void Generate(SchedulePage page)
    {
        int[] lessonNumbers = [1, 2, 4, 5];
        foreach(var squadId in page.Squads)
        {
            var squad = squadRepo.Get(squadId);
            var themes = planRepo.FindThemesForSemester(squad.DirectionId.Value, page.Semester);
            var themeNumbers = themes.ToDictionary(x => x.Id, x => x.Number);
            var lessons = themes.SelectMany(x => x.Lessons);
            var groupedLessons = lessons
                .GroupBy(x => x.SubjectId)
                .Select(
                    x => x.OrderBy(l => int.Parse($"{themeNumbers[l.ThemeId]}{l.Number.ToString()}")).ToList()
                )
                .ToList();

            var first = new Index(0, new Val(0));
            var second = new Index(1, new Val(0));
            foreach (var date in page.Dates)
            {
                foreach (var lessonNumber in lessonNumbers)
                {
                    var index = lessonNumber is 1 or 2 ? first : second;
                    if (index.Subject < groupedLessons.Count)
                    {
                    }
                    else if(index.Subject >= groupedLessons.Count)
                    {
                        index.Subject = groupedLessons.Count - 1;
                    }
                    else
                    {
                        continue;
                    }
                    
                    var subject = groupedLessons[index.Subject];

                    if (index.Lesson.Value >= subject.Count)
                    {
                        break;
                    }
                    
                    var lesson = subject[index.Lesson.Value];
                    var @event = new Event
                    {
                        ScheduleId = page.ScheduleId,
                        SquadId = squadId,
                        AudienceId = squad.FixedAudienceId,
                        TeacherId = squad.DaddyId,
                        LessonId = lesson.Id,
                        ThemeId = lesson.ThemeId,
                        SubjectId = lesson.SubjectId,
                        Date = date,
                        Number = lessonNumber
                    };
                    page.Events.Add(@event);

                    index.Lesson.Value++;
                    if (index.Lesson.Value == subject.Count)
                    {
                        if (index.Subject + 2 >= groupedLessons.Count)
                        {
                            var other = lessonNumber is 1 or 2 ? second : first;
                            index.Subject = other.Subject;
                            index.Lesson = other.Lesson;
                        }
                        else
                        {
                            index.Subject += 2;
                            index.Lesson.Value = 0;
                        }
                    }
                }
            }
        }
    }
}

file record Index(int Subject, Val Lesson)
{
    public int Subject { get; set; } = Subject;
    public Val Lesson { get; set; } = Lesson;
}

file record Val(int Value)
{
    public int Value { get; set; } = Value;
}

