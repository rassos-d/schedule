using System.Text.Json;
using static Scheduler.Constants.FilePaths;
namespace Scheduler.DataAccess.Base;

public abstract class BaseRepository
{
    protected readonly JsonSerializerOptions JsonOptions;
    protected readonly string DirectoryPath;
    protected BaseRepository(string directoryPath)
    {
        DirectoryPath = Path.Combine(BaseFolder, directoryPath);

        if (Directory.Exists(DirectoryPath) == false)
        {
            Directory.CreateDirectory(DirectoryPath);
        }
        
        JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }
    protected abstract void SaveChanges(Guid? id = null);

    protected string ReadFile(string path)
    {
        var filePath = Path.Combine(DirectoryPath, path);
        if (File.Exists(filePath) == false)
        {
            throw new FileNotFoundException();
        }
        
        return File.ReadAllText(filePath);
    }

    protected void WriteFile(string path, object text)
    {
        var filePath = Path.Combine(DirectoryPath, path);
        File.WriteAllText(filePath, JsonSerializer.Serialize(text, JsonOptions));
    }
}