using Scheduler.Dto.Constants;
using Scheduler.Entities.Plan;

namespace Scheduler.Extensions;

public static class DirectionExtensions
{
    public static Semester GetSemester(this Direction direction, StudyYear studyYear, int sem)
    { 
        var semester = studyYear switch
        {
            StudyYear.First when sem == 1 => Semester.First,
            StudyYear.First when sem == 0 => Semester.Second,
            StudyYear.Second when sem == 1 => Semester.Third,
            StudyYear.Second when sem == 0 => Semester.Fourth,
            StudyYear.Third when sem == 1 => Semester.Fifth,
            StudyYear.Third when sem == 0 => Semester.Sixth,
            StudyYear.Fourth when sem == 1 => Semester.Seventh,
            StudyYear.Fourth when sem == 0 => Semester.Eighth,
            StudyYear.Fifth when sem == 1 => Semester.Ninth,
            StudyYear.Fifth when sem == 0 => Semester.Tenth,
            StudyYear.Sixth when sem == 1 => Semester.Eleventh,
            _ => Semester.First,
        };

        semester = (Semester)((int)semester - (int)direction.Type);
        return semester;
    }
}