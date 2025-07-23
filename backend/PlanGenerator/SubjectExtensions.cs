using System.Text;

namespace Scheduler.Extensions;

public static class SubjectExtensions
{
    private static readonly char[] Separator = [' ', '-'];
    private static readonly string[] MilitaryWords = ["военно-", "военно", "воинские", "военная"];

    public static string GetShortName(this string? subject)
    {
        if (subject is null)
        {
            return "";
        }
        return SubjectShorts.TryGetValue(subject.ToLower(), out var shortName) 
            ? shortName 
            : ShortName(subject);
    }
    
    private static string ShortName(string subject)
    {
        var result = new StringBuilder();
        var name = subject.ToLower()
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

    private static readonly Dictionary<string, string> SubjectShorts = new()
    {
        ["военно-политическая подготовка"] = "ВПП",
        ["военнополитическая подготовка"] = "ОВП.ВПП",
        ["общевоинские уставы вс рф"] = "ОВУ ВС РФ",
        ["строевая подготовка"] = "СтрП",
        ["огневая подготовка"] = "ОП",
        ["общая тактика"] = "ОТ",
        ["радиационная, химическая и биологическая защита"] = "РХБЗ",
        ["военно-инженерная подготовка"] = "ВИП",
        ["военная топография"] = "ВоенТоп",
        ["военно-медицинская подготовка"] = "ВМП",
        ["военномедицинская подготовка"] = "ВМП",
        ["основы выживания"] = "ОснВыж",
        ["иностранные армии и твд"] = "ИА и ТВД",
        ["тактико-специальная подготовка"] = "ТСП",
        ["тактикоспециальная подготовка"] = "ТСП",
        ["организация связи"] = "ОСРС",
        ["воздушно-десантная подготовка"] = "ВДП",
        ["воздушнодесантная подготовка"] = "ВДП",
        ["военно-специальная подготовка"] = "ВСП",
        ["военноспециальная подготовка"] = "ВСП",
        ["работа на средствах связи"] = "РСС",
        ["вооружение подразделений спн"] = "ВПР",
        ["минно-подрывное дело"] = "МПД",
        ["минноподрывное дело"] = "МПД",
        ["технические средства разведки"] = "ВСП.ТСР",
        ["подготовка по связи"] = "ВСП.Подг. по связи",
        ["военно-техническая подготовка"] = "ВТП",
        ["военнотехническая подготовка"] = "ВТП",
        ["комплексы и средства связи"] = "ВТП.КиС СРС",
        ["техническая эксплуатация радиоэлектронных средств и комплексов"] = "ВТП.ТЭРСК",
        ["организация военно-профессиональной деятельности"] = "ОВПД",
        ["организация военнопрофессиональной деятельности"] = "ОВПД",
    };
}