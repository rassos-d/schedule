using System.Text;
using Scheduler.Constants;
using Scheduler.Entities.Plan;

namespace Scheduler.Extensions;

public static class SubjectExtensions
{
    private static readonly char[] Separator = [' ', '-'];
    private static readonly string[] MilitaryWords = ["военно-", "военно", "воинские", "военная"];

    public static string GetShortName(this Subject subject)
    {
        var result = new StringBuilder();
        var name = subject.Name.ToLower()
            .Split(Separator)
            .Where(x => string.IsNullOrWhiteSpace(x) == false && x.Length > 2)
            .ToList();
        if (name.Count == 2)
        {
            if (CheckOnMilitary(name[0], result) == false)
            {
                var word = name[0];
                result.Append(char.ToUpper(word[0]));
                result.Append(word[1]);
                result.Append(word[2]);
            }
            if (CheckOnMilitary(name[1], result) == false)
            {
                result.Append(char.ToUpper(name[1].First()));
            }
            
            return result.ToString();
        }

        foreach (var word in name)
        {
            if (CheckOnMilitary(word, result) == false)
            {
                result.Append(char.ToUpper(word.First()));
            }
        }

        return result.ToString();
    }
    
    public static List<Subject> AddSummingUp(this List<Subject> subjects)
    {
        var result = new List<Subject>(subjects) { DataConst.SummingUp };
        return result;
    }

    private static bool CheckOnMilitary(string word, StringBuilder result)
    {
        var militaryWord = MilitaryWords.FirstOrDefault(word.Contains);
        if (militaryWord is null)
        {
            return false;
        }
        
        if (word.StartsWith(militaryWord))
        {
            result.Append('В');
            var nextWord = word.Remove(0, militaryWord.Length);
            if (nextWord.Length > 0)
            {
                result.Append(char.ToUpper(nextWord.First()));
            }
        }
        else
        {
            result.Append(char.ToUpper(word.First()));
            result.Append('В');
        }

        return true;
    }
}