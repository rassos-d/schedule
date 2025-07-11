using System.Text.Json;
using Scheduler.DataAccess.Base;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository : BaseRepository
{
    private readonly GeneralData _data;
    private const string GeneralPath = "general.json";
    private readonly string _filePath;

    public GeneralRepository() : base(string.Empty)
    {
        _filePath = Path.Combine(DirectoryPath, GeneralPath);
        _data = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), JsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    public override void SaveChanges() => 
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, JsonOptions));
}