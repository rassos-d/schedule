namespace Scheduler.Dto.Statistic.Squad;

public class SquadStatisticDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    
    public required Guid? TeacherId { get; init; }
    public required Guid? FixedAudienceId { get; init; }
    public required List<SubjectStatisticDto> Subjects { get; init; }
}