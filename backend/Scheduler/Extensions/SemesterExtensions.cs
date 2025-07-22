using Scheduler.Dto.Constants;

namespace Scheduler.Extensions;

public static class SemesterExtensions
{
    public static int ToViewSem(this Semester semester)
    {
        return semester switch
        {
            Semester.First => 0,
            Semester.Second => 1,
            Semester.Third => 0,
            Semester.Fourth => 1,
            Semester.Fiveth => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(semester), semester, null)
        };
    }
}