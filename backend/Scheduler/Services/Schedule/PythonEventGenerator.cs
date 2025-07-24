using System.Diagnostics;
using Scheduler.Entities.Schedule;
using Scheduler.Exceptions;

namespace Scheduler.Services.Schedule;

public class PythonEventGenerator
{
    public void Generate(SchedulePage page)
    {
        var generalDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.FilePaths.BaseFolder, Constants.FilePaths.GeneralFilePath);
        var scheduleFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.FilePaths.BaseFolder, $"plan", $"{page.ScheduleId}", $"{page.StudyYear}.json" );
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = $"{Directory.GetCurrentDirectory() + Constants.FilePaths.GeneratorFilePath}.exe",
                Arguments = $"--arg1 {generalDataFilePath} --arg2 {scheduleFilePath}",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            }
        };

        process.Start();

        process.WaitForExit();
        if (process.ExitCode != 0)
            throw new AlgorithmDontWorkException("Алгоритм не смог отработать и завершился с ошибкой");
    }
}