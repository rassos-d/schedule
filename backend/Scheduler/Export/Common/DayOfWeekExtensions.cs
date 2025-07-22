internal static class DayOfWeekExtensions
{
    private readonly static Dictionary<DayOfWeek, string> Day = new Dictionary<DayOfWeek, string>()
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