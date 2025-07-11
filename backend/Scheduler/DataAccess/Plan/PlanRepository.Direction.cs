using System.Text.Json;
using Scheduler.Entities.Plan;
using Scheduler.Models;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveDirection(Direction direction)
    {
        Directions.Add(direction);
        WriteFile($"{direction.Id}.json", direction);
        
        var directions = GetAllDirectionInfos();
        directions.Add(new DirectionInfo(direction.Id, direction.Name));
        
        WriteFile(DirectionsPath, directions);
    }

    public Direction? GetDirection(Guid id)
    {
        var direction = Directions.FirstOrDefault(d => d.Id == id);
        if (direction is not null)
        {
            return direction;
        }
        
        var json = ReadFile($"{id}.json");
        direction = JsonSerializer.Deserialize<Direction>(json, JsonOptions);

        if (direction is not null)
        {
            Directions.Add(direction);
        }
        
        return direction;
    }

    public List<DirectionInfo> GetAllDirectionInfos()
    {
        var json = ReadFile(DirectionsPath);
        return JsonSerializer.Deserialize<List<DirectionInfo>>(json, JsonOptions) ?? [];
    }

    public bool DeleteDirection(Guid id)
    {
        var direction = GetDirection(id);
        Directions.Remove(direction);
        
        var filePath = Path.Combine(DirectoryPath, $"{id}.json");
        if (File.Exists(filePath) == false)
        {
            return false;
        }

        File.Delete(filePath);
        return true;
    }
}