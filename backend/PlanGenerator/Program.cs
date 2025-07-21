using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    private const string Plan = "plan";
    static void Main(string[] args)
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

        var subjectsArray = new JArray();
        foreach (var section in inputData["sections"]!)
        {
            var subjectId = Guid.NewGuid();
            var subjectObj = new JObject
            {
                ["Semester"] = section["semester"]?.Value<int>(),
                ["Name"] = section["title"],
                ["DirectionId"] = directionId,
                ["Id"] = subjectId
            };
            
            var themes = new JArray();
            foreach (var topic in section["topics"]!)
            {
                var themeId = Guid.NewGuid();
                var themeObj = new JObject
                {
                    ["Number"] = topic["topic_number"]?.ToObject<int>(),
                    ["Name"] = topic["title"]?.ToString(),
                    ["SubjectId"] = subjectId,
                    ["Id"] = themeId
                };

                var lessons = new JArray();
                foreach (var lesson in topic["lessons"]!)
                {
                    var lessonObj = new JObject
                    {
                        ["SelfStudyHours"] = lesson["self_study_hours"]?.Value<int>(),
                        ["Number"] = lesson["lesson_number"]?.Value<int>(),
                        ["Name"] = lesson["title"]?.ToString(),
                        ["Type"] = GetLessonTypeFromText(lesson["type"]?.Value<string>()),
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

        outputObject["Subjects"] = subjectsArray;
        Console.WriteLine(outputObject.ToString());
        File.WriteAllText($"{Plan}/{directionId}.json", outputObject.ToString());
        return directionId;
    }

    private static int GetLessonTypeFromText(string? type) =>
        type?.Trim() switch
        {
            "Лекция" => 1,
            "Семинар" => 0,
            "Практическое занятие" => 2,
            "Групповое занятие" => 3,
            "Практическоезанятие" => 5,
            "Выходной день" => 4,
            _ => 1
        };
}