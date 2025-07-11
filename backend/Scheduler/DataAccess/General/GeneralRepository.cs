using System.Text.Json;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository
{
    private readonly string _filePath;
    private GeneralData _data;
    private readonly JsonSerializerOptions _jsonOptions;

    public GeneralRepository(string basePath = "data")
    {
        _filePath = Path.Combine(basePath, "general.json");
        Directory.CreateDirectory(basePath);
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        LoadData();
    }

    private void LoadData()
    {
        _data = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), _jsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    public void SaveChanges() => File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, _jsonOptions));
    
    public void SaveChangesToDisk() => SaveChanges();
}