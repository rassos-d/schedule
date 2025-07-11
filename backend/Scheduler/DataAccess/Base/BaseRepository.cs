using System.Text.Json;

namespace Scheduler.DataAccess.Base;

public abstract class BaseRepository
{
    protected readonly JsonSerializerOptions JsonOptions;
    protected readonly string DirectoryPath;
    
    private const string BasePath = "data";

    protected BaseRepository(string directoryPath)
    {
        DirectoryPath = Path.Combine(BasePath, directoryPath);

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
    public abstract void SaveChanges();

    protected string ReadFile(string path)
    {
        var filePath = Path.Combine(DirectoryPath, path);
        return File.Exists(filePath) == false 
            ? string.Empty 
            : File.ReadAllText(filePath);
    }

    protected void WriteFile(string path, object text)
    {
        var filePath = Path.Combine(DirectoryPath, path);
        File.WriteAllText(filePath, JsonSerializer.Serialize(text, JsonOptions));
    }
}