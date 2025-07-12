using OfficeOpenXml;
using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;

namespace Scheduler.Export;

public class ExcelExportService
{
    private readonly string templatePath = Path.Combine("Export", "template.xlsx");

    private readonly GeneralRepository _generalRepository;
    private readonly PlanRepository _planRepository;
    private readonly ScheduleRepository _scheduleRepository;

    public ExcelExportService(
        GeneralRepository generalRepository,
        PlanRepository planRepository,
        ScheduleRepository scheduleRepository
        )
    {
        ExcelPackage.License.SetNonCommercialPersonal("VUC");
        _generalRepository = generalRepository;
        _planRepository = planRepository;
        _scheduleRepository = scheduleRepository;
    }

    public void Save(Guid scheduleId)
    {
        if (!File.Exists(templatePath))
            throw new FileNotFoundException(new FileInfo(templatePath).FullName);

        using var templateExcel = new ExcelPackage(templatePath);
        using var resultExcel = new ExcelPackage("result.xlsx");

        resultExcel.Workbook.Worksheets.Add("1", templateExcel.Workbook.Worksheets[0]);


        resultExcel.Save();
        // var schedule = _scheduleRepository.GetSchedule(scheduleId);
        // foreach (var e in schedule.Events)
        // {
        // }
    }
}