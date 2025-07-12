using Scheduler.DataAccess.General;
using Scheduler.Dto.General.Teacher;
using Scheduler.Entities.General;

namespace Scheduler.Services.General;

public class TeacherService(TeacherRepository repo)
{
    public List<Teacher> Find() => repo.GetAll();
    
    public Guid Create(TeacherCreateDto dto)
    {
        var teacher = new Teacher { Name = dto.Name, Rank = dto.Rank};
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

        if (dto.SubjectIds is not null &&  dto.SubjectIds.Count != 0)
        {
            teacher.SubjectIds = dto.SubjectIds;
        }

        if (dto.Vacations is not null && dto.Vacations.Count != 0)
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