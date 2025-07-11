using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository : BaseRepository
{
    private readonly GeneralData _data;
    private const string GeneralPath = "general.json";
    private readonly string _filePath;

    public DictionaryRepository<Audience> Audiences;
    public DictionaryRepository<Squad> Squads;
    public DictionaryRepository<Teacher> Teachers;

    public GeneralRepository() : base(string.Empty)
    {
        _filePath = Path.Combine(DirectoryPath, GeneralPath);
        _data = File.Exists(_filePath)
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), JsonOptions) ?? new GeneralData()
            : new GeneralData();

        Audiences = new DictionaryRepository<Audience>(_data.Audiences);
        Squads = new DictionaryRepository<Squad>(_data.Squads);
        Teachers = new DictionaryRepository<Teacher>(_data.Teachers);
    }

    public override void SaveChanges() => 
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, JsonOptions));
}