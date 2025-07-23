using System.Text.Json;
using Scheduler.Entities.Plan;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveDirection(Direction direction)
    {
        var directions = GetAllDirectionInfos();
        if (_directions.Any(d => d.Id == direction.Id))
        {
            directions.RemoveAll(d => d.Id == direction.Id);
            _directions.RemoveAll(d => d.Id == direction.Id);
        }

        _directions.Add(direction);
        WriteFile($"{direction.Id}.json", direction);
        
        directions.Add(new DirectionInfo(direction.Id, direction.Name));
        
        WriteFile(DirectionsFilePath, directions);
    }

    public Direction? GetDirection(Guid id)
    {
        var direction = _directions.FirstOrDefault(d => d.Id == id);
        if (direction is not null)
        {
            return direction;
        }
        
        var json = ReadFile($"{id}.json");
        direction = JsonSerializer.Deserialize<Direction>(json, JsonOptions);

        if (direction is not null)
        {
            _directions.Add(direction);
        }
        
        return direction;
    }

    public List<DirectionInfo> GetAllDirectionInfos()
    {
        var json = ReadFile(DirectionsFilePath);
        return JsonSerializer.Deserialize<List<DirectionInfo>>(json, JsonOptions) ?? [];
    }

    public void DeleteDirection(Guid id)
    {
        var direction = GetDirection(id);
        if (direction is not null)
        {
            _directions.RemoveAll( d => d.Id == direction.Id);
        }
        
        var filePath = Path.Combine(DirectoryPath, $"{id}.json");
        if (File.Exists(filePath) == false)
        {
            return;
        }

        File.Delete(filePath);
    }
}