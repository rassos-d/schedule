public class Teacher : Entity
{
    public string Name { get; set; }

    public string Rank { get; set; }

    public List<Tuple<DateTime, DateTime>> Vacations { get; set; } = [];

    public List<Guid> SubjectIds { get; set; } = [];
}