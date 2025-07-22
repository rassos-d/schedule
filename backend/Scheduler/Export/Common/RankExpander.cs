internal static class RankExpander
{
    private readonly static Dictionary<string, string> ShortToLongRank = new Dictionary<string, string>()
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
        @short is null ? null : ShortToLongRank.GetValueOrDefault(@short, null);
}