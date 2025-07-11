using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository
{
    public Teacher? GetTeacher(Guid id) => _data.Teachers.GetValueOrDefault(id);
    public List<Teacher> GetAllTeachers() => _data.Teachers.Values.ToList();
    public void UpsertTeacher(Teacher teacher) => _data.Teachers[teacher.Id] = teacher;
    public bool DeleteTeacher(Guid id) => _data.Teachers.Remove(id);
}