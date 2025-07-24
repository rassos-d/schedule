using Scheduler.Dto.Constants;

namespace Scheduler.Extensions;

public static class SemesterExtensions
{
    public static int ToViewSem(this Semester semester)
    {
        return (int) (semester + 1) % 2; 
    }
}