using Scheduler.Constants;
using Scheduler.Entities.Plan;

namespace Scheduler.Extensions;

public static class SubjectExtensions
{
    private static readonly char[] Separator = [' ', '-'];

    public static string GetShortName(this Subject subject)
    {
        var name = subject.Name
            .Split(Separator)
            .Where(x => string.IsNullOrWhiteSpace(x) == false && x.Length > 1)
            .Select(x => x.ToUpper().First());
        return new string(name.ToArray());
    }

    public static List<Subject> AddSummingUp(this List<Subject> subjects)
    {
        subjects.Add(DataConst.SummingUp);
        return subjects;
    }
}