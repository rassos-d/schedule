//using Xceed.Document.NET;
//using Xceed.Words.NET;

///// <summary>
///// Парсер учебного плана из документов формата DOCX
///// </summary>
//public class PlanParser
//{
//    private object curriculum;
//    private Section? current_section;
//    private Topic? current_topic;
//    private Section? current_semester;
//    private int? last_semester;
//    private HashSet<Topic> processed_topics;
//    private Dictionary<string, int> column_indices;

//    public PlanParser()
//    {
//        curriculum = new Curriculum();
//        current_section = null;
//        current_topic = null;
//        current_semester = null;  // Текущий активный семестр
//        last_semester = null;  // Последний известный семестр
//        processed_topics = new HashSet<Topic>();
//        column_indices = new Dictionary<string, int>();  // Для хранения индексов столбцов по названиям
//    }

//    /// <summary>
//    /// Проверяет, что таблица содержит 6 или 7 столбцов (основной вариант)
//    /// </summary>
//    public bool IsValidTable(Table table)
//    {
//        var a = DocX.Create();
//        a.Tables[0].Rows[0].Cells[0].get
//        if (table.ColumnCount != 6 && table.ColumnCount != 7)
//            return false;

//        // Дополнительная проверка по содержимому первых строк
//        var first_row_text = string.Join(' ', table.Rows[0].Cells.Select(c => c));

//         ' '.join(cell.text.strip() for cell in table.rows[0].cells)
//        flag = any(keyword in first_row_text.lower()
//                 for keyword in ['количество'])
//        return flag       
//    }
    
//     def is_valid_table(self, table):
//        if len(table.columns) not in [6, 7]:
//            return False

//        # Дополнительная проверка по содержимому первых строк
//        first_row_text = ' '.join(cell.text.strip() for cell in table.rows[0].cells)
//        flag = any(keyword in first_row_text.lower()
//                 for keyword in ['количество'])
//        return flag
//}