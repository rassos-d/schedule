using Scheduler.Dto.Constants;
using Scheduler.Dto.Plan.Lesson;
using Scheduler.Entities.Plan;
using Scheduler.Extensions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public List<Lesson> FindLessons(Guid? themeId = null, Guid[]? directionIds = null)
    {
        if (themeId.HasValue == false)
        {
            return FindThemes().SelectMany(theme => theme.Lessons).ToList();
        }
        
        var theme = GetTheme(themeId.Value);
        return theme?.Lessons ?? [];
    }
    
    public List<LessonInfo> FindLessonsForSemester(Guid directionId, Semester semester)
    {
        var direction = GetDirection(directionId)!;
        var lessons = direction.Subjects
            .SelectMany(x => x.Themes)
            .SelectMany(t => t.Lessons.Select(l => new LessonInfo
            {
                Id = l.Id,
                LessonType = l.Type,
                Number = l.Number,
                Semester = l.Semester,
                SubjectId = l.SubjectId,
                ThemeId = l.ThemeId,
                Type = l.Type.GetView(),
                ThemeNumber = t.Number
            }))
            .Where(t => t.Semester == semester);
        return lessons.ToList();
    }
    
    public void SaveLesson(Lesson lesson)
    {
        var subject = Subjects.FirstOrDefault(x => x.Id == lesson.SubjectId);
        var theme = subject?.Themes.FirstOrDefault(x => x.Id == lesson.ThemeId);

        if (theme is null)
        {
            return;
        }
        
        theme.Lessons.Add(lesson);
        SaveChanges();
    }

    public Lesson? GetLesson(Guid id)
    {
        return GetLessons().FirstOrDefault(l => l.Id == id);
    }

    public IEnumerable<Lesson> GetLessons()
    {
        return Subjects.SelectMany(s => s.Themes).SelectMany(t => t.Lessons);
    }

    public void UpdateLesson(LessonUpdateDto dto)
    {
        var lesson = GetLesson(dto.Id);

        if (lesson is null)
        {
            return;
        }
        // if (lesson is null)
        //     throw new EntityNotFoundException("Урок не существует");
        //
        // if (GetLessons().Any(l => l.Name == dto.Name))
        //     throw new EntityAlreadyExistExceptions("Занятие с таким именем уже создано");
        
        if (dto.Number is not null && dto.Number > 0)
        {
            lesson.Number = dto.Number.Value;
        }
        
        if (dto.Semester is not null)
        {
            lesson.Semester = dto.Semester.Value;
        }
        
        SaveChanges();
    }

    public void DeleteLesson(Guid id)
    {
        var lesson = GetLesson(id);
        var theme = GetTheme(lesson!.ThemeId);
        theme?.Lessons.Remove(lesson);
        SaveChanges();
    }
}