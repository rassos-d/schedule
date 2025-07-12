using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public class TeacherRepository : GeneralRepository<Teacher>
{
    protected override Func<GeneralData, Dictionary<Guid, Teacher>> GetData => data => data.Teachers;
}