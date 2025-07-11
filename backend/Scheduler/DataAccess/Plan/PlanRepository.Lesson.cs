using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveLesson(Lesson lesson)
    {
        var subject = Subjects.FirstOrDefault(x => x.Id == lesson.SubjectId);
        var theme = subject?.Themes.FirstOrDefault(x => x.Id == lesson.ThemeId);

        if (theme is null)
        {
            return;
        }
        
        theme.Lessons.Add(lesson);
    }

    public Lesson? GetLesson(Guid id)
    {
        return Subjects
            .SelectMany(s => s.Themes
                .SelectMany(t => t.Lessons))
            .FirstOrDefault(l => l.Id == id);
    }

    public List<Lesson> GetLessonsByTheme(Guid themeId)
    {
        return GetTheme(themeId)?.Lessons ?? [];
    }

    public void DeleteLesson(Guid id)
    {
        var lesson = GetLesson(id);
        var theme = GetTheme(lesson!.ThemeId);
        theme?.Lessons.Remove(lesson);
    }
}