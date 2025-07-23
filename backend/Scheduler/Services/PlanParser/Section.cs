/// <summary>
/// Раздел учебного плана, содержащий темы
/// </summary>
public class Section
{
    public string title { get; private set; }
    public List<Topic> topics { get; private set; }

    public Section(string title)
    {
        this.title = title;
        topics = new List<Topic>();
    }

    /// <summary>
    /// Добавляет тему в раздел
    /// </summary>
    public void AddTopic(Topic topic)
    {
        topics.Add(topic);
    }

    public override string ToString() => $"Раздел: {title}";
}