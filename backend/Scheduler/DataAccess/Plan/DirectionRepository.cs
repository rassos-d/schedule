using Scheduler.Entities.Plan;
using System.Text.Json;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess.Plan
{ 
    public class DirectionRepository
    {
        private readonly string _directoryPath;
        private readonly Dictionary<Guid, Direction> _directionsCache = new();
        private readonly JsonSerializerOptions _jsonOptions;
        public DirectionRepository(string basePath = "data")
        {
            _directoryPath = Path.Combine(basePath, "directions");
            Directory.CreateDirectory(_directoryPath);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        private Direction? LoadDirection(Guid id)
        {
            var json = ReadFile($"{id}.json");
            return JsonSerializer.Deserialize<Direction>(json, _jsonOptions);
        }

        public Direction? GetDirection(Guid id)
        {
            if (_directionsCache.TryGetValue(id, out var cachedDirection))
            {
                return cachedDirection;
            }

            var direction = LoadDirection(id);
            if (direction == null)
            {
                return null;
            }

            _directionsCache[id] = direction;

            return direction;
        }

        public List<Direction>? GetAllDirections()
        {
            var json = ReadFile(DirectionsFilePath);
            return JsonSerializer.Deserialize<List<Direction>>(json, _jsonOptions);
        }

        public void SaveDirection(Direction direction)
        {
            var directionsJson = ReadFile(DirectionsFilePath);
            var directions = JsonSerializer.Deserialize<List<Direction>>(directionsJson, _jsonOptions);
            directions?.Add(direction); // добавляем направление в общий список направлений
            WriteFile(DirectionsFilePath, directions);

            _directionsCache[direction.Id] = direction;
            WriteFile($"{direction.Id}.json", _directionsCache);
        }

        public bool DeleteDirection(Guid id)
        {
            _directionsCache.Remove(id);
            var filePath = Path.Combine(_directoryPath, $"{id}.json");
            if (!File.Exists(filePath)) return false;

            File.Delete(filePath);
            return true;
        }

        private string ReadFile(string path)
        {
            var filePath = Path.Combine(_directoryPath, path);
            return !File.Exists(filePath) ? string.Empty : File.ReadAllText(filePath);
        }

        private void WriteFile(string path, object content)
        {
            var filePath = Path.Combine(_directoryPath, path);
            File.WriteAllText(filePath, JsonSerializer.Serialize(content, _jsonOptions));
        }
    }
}
