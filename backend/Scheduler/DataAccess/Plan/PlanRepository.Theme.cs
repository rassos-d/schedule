using Scheduler.Dto;
using Scheduler.Entities.Plan;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveTheme(Theme theme)
    {
        var subject = GetSubject(theme.SubjectId);
        if (subject is not null)
        {
            subject.Themes.Add(theme);
        }
        SaveChanges();
    }

    public void UpdateTheme(EntityNameUpdateDto dto)
    {
        var theme = GetTheme(dto.Id);

        if (theme is null)
        {
            return;
        }
        
        if (dto.Name.Length > 0)
        {
            theme.Name = dto.Name;
        }
        
        SaveChanges();
    }

    public List<Theme> FindThemes(Guid? subjectId = null)
    {
        if (subjectId.HasValue)
        {
            var subject = Subjects.FirstOrDefault(x => x.Id == subjectId);
            return subject?.Themes ?? [];
        }

        return Subjects.SelectMany(s => s.Themes).ToList();
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