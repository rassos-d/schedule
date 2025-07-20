using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main(string[] args)
    {
        // Исходная структура JSON
        string inputJson = File.ReadAllText("tp.json");

        JObject inputData = JObject.Parse(inputJson);

        var outputObject = new JObject();
        outputObject["Name"] = "ВУС-391000";  // Название направления подготовки

        JArray subjectsArray = new JArray();  // Массив предметов

        foreach (var section in inputData["sections"])
        {
            var subjectObj = new JObject();
            var subjectId = Guid.NewGuid();
            int semester = (int)section["semester"];
            subjectObj["Semestr"] = semester;
            subjectObj["Name"] = $"{section["title"].ToString()} из ВУС-391000";
            subjectObj["DirectionId"] = Guid.Parse("1700659a-a2fe-45e5-ab00-d5b57a9c6c9f").ToString("");
            subjectObj["Id"] = subjectId;
            var themes = new JArray();

            foreach (var topic in section["topics"])
            {
                var themeObj = new JObject();
                var themeId = Guid.NewGuid();
                int topicNumber = (int)topic["topic_number"];
                themeObj["Number"] = topicNumber;
                themeObj["Name"] = topic["title"].ToString();
                themeObj["SubjectId"] = subjectId;
                themeObj["Id"] = themeId;
                var lessons = new JArray();

                foreach (var lesson in topic["lessons"])
                {
                    var lessonObj = new JObject();

                    int selfStudyHours = (int)(double)lesson["self_study_hours"];
                    int lessonNumber = (int)lesson["lesson_number"];
                    lessonObj["SelfStudyHours"] = selfStudyHours;
                    lessonObj["Number"] = lessonNumber;
                    lessonObj["Name"] = lesson["title"].ToString();
                    lessonObj["Type"] = GetLessonTypeFromText((string)lesson["type"]);
                    lessonObj["SubjectId"] = subjectId;
                    lessonObj["ThemeId"] = themeId;
                    lessonObj["Id"] = Guid.NewGuid().ToString("D");

                    lessons.Add(lessonObj);
                }

                themeObj["Lessons"] = lessons;

                themes.Add(themeObj);
            }
            subjectObj["Themes"] = themes;

            subjectObj["Id"] = subjectId;
            subjectsArray.Add(subjectObj);
        }

        outputObject["Subjects"] = subjectsArray;
        Console.WriteLine(outputObject.ToString());
        File.WriteAllText("result.json", outputObject.ToString());
    }

    private static int GetLessonTypeFromText(string type)
    {
        switch (type.Trim())
        {
            case "Лекция":
                return 1;
            case "Семинар":
                return 0;
            case "Практическое занятие":
                return 2;
            case "Групповое занятие":
                return 3;
            case "Практическоезанятие":
                return 5;
            case "Выходной день":
                return 4;
            default:
                throw new Exception($"Тип занятия '{type}' неизвестен.");
        }
    }
}