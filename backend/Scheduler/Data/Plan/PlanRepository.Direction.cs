using System.Text.Json;
using Scheduler.Models.Plan;

namespace Scheduler.Data.Plan;

public partial class PlanRepository
{
    public void SaveDirection(Direction direction)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{direction.Id}.json");
        string json = JsonSerializer.Serialize(direction, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public Direction? GetDirection(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{id}.json");
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Direction>(json, _jsonOptions);
    }

    public List<Direction> GetAllDirections()
    {
        return Directory.GetFiles(_directoryPath, "direction_*.json")
            .Select(file => JsonSerializer.Deserialize<Direction>(File.ReadAllText(file), _jsonOptions))
            .Where(d => d != null)
            .ToList()!;
    }

    public bool DeleteDirection(Guid id)
    {
        string filePath = Path.Combine(_directoryPath, $"direction_{id}.json");
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}