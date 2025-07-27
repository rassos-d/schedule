using Scheduler.Dto.Base;
using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Direction;

public record DirectionCreateDto : EntityWithNameCreateDto
{
    public DirectionType Type { get; set; }
}