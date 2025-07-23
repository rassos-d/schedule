/// <summary>
/// Весь учебный план с иерархией разделов, тем и занятий
/// </summary>
public class Curriculum
{
    private List<Section> sections;
    private Dictionary<string, Topic> topicMap;

    public Curriculum()
    {
        sections = new List<Section>();
        topicMap = new Dictionary<string, Topic>();
    }

    /// <summary>
    /// Добавляет раздел в учебный план
    /// </summary>
    public void AddSection(Section section)
    {
        sections.Add(section);
    }

    /// <summary>
    /// Находит тему по идентификаторам.
    /// </summary>
    public Topic? FindTopic(int topic_number, string title, int? semester = null)
    {
        if (semester.HasValue)
        {
            return topicMap.TryGetValue($"{semester}_{topic_number}_{title}", out var topic) ? topic : null;
        }

        foreach (var section in sections)
        {
            foreach (var topic in section.topics)
            {
                if (topic.topic_number == topic_number && topic.title == title)
                    return topic;
            }
        }

        return null;
    }

    public override string ToString() => $"Тематический план (всего разделов: {sections.Count})";
}