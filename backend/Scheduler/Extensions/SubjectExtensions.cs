using Scheduler.Entities.Plan;

namespace Scheduler.Extensions;

public static class SubjectExtensions
{
    private static readonly char[] Separator = [' ', '-'];

    public static string GetShortName(this Subject subject)
    {
        var name = subject.Name
            .Split(Separator)
            .Select(x => x.ToUpper().First());
        return new string(name.ToArray());
    }
}