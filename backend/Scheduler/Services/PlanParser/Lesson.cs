/// <summary>
/// Учебное занятие с его характеристиками
/// </summary>
public record Lesson(
    int lesson_number,
    int local_number,
    string lesson_type,
    string title,
    float hours,
    float self_study_hours = 0f
    )
{
    public override string ToString() =>
        $"{lesson_type} №{lesson_number} ({local_number}): {title} [Ауд: {hours}ч, Сам: {self_study_hours}ч]";
}