namespace Scheduler.Services.Export.Common;

internal static class DayOfWeekExtensions
{
    private static readonly Dictionary<DayOfWeek, string> Day = new()
    {
        {DayOfWeek.Monday, "ПОНЕДЕЛЬНИК"},
        {DayOfWeek.Tuesday, "ВТОРНИК"},
        {DayOfWeek.Wednesday, "СРЕДА"},
        {DayOfWeek.Thursday, "ЧЕТВЕРГ"},
        {DayOfWeek.Friday, "ПЯТНИЦА"},
        {DayOfWeek.Saturday, "СУББОТА"},
        {DayOfWeek.Sunday, "ВОСКРЕСЕНИЕ"}
    };

    public static string ToRussian(this DayOfWeek dayOfWeek) => Day[dayOfWeek];
}