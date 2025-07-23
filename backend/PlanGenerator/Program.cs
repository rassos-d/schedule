using Newtonsoft.Json.Linq;
using Scheduler.Extensions;

internal class Program
{
    private const string Plan = "plan";
    static void Main()
    {
        if (Directory.Exists(Plan) == false)
        {
            Directory.CreateDirectory(Plan);    
        }
        
        var files = new[] { "093300", "093700", "094100", "493000" };
        var directions = new List<Tuple<Guid, string>>();
        foreach (var file in files)
        {
            directions.Add(Tuple.Create(Parse(file), file)); 
        }

        var result = new JArray();
        foreach (var dir in directions)
        {
            result.Add(new JObject
            {
                ["Id"] = dir.Item1,
                ["Name"] = $"ВУС-{dir.Item2}",
            });
        }
        
        File.WriteAllText($"{Plan}/directions.json", result.ToString());
    }

    private static Guid Parse(string file)
    {
        var inputJson = File.ReadAllText($"{file}.json");
        var inputData = JObject.Parse(inputJson);

        var directionId = Guid.NewGuid();
        var outputObject = new JObject
        {
            ["Id"] =  directionId,
            ["Name"] = $"ВУС-{file}"
        };

        foreach (var section in inputData["sections"])
        {
            foreach (var topic in section["topics"])
            {
                topic["part"] = topic["part"]?.Value<string>()?.GetShortName();
            }
        }

        var subjectsArray = new JArray();
        foreach (var section in inputData["sections"]!)
        {
            var topics = section["topics"]!.GroupBy(t => t["part"]);
            foreach (var topic in topics)
            {
                var subjectId = Guid.NewGuid();
                var subjectObj = new JObject
                {
                    ["Name"] = $"{section["title"]}.{topic.Key}",
                    ["DirectionId"] = directionId,
                    ["Id"] = subjectId
                };
                var themes = new JArray();
                foreach (var theme in topic)
                {
                    var themeId = Guid.NewGuid();
                    var themeNumber = theme["topic_number"]?.ToObject<int>();
                    var themeObj = new JObject
                    {
                        ["Semester"] = theme["semester"]?.Value<int>(),
                        ["Number"] = themeNumber,
                        ["Name"] = $"Тема {themeNumber}",
                        ["SubjectId"] = subjectId,
                        ["Id"] = themeId
                    };

                    var lessons = new JArray();
                    foreach (var lesson in theme["lessons"]!)
                    {
                        var number = lesson["local_number"]?.Value<int>();
                        var lessonType = lesson["type"]?.Value<string>();
                        var lessonObj = new JObject
                        {
                            ["SelfStudyHours"] = lesson["self_study_hours"]?.Value<int>(),
                            ["Number"] = number,
                            ["Name"] = $"Занятие {number}. ({ShortLessonType(lessonType)})",
                            ["Type"] = GetLessonTypeFromText(lessonType),
                            ["SubjectId"] = subjectId,
                            ["ThemeId"] = themeId,
                            ["Id"] = Guid.NewGuid()
                        };

                        lessons.Add(lessonObj);
                    }

                    themeObj["Lessons"] = lessons;
                    themes.Add(themeObj);
                }
            
                subjectObj["Themes"] = themes;
                subjectsArray.Add(subjectObj);   
            }
        }

        outputObject["Subjects"] = subjectsArray;
        Console.WriteLine(outputObject.ToString());
        File.WriteAllText($"{Plan}/{directionId}.json", outputObject.ToString());
        return directionId;
    }

    private static int GetLessonTypeFromText(string? type) =>
        type?.Trim() switch
        {
            "Лекция" => 1,
            "Семинар" => 3,
            "Практическое занятие" => 2,
            "Групповое занятие" => 0,
            "Практическоезанятие" => 2,
            "Выходной день" => 4,
            _ => 1
        };
    
    private static string ShortLessonType(string? type) =>
        type?.Trim() switch
        {
            "Лекция" => "Лекция",
            "Семинар" => "Семинар",
            "Практическое занятие" => "Практическое",
            "Групповое занятие" => "Групповое",
            "Практическоезанятие" => "Практическое",
            "Выходной день" => "Выходной",
            _ => "Тип не указан"
        };
}