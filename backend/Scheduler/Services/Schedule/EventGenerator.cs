using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities;
using Scheduler.Entities.Schedule;

namespace Scheduler.Services.Schedule;

public class EventGenerator(SquadRepository squadRepo, PlanRepository planRepo)
{
    public void Generate(int semester, SchedulePage page)
    {
        int[] lessonNumbers = [1, 2, 4];
        foreach (var squadId in page.Squads)
        {
            var squad = squadRepo.Get(squadId);
            if (squad?.DirectionId == null || !squad.StudyYear.HasValue)
                continue;
            var lessons =
                planRepo.FindLessonsForSemester(squad.DirectionId.Value, squad.StudyYear.Value, semester);
            if (lessons.Count == 0)
                continue;
            var groupedLessons = lessons
                .GroupBy(x => x.SubjectId)
                .Select(x => x.OrderBy(l => int.TryParse($"{l.ThemeNumber}{l.Number.ToString()}", out var orderBy) ? orderBy : int.MaxValue).ToList()
                )
                .OrderBy(x => x.Count)
                .ToList();
            
            var first = new Index(0, new Val(0));
            var second = new Index(1, new Val(0));
            foreach (var date in page.Dates)
            {
                foreach (var lessonNumber in lessonNumbers)
                {
                    var index = groupedLessons.Count == 1 ? first : lessonNumber is 1 or 2 ? first : second;
                    if (index.Subject < groupedLessons.Count)
                    {
                    }
                    else if (index.Subject >= groupedLessons.Count)
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
                    for (var i = 0; i < lesson.HoursCount; i += 2)
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
                            Date = i == 0 ? date : null,
                            Number = i == 0 ? lessonNumber : null
                        };
                        page.Events.Add(@event);
                    }

                    index.Lesson.Value++;

                    if (index.Lesson.Value == subject.Count)
                    {
                        if (index.Subject + 2 >= groupedLessons.Count && groupedLessons.Count > 1)
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

            var eventLessons = page.Events.Select(x => x.LessonId);
            var stashLessons = groupedLessons
                .SelectMany(x => x)
                .Where(x => eventLessons.Contains(x.Id) == false)
                .Select(x => new Event
                {
                    ScheduleId = page.ScheduleId,
                    SquadId = squadId,
                    AudienceId = squad.FixedAudienceId,
                    TeacherId = squad.DaddyId,
                    LessonId = x.Id,
                    ThemeId = x.ThemeId,
                    SubjectId = x.SubjectId
                });
            
            page.Events.AddRange(stashLessons);
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