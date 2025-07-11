using Scheduler.Dto;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public List<Lesson> FindLessons(Guid? themeId = null)
    {
        if (themeId.HasValue == false)
        {
            return FindThemes().SelectMany(theme => theme.Lessons).ToList();
        }
        
        var theme = GetTheme(themeId.Value);
        return theme?.Lessons ?? [];
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
        return Subjects
            .SelectMany(s => s.Themes
                .SelectMany(t => t.Lessons))
            .FirstOrDefault(l => l.Id == id);
    }

    public void UpdateLesson(EntityNameUpdateDto dto)
    {
        var lesson = GetLesson(dto.Id);
        if (lesson is null)
        {
            return;
        }

        if (dto.Name.Length > 0)
        {
            lesson.Name = dto.Name;
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