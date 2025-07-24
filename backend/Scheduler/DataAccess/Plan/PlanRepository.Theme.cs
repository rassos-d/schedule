using Scheduler.Dto.Constants;
using Scheduler.Dto.Plan.Lesson;
using Scheduler.Dto.Plan.Theme;
using Scheduler.Entities.Plan;
using Scheduler.Exceptions;
using Scheduler.Extensions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveTheme(Theme theme)
    {
        // if (FindThemes().Any(t => string.Equals(t.Name, theme.Name, StringComparison.CurrentCultureIgnoreCase)))
        //     throw new EntityAlreadyExistExceptions("Тема с таким именем уже создана");
        var subject = GetSubject(theme.SubjectId);
        if (subject is not null)
        {
            subject.Themes.Add(theme);
            SaveChanges(subject.DirectionId);
        }
    }

    public void UpdateTheme(ThemeUpdateDto dto)
    {
        var theme = GetTheme(dto.Id);

        if (theme is null)
            throw new EntityNotFoundException("Тема не существует");

        // if (FindThemes().Any(s => string.Equals(s.Name, dto.Name, StringComparison.CurrentCultureIgnoreCase)))
        //     throw new EntityAlreadyExistExceptions("Тема с таким именем уже создана");

        if (dto.Number is not null && dto.Number > 0)
        {
            theme.Number = dto.Number.Value;
        }

        SaveChanges();
    }

    public List<Theme> FindThemes(Guid? subjectId = null, Guid? directionId = null)
    {
        var themes = Subjects.SelectMany(x => x.Themes);

        if (directionId.HasValue)
            themes = Subjects.Where(s => s.DirectionId == directionId).SelectMany(x => x.Themes);

        if (subjectId.HasValue)
            themes = themes.Where(t => t.SubjectId == subjectId);

        return themes.ToList();
    }

    public void DeleteTheme(Guid id)
    {
        var subject = Subjects.FirstOrDefault(s => s.Themes.Any(t => t.Id == id))
                      ?? FindSubjects().FirstOrDefault(s => s.Themes.Any(t => t.Id == id));
        if (subject == null)
        {
            return;
        }

        var theme = subject.Themes.First(theme => theme.Id == id);
        subject.Themes.Remove(theme);
        SaveChanges();
    }

    public Theme? GetTheme(Guid id)
    {
        return Subjects
                   .SelectMany(d => d.Themes)
                   .FirstOrDefault(x => x.Id == id) ??
               FindSubjects()
                   .SelectMany(x => x.Themes)
                   .FirstOrDefault(x => x.Id == id);
    }
}