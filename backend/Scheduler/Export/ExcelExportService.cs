using System.Drawing;
using OfficeOpenXml;
using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities;
using Scheduler.Entities.General;
using Scheduler.Entities.Plan;
using Scheduler.Entities.Schedule;

namespace Scheduler.Export;

public class ExcelExportService
{

    private const string resultPath = "result.xlsx";
    private readonly string templatePath = Path.Combine("Export", "template.xlsx");
    private readonly ScheduleRepository scheduleRepository;
    private readonly TeacherRepository teacherRepository;
    private readonly SquadRepository squadRepository;
    private readonly PlanRepository planRepository;

    public ExcelExportService(
        ScheduleRepository scheduleRepository,
        TeacherRepository teacherRepository,
        SquadRepository squadRepository,
        PlanRepository planRepository
        )
    {
        ExcelPackage.License.SetNonCommercialPersonal("VUC");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException(new FileInfo(templatePath).FullName);

        this.scheduleRepository = scheduleRepository;
        this.teacherRepository = teacherRepository;
        this.squadRepository = squadRepository;
        this.planRepository = planRepository;
    }

    public void Save(Guid scheduleId)
    {
        if (File.Exists(resultPath))
            File.Delete(resultPath);

        using var templateExcel = new ExcelPackage(templatePath);
        var template = GetTemplate(templateExcel);

        var schedule = scheduleRepository.GetSchedule(scheduleId);
        using var resultExcel = new ExcelPackage(resultPath);

        foreach (var page in schedule.Pages)
        {
            WriteSheet(resultExcel.Workbook, template, page);
        }

        resultExcel.Save();
    }

    private static Template GetTemplate(ExcelPackage templateExcel)
    {
        return new Template
        {
            Header = new Template.TemplateElement
            {
                Sheet = templateExcel.Workbook.Worksheets[0],
                Range = null!,
                Size = new Size(20, 4)
            },
            Body = new Template.TemplateElement
            {
                Sheet = null!,
                Range = templateExcel.Workbook.Worksheets[1].Cells[1, 1, 22, 20],
                Size = new Size(20, 22)
            },
            Footer = new Template.TemplateElement
            {
                Sheet = null!,
                Range = templateExcel.Workbook.Worksheets[2].Cells[1, 1, 12, 20],
                Size = new Size(20, 12)
            }
        };
    }

    private void WriteSheet(
        ExcelWorkbook workbook,
        Template template,
        SchedulePage page)
    {
        // создаем лист с сразу заполненной шапкой
        var sheet = workbook.Worksheets.Add(page.StudyYear.ToString(), template.Header.Sheet);
        var totalHeight = template.Header.Size.Height;

        foreach (var squadId in page.Squads)
        {
            // ставим взвод
            template.Body.Range.Copy(sheet.Cells[totalHeight, 1]);
            var squad = squadRepository.Get(squadId);
            FillSquad(GetSquad(page, squad!), sheet.Cells, totalHeight);
            totalHeight += template.Body.Size.Height;
        }

        // ставим подвал
        template.Footer.Range.Copy(sheet.Cells[totalHeight, 1]);
        totalHeight += template.Footer.Size.Height;

        // устанавливаем область печати
        sheet.PrinterSettings.PrintArea = sheet.Cells[1, 1, totalHeight, template.Body.Size.Width];
    }

    private SquadExcel GetSquad(SchedulePage page, Squad squad)
    {
        var teacher = squad.DaddyId.HasValue ? teacherRepository.Get(squad.DaddyId.Value) : null;
        var direction = squad.DirectionId.HasValue ? planRepository.GetDirection(squad.DirectionId.Value) : null;
        return new SquadExcel()
        {
            Name = squad.Name,
            DirectionName = direction?.Name!,
            Dates = page.Dates,
            DaddyName = teacher?.Name!,
            Events = page.Events.Where(x => x.SquadId == squad.Id).ToList()
        };
    }

    private void FillSquad(SquadExcel squad, ExcelRange cells, int heightOffset)
    {
        cells.SetCellValue(heightOffset, 0, GetSquadName(squad));
        var col = 3;
        for (var dateIndex = 0; dateIndex < squad.Dates.Count; dateIndex++)
        {
            var date = squad.Dates[dateIndex];
            cells.SetCellValue(heightOffset - 1, col, $"{date.Day}.{date.Month}");
            col++;
        }
    }

    private string GetSquadName(SquadExcel squad) =>
    $"Взвод {squad.Name}\n\n{squad.DirectionName}\n\nОтветственный\nпреподаватель\nподполковник\n{squad.DaddyName}";

    private record Template
    {
        public required TemplateElement Header { get; init; }
        public required TemplateElement Body { get; init; }
        public required TemplateElement Footer { get; init; }

        public record TemplateElement
        {
            public required ExcelWorksheet Sheet { get; init; }
            public required ExcelRange Range { get; init; }
            public required Size Size { get; init; }
        }
    }

    private record SquadExcel
    {
        public required List<DateOnly> Dates { get; init; }
        public required string Name { get; init; }
        public required string DirectionName { get; init; }
        public required string DaddyName { get; init; }
        public required List<Event> Events { get; init; }
    }
}