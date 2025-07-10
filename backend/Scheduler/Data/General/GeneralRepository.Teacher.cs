using Scheduler.Models.General;

namespace Scheduler.Data.General;

public partial class GeneralRepository
{
    public Teacher? GetTeacher(Guid id) => _data.Teachers.FirstOrDefault(t => t.Id == id);
    public List<Teacher> GetAllTeachers() => _data.Teachers;
    
    public void AddTeacher(Teacher teacher) => _data.Teachers.Add(teacher);
    
    public bool UpdateTeacher(Teacher teacher)
    {
        var index = _data.Teachers.FindIndex(t => t.Id == teacher.Id);
        if (index == -1) return false;
        _data.Teachers[index] = teacher;
        return true;
    }
    public bool DeleteTeacher(Guid id)
    {
        var teacher = GetTeacher(id);
        if (teacher == null) return false;
        return _data.Teachers.Remove(teacher);
    }
}