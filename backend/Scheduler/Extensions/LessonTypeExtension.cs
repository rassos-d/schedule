using Scheduler.Entities.Constants;

namespace Scheduler.Extensions;

public static class LessonTypeExtension
{
    public static string GetView(this LessonType type)
    {
        return type switch
        {
            LessonType.Group => "г.з",
            LessonType.Lecture => "лек.",
            LessonType.Practice => "п.з.",
            LessonType.Seminar => "сем.",
            LessonType.Training => "трен.",
            LessonType.Weekend => "Выходной",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}