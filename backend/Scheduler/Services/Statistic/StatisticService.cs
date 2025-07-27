using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Statistic.Squad;
using Scheduler.Dto.Statistic.Teacher;

namespace Scheduler.Services.Statistic;

public class StatisticService(
    ScheduleRepository scheduleRepo,
    PlanRepository planRepo,
    SquadRepository squadRepo,
    TeacherRepository teacherRepo
    )
{
    public List<SquadStatisticDto> GetSquadStatistics(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        var result =  new List<SquadStatisticDto>();
        var schedule =  scheduleRepo.GetSchedule(scheduleId);
        var page = scheduleRepo.GetSchedulePage(scheduleId, dayOfWeek);
        var eventsBySquad = page.Events
            .Where(x => x.SquadId.HasValue)
            .GroupBy(e => e.SquadId);
        foreach (var group in eventsBySquad)
        {
            var squad = squadRepo.Get(group.Key!.Value);
            var squadStatisticDto = new SquadStatisticDto
            {
                Id = group.Key!.Value,
                Name = squad?.Name ?? "unknown",
                TeacherId = squad?.DaddyId,
                FixedAudienceId = squad?.FixedAudienceId,
                Subjects = []
            };
            
            var plannedSquadLessons = planRepo
                .FindLessonsForSemester(squad!.DirectionId!.Value, squad.StudyYear!.Value, schedule.Semester);
            var subjectStatistics = plannedSquadLessons
                .GroupBy(x => x.SubjectId)
                .Select(s => new
                {
                    Id = s.Key,
                    planRepo.GetSubject(s.Key)!.Name,
                    PlannedHours = s.Sum(l => l.HoursCount),
                    Lessons = s.ToList()
                });
            foreach (var subjectStatistic in subjectStatistics)
            {
                var completedHours = group.Count(e => e.SubjectId == subjectStatistic.Id) * 2;
                var missingLessons = subjectStatistic.Lessons
                    .Where(l => group.Select(x => x.LessonId).Contains(l.Id) == false)
                    .Select(l => new MissingLessonStatisticDto
                    {
                        LessonId = l.Id,
                        LessonNumber = l.Number ?? -1,
                        ThemeId = l.ThemeId,
                        ThemeNumber = l.ThemeNumber ?? -1,
                        HoursCount = l.HoursCount
                    })
                    .ToList();

                var subjectStatisticDto = new SubjectStatisticDto
                {
                    Id = subjectStatistic.Id,
                    Name = subjectStatistic.Name,
                    PlannedHours = subjectStatistic.PlannedHours,
                    CompletedHours = completedHours,
                    MissingLessons = missingLessons,
                };
                squadStatisticDto.Subjects.Add(subjectStatisticDto);
            }
            
            result.Add(squadStatisticDto);
        }
        
        return result;
    }

    public List<TeacherStatisticDto> GetTeacherStatistics(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        var result = new List<TeacherStatisticDto>();
        var page =  scheduleRepo.GetSchedulePage(scheduleId, dayOfWeek);
        
        var groupedByTeacher = page.Events
            .Where(e => e.TeacherId is not null)
            .GroupBy(e => (Guid)e.TeacherId!);

        foreach (var group in groupedByTeacher)
        {
            var teacher = teacherRepo.Get(group.Key);
            var hoursCount = group.Count() * 2;
            var subjectsCount = group.DistinctBy(x => x.SubjectId).Count();
            var teacherStatistics = new TeacherStatisticDto
            {
                Id = teacher!.Id,
                Name = teacher.Name,
                Rank = teacher.Rank,
                HoursCount = hoursCount,
                SubjectsCount = subjectsCount,
            };
            result.Add(teacherStatistics);
        }

        return result;
    }
}