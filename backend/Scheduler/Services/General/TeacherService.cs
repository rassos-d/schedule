using Scheduler.DataAccess.General;
using Scheduler.Dto.General.Teacher;
using Scheduler.Entities.General;

namespace Scheduler.Services.General;

public class TeacherService(TeacherRepository repo)
{
    public List<Teacher> Find()
    { 
        var teachers = repo.GetAll();
        teachers.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
        return teachers;
    }
    
    public Guid Create(TeacherCreateDto dto)
    {
        var teacher = new Teacher { Name = dto.Name, Rank = dto.Rank, Vacations = dto.Vacations, SubjectIds = dto.SubjectIds };
        repo.Upsert(teacher);
        repo.SaveChanges();
        return teacher.Id;
    }
    
    public bool Update(TeacherUpdateDto dto)
    {
        var teacher = repo.Get(dto.Id);

        if (teacher == null)
        {
            return false;
        }
        
        if (dto.Name is not null && dto.Name.Length > 0)
        {
            teacher.Name = dto.Name;
        }

        if (dto.Rank is not null &&  dto.Rank.Length > 0)
        {
            teacher.Rank = dto.Rank;
        }

        if (dto.SubjectIds is not null)
        {
            teacher.SubjectIds = dto.SubjectIds;
        }

        if (dto.Vacations is not null)
        {
            teacher.Vacations = dto.Vacations;
        }
        
        repo.Upsert(teacher);
        repo.SaveChanges();
        return true;
    }

    public void Delete(Guid id)
    {
        repo.Delete(id);
    }
}