/// <summary>
/// Парсер учебного плана из документов формата DOCX
/// </summary>
public class PlanParser
{
    private object curriculum;
    private Section? current_section;
    private Topic? current_topic;
    private Section? current_semester;
    private int? last_semester;
    private HashSet<Topic> processed_topics;
    private Dictionary<string, int> column_indices;

    public PlanParser()
    {
        curriculum = new Curriculum();
        current_section = null;
        current_topic = null;
        current_semester = null;  // Текущий активный семестр
        last_semester = null;  // Последний известный семестр
        processed_topics = new HashSet<Topic>();
        column_indices = new Dictionary<string, int>();  // Для хранения индексов столбцов по названиям
    }
}