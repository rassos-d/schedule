namespace Scheduler.Dto.General.Squad;

public class SquadRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? DirectionId { get; set; }
}