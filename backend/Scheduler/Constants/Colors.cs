using System.Text.Json;

namespace Scheduler.Constants;

public static class Colors
{
    public static List<string> All()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), FilePaths.BaseFolder, FilePaths.ColorsFolder, FilePaths.ColorFilePath );
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        var text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<string>>(text, jsonOptions)!;
    }
}