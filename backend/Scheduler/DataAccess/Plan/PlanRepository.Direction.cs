using Scheduler.Entities.Plan;
using Scheduler.Models;
using System.Text.Json;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    public void SaveDirection(Direction direction)
    {
        var directions = GetAllDirectionInfos();
       
        Directions.RemoveAll(d => d.Id == direction.Id);
        directions.RemoveAll(d => d.Id == direction.Id);
        

        directions.Add(new DirectionInfo(direction.Id, direction.Name));

        WriteFile(DirectionsFilePath, directions);

        Directions.Add(direction);
        WriteFile($"{direction.Id}.json", direction);
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
        var json = ReadFile(DirectionsFilePath);
        return JsonSerializer.Deserialize<List<DirectionInfo>>(json, JsonOptions) ?? [];
    }

    public void DeleteDirection(Guid id)
    {
        var direction = GetDirection(id);
        Directions.RemoveAll(d => d.Id == direction.Id);
        
        var directions = GetAllDirectionInfos();
        directions.RemoveAll(d => d.Id == direction.Id);
        WriteFile(DirectionsFilePath, directions);

        var filePath = Path.Combine(DirectoryPath, $"{id}.json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}