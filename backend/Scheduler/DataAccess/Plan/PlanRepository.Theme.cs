using System.Text.Json;
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
    }

    public List<Theme> GetThemesBySubject(Guid subjectId)
    {
        var subject = Subjects.FirstOrDefault(x => x.Id == subjectId);
        return subject?.Themes ?? [];
    }

    public void DeleteTheme(Guid id)
    {
        var subject = Subjects.FirstOrDefault(s => s.Themes.Any(t => t.Id == id))
            ?? GetAllSubjects().FirstOrDefault(s => s.Themes.Any(t => t.Id == id));
        if (subject == null)
        {
            return;
        }

        var theme = subject.Themes.First(theme => theme.Id == id);
        subject.Themes.Remove(theme);
    }

    public Theme? GetTheme(Guid id)
    {
        return Subjects
                   .SelectMany(d => d.Themes)
                   .FirstOrDefault(x => x.Id == id) ??
               GetAllSubjects()
                   .SelectMany(x => x.Themes)
                   .FirstOrDefault(x => x.Id == id);
    }
}