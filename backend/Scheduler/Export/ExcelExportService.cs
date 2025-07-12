using System.Drawing;
using OfficeOpenXml;
using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;

namespace Scheduler.Export;

public class ExcelExportService
{
    private readonly string templatePath = Path.Combine("Export", "template.xlsx");

    // private readonly GeneralRepository _generalRepository;
    // private readonly PlanRepository _planRepository;
    // private readonly ScheduleRepository _scheduleRepository;

    public ExcelExportService(
        // GeneralRepository generalRepository,
        // PlanRepository planRepository,
        // ScheduleRepository scheduleRepository
        )
    {
        ExcelPackage.License.SetNonCommercialPersonal("VUC");
        // _generalRepository = generalRepository;
        // _planRepository = planRepository;
        // _scheduleRepository = scheduleRepository;
    }

    public void Save(Guid scheduleId)
    {
        const string resultPath = "result.xlsx";

        if (!File.Exists(templatePath))
            throw new FileNotFoundException(new FileInfo(templatePath).FullName);

        if (File.Exists(resultPath))
            File.Delete(resultPath);

        using var templateExcel = new ExcelPackage(templatePath);
        using var resultExcel = new ExcelPackage(resultPath);

        var templateHeader = templateExcel.Workbook.Worksheets[0];
        var headerSize = new Size(20, 4);
        var templateSquad = templateExcel.Workbook.Worksheets[1].Cells["A1:T22"];
        var squadSize = new Size(20, 22);
        var templateFooter = templateExcel.Workbook.Worksheets[2].Cells["A1:T12"];
        var footerSize = new Size(20, 12);

        // создаем лист с сразу заполненной шапкой
        var sheet = resultExcel.Workbook.Worksheets.Add("1", templateHeader);
        var totalHeight = headerSize.Height;

        // ставим взвод
        templateSquad.Copy(sheet.Cells[$"A{totalHeight}"]);
        totalHeight += squadSize.Height;

        // ставим 2 взвод
        templateSquad.Copy(sheet.Cells[$"A{totalHeight}"]);
        totalHeight += squadSize.Height;

        // ставим подвал
        templateFooter.Copy(sheet.Cells[$"A{totalHeight}"]);
        totalHeight += footerSize.Height;

        // var schedule = _scheduleRepository.GetSchedule(scheduleId);
        // foreach (var e in schedule.Events)
        // {
        // }
        sheet.PrinterSettings.PrintArea = sheet.Cells[$"A1:T{totalHeight}"];
        resultExcel.Save();
    }

    private void WriteSquad()
    {
        
    }
}