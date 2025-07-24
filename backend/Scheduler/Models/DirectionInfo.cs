using Scheduler.Dto.Constants;

namespace Scheduler.Models;

public record DirectionInfo(Guid Id, string Name, DirectionType Type);