/// <summary>
/// Учебная тема, содержащая занятия
/// </summary>
public class Topic
{
    public int topic_number { get; private set; }
    public string title { get; private set; }
    public int? semester { get; private set; }
    public List<Lesson> lessons { get; private set; }
    public string topic_key { get; private set; }

    public Topic(int topic_number, string title, int? semester)
    {
        this.topic_number = topic_number;
        this.title = title;
        this.semester = semester;  // Семестр теперь принадлежит теме
        this.lessons = new List<Lesson>();
        this.topic_key = string.Empty; // Уникальный ключ для идентификации
    }

    /// <summary>
    /// Устанавливает семестр изучения темы и генерирует уникальный ключ.
    /// </summary>
    public void SetSemester(int semester)
    {
        this.semester = semester;
        topic_key = $"{semester}_{topic_number}_{title}";
    }

    /// <summary>
    /// Добавляет занятие в тему.
    /// </summary>
    public void AddLesson(Lesson lesson)
    {
        lessons.Add(lesson);
    }

    public override string ToString()
    {
        var semester_info = semester.HasValue ? $" (Семестр {semester})" : " (Семестр не указан)";
        return $"Тема {topic_number}: {title}{semester_info}";
    }
}