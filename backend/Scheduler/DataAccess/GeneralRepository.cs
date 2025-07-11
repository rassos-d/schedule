using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Entities.General;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess;

public class GeneralRepository : BaseRepository
{
    private readonly GeneralData _data;
    private readonly string _filePath;

    public readonly DictionaryRepository<Audience> Audiences;
    public readonly DictionaryRepository<Squad> Squads;
    public readonly DictionaryRepository<Teacher> Teachers;

    public GeneralRepository() : base(string.Empty)
    {
        _filePath = Path.Combine(DirectoryPath, GeneralFilePath);
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