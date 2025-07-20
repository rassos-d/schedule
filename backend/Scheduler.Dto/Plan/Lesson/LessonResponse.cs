namespace Scheduler.Dto.Lesson;

public class LessonResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Number { get; set; }

    public int SelfStudyHours { get; set; }
}