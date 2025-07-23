using System.Text.RegularExpressions;
using System.Text.Json;

public class Program
{
    enum LessonType
    {
        Group = 0,
        Lecture = 1,
        Practice = 2,
        Seminar = 3,
        Sum = 4
    }

    public class Lesson
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Number { get; set; }
        public int Type { get; set; }
        public int Semester { get; set; }
        public Guid ThemeId { get; set; }
        public Guid SubjectId { get; set; }
    }

    public class Theme
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Number { get; set; }
        public Guid SubjectId { get; set; }
        public List<Lesson> Lessons { get; set; } = new();
    }

    public class Subject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid DirectionId { get; set; }
        public List<Theme> Themes { get; set; } = new();
    }

    public class Direction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "ВУС-094100";
        public List<Subject> Subjects { get; set; } = new();
    }

    public static void Main()
    {
        string input = "“ОВП.ВПП”1л,1л,1лл,1л,1л,1л,1ллл”ОВП.ОВУ ВС РФ”1л,1л,1л,1л,1л,1л,1л,1лл”ОВП.СтрП”1л,1пппп,1пп,1п”ОВП.ОП”1ггг,1г,1гг,1гпп”ТП.ОТ”2лл,2лллс,2гг,2гг”ТП.РХБЗ”1лл,1гпп,1г,1гп”ТП.ВИП”2л,2г,2г,2гг,2г,2г”ТП.ВоенТоп”2г,2гг,2г,2г,2гп,2гп”ТП.ВМП”2ггпг”ТП.ОснВыж”2гг,2г,2г”ВСП.ВПР”2гггггг,2ппп”ВСП.МПД”3гг,3гг,3гг,3ггггг,3г”ВСП.ТСР”3г,3гпгпгпг”ВСП.Подг. по связи”3гп,3гп,3гп”ТСП.ТСП”3л,3лспг,3гг,3лгггс,3л,3лс4п,4л,4лллспп,4лл,4гпп,4л,4лс,4лп,4лгггс,4л”ТСП.ИА и ТВД”4л,4ллллс,4ггг,4ггг5лс,5л,5лллс,5лл,5л,5г,5л,5лк”ВДП”5гггк,5гпгпппп,5гг,5ггп";

        Direction direction = new();
        Guid directionId = direction.Id;

        var parts = Regex.Split(input, "“|”");
        for (int i = 1; i < parts.Length; i += 2)
        {
            string subjectName = parts[i].Trim();
            string rawLessons = parts[i + 1].Trim();
            Subject subject = new()
            {
                Name = subjectName,
                DirectionId = directionId
            };

            var lessonStrings = rawLessons.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int themeCounter = 1;
            Theme currentTheme = null;
            string currentNumber = null;

            for (int j = 0; j < lessonStrings.Length; j++)
            {
                string token = lessonStrings[j].Trim();
                if (string.IsNullOrEmpty(token)) continue;

                var match = Regex.Match(token, @"(\d+)([лпгкс]+)");
                if (!match.Success) continue;

                string numPart = match.Groups[1].Value;
                string letters = match.Groups[2].Value;

                
                    currentTheme = new Theme
                    {
                        Number = themeCounter++,
                        SubjectId = subject.Id
                    };
                    subject.Themes.Add(currentTheme);
                    currentNumber = numPart;
                

                for (int k = 0; k < letters.Length; k++)
                {
                    char ch = letters[k];
                    LessonType type = ch switch
                    {
                        'л' => LessonType.Lecture,
                        'п' => LessonType.Practice,
                        'г' => LessonType.Group,
                        'с' => LessonType.Seminar,
                        'к' => LessonType.Sum,
                        _ => LessonType.Group
                    };

                    Lesson lesson = new()
                    {
                        Number = currentTheme.Lessons.Count + 1,
                        Type = (int)type,
                        Semester = int.Parse(numPart.Substring(0, 1)),
                        SubjectId = subject.Id,
                        ThemeId = currentTheme.Id
                    };

                    currentTheme.Lessons.Add(lesson);
                }
            }

            direction.Subjects.Add(subject);
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(direction, options);
        Console.WriteLine(json);
    }
}
