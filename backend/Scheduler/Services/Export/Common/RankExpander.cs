namespace Scheduler.Services.Export.Common;

internal static class RankExpander
{
    private static readonly Dictionary<string, string> ShortToLongRank = new()
    {
        {"п/п-к", "подполковник"},
        {"м-р", "майор"},
        {"п-к", "полковник"},
        {"к-н", "капитан"},
        {"п/п-к.", "подполковник"},
        {"м-р.", "майор"},
        {"п-к.", "полковник"},
        {"к-н.", "капитан"}
    };

    public static string? GetFullOrDefault(string? @short) =>
        @short is not null && ShortToLongRank.TryGetValue(@short, out var value) ? value : null;
}