using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Entities.Plan;
using Scheduler.Exceptions;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveTheme(Theme theme)
    {
        if (FindThemes().Any(t => string.Equals(t.Name, theme.Name, StringComparison.CurrentCultureIgnoreCase)))
            throw new EntityAlreadyExistExceptions("Тема с таким именем уже создана");
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
            throw new EntityNotFoundException("Тема не существует");

        if (FindThemes().Any(s => string.Equals(s.Name, dto.Name, StringComparison.CurrentCultureIgnoreCase)))
            throw new EntityAlreadyExistExceptions("Тема с таким именем уже создана");
        if (dto.Name.Length > 0)
        {
            theme.Name = dto.Name;
        }
        
        SaveChanges();
    }
    
    public List<Theme> FindThemes(Guid? subjectId = null, Guid? directionId = null, Semester? semester = null)
    {
        var themes = Subjects.SelectMany(x => x.Themes);
        var subjects = Subjects;

        if (subjectId.HasValue)
        {
            themes = themes.Where(t => t.SubjectId == subjectId);
        }

        if(directionId.HasValue || semester.HasValue)
        {
            if(directionId.HasValue)
            {
                subjects = subjects.Where(s => s.DirectionId == directionId);
            }
            if(semester.HasValue)
            {
                subjects = subjects.Where(s => s.Semester == semester);
            }

            themes = themes.Where(t => subjects.Select(s => s.Id).Contains(t.SubjectId));
        }

        return themes.ToList();
    }

    public List<Theme> FindThemesForSemester(Guid directionId, Semester semester)
    {
        var direction = GetDirection(directionId)!;
        var subjects = direction.Subjects.Where(s => s.Semester == semester).ToList();
        var themes = subjects
            .SelectMany(x => x.Themes)
            .Where(t => subjects
                .Select(s => s.Id)
                .Contains(t.SubjectId)
            );
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