using System.Text.Json;
using Scheduler.DataAccess.Base;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository : BaseRepository
{
    private readonly GeneralData _data;
    private readonly string _filePath;

    public GeneralRepository() : base(string.Empty)
    {
        _filePath = Path.Combine(DirectoryPath, GeneralFilePath);
        _data = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), JsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    public override void SaveChanges() => 
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, JsonOptions));
}