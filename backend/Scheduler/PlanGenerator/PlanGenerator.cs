using Newtonsoft.Json.Linq;
using Scheduler.Dto.Constants;

public class PlanGenerator
{
    // todo загружаем один темплан, если он уже существует (по вусовке) - спросить заменить или сохранить копию (1)
    // если пересечения не было - не трогать (старые остаются, новые добавляются)
    // смержить directions.json
    private const string PlanDirectory = "newPlan";
    private readonly string VucesDirectory = Path.Combine("PlanGenerator", "vuces");

    public void Generate()
    {
        if (Directory.Exists(PlanDirectory))
            Directory.Delete(PlanDirectory, true);
        Directory.CreateDirectory(PlanDirectory);

        if (!Directory.Exists(VucesDirectory))
            Directory.CreateDirectory(VucesDirectory);

        var fileNames = Directory.GetFiles(VucesDirectory)
            .Where(x => x.EndsWith(".json"))
            .Select(x => x.Split(Path.DirectorySeparatorChar, '.')[^2]);

        var directions = new List<Tuple<Guid, string>>();
        foreach (var fileName in fileNames)
        {
            var filePath = Path.Combine(VucesDirectory, $"{fileName}.json");
            directions.Add(Tuple.Create(ParseDirection(filePath, fileName), fileName));
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

        File.WriteAllText($"{PlanDirectory}/directions.json", result.ToString());
    }

    private static Guid ParseDirection(string file, string directionName)
    {
        var inputJson = File.ReadAllText(file);
        var inputData = JObject.Parse(inputJson);

        var directionId = Guid.NewGuid();
        var outputObject = new JObject
        {
            ["Id"] =  directionId,
            ["Name"] = $"ВУС-{directionName}"
        };

        var subjectsArray = new JArray();
        foreach (var section in inputData["sections"]!)
        {
            var subjectId = Guid.NewGuid();
            var subjectObj = new JObject
            {
                ["Name"] = section["title"],
                ["DirectionId"] = directionId,
                ["Id"] = subjectId,
                ["ShortName"] = new Subject { Name = section["title"].ToString() }.GetShortName()
            };
            
            var themes = new JArray();
            foreach (var topic in section["topics"]!)
            {
                var themeId = Guid.NewGuid();
                var themeObj = new JObject
                {
                    ["Semester"] = topic["semester"]?.Value<int>(),
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
                        ["Type"] = (int)GetLessonTypeFromText(lesson["type"]?.Value<string>()),
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
        File.WriteAllText($"{PlanDirectory}/{directionId}.json", outputObject.ToString());
        return directionId;
    }

    private static LessonType GetLessonTypeFromText(string? type) =>
        type?.Trim() switch
        {
            "Семинар" => LessonType.Seminar,
            "Лекция" => LessonType.Lecture,
            "Практическое занятие" => LessonType.Practice,
            "Групповое занятие" => LessonType.Group,
            "Выходной день" => LessonType.Weekend,
            "Практическоезанятие" => LessonType.Practice,
            _ => LessonType.Lecture
        };
}