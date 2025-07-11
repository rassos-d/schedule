using System.Text.Json;

namespace Scheduler.DataAccess.Plan;

public partial class PlanRepository
{
    private readonly string _directoryPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public PlanRepository(string basePath = "data")
    {
        _directoryPath = Path.Combine(basePath, "plan");
        Directory.CreateDirectory(_directoryPath);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }
}