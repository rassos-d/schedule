using System.Text.Json;
using Scheduler.DataAccess.Base;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository : BaseRepository
{
    private readonly GeneralData _data;
    private const string FilePath = "general.json";

    public GeneralRepository() : base(string.Empty)
    {
        _data = File.Exists(FilePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(FilePath), JsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    protected override void SaveChanges() => 
        File.WriteAllText(FilePath, JsonSerializer.Serialize(_data, JsonOptions));
}