public class Lesson : Entity
{
    public string Name { get; set; }

    public LessonType Type { get; set; }

    public Guid ThemeId { get; set; }
}