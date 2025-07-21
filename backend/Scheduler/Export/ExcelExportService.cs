using System.Drawing;
using OfficeOpenXml;
using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities;
using Scheduler.Entities.General;
using Scheduler.Entities.Schedule;

namespace Scheduler.Export;

public class ExcelExportService
{

    private const string resultPath = "result.xlsx";
    private readonly string templatePath = Path.Combine("Export", "template.xlsx");
    private readonly ScheduleRepository scheduleRepository;
    private readonly TeacherRepository teacherRepository;
    private readonly AudienceRepository audienceRepository;
    private readonly SquadRepository squadRepository;
    private readonly PlanRepository planRepository;

    public ExcelExportService(
        ScheduleRepository scheduleRepository,
        TeacherRepository teacherRepository,
        AudienceRepository audienceRepository,
        SquadRepository squadRepository,
        PlanRepository planRepository
        )
    {
        ExcelPackage.License.SetNonCommercialPersonal("VUC");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException(new FileInfo(templatePath).FullName);

        this.scheduleRepository = scheduleRepository;
        this.teacherRepository = teacherRepository;
        this.audienceRepository = audienceRepository;
        this.squadRepository = squadRepository;
        this.planRepository = planRepository;
    }

    public Stream Save(Guid scheduleId)
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
        return resultExcel.Stream;
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
        var totalHeight = template.Header.Size.Height + 1;

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
            DaddyName = string.Join('\n', new[] { teacher?.Rank, teacher?.Name }.Where(x => !string.IsNullOrEmpty(x))),
            Events = page.Events.Where(x => x.SquadId == squad.Id).ToList()
        };
    }

    private void FillSquad(SquadExcel squad, ExcelRange cells, int heightOffset)
    {
        var squadName = cells.TakeSingleCell(heightOffset, 0);
        AddFormattedText(squadName, "Взвод ", 36);
        AddFormattedText(squadName, $"{squad.Name}\n\n", 36);
        AddFormattedText(squadName, $"{squad.DirectionName}\n\n", 26);
        AddFormattedText(squadName, $"Ответственный\nпреподаватель\n", 22);
        AddFormattedText(squadName, $"{squad.DaddyName}", 22);

        var col = 3;
        var colByDate = new Dictionary<DateOnly, int>();
        for (var dateIndex = 0; dateIndex < squad.Dates.Count; dateIndex++)
        {
            var date = squad.Dates[dateIndex];
            colByDate.Add(date, col);
            cells.SetCellValue(heightOffset - 1, col, date.ToString("dd.MM"));
            col++;
        }

        const int eventOffset = 4;
        foreach (var @event in squad.Events)
        {
            if (@event.Date.HasValue && @event.Number.HasValue && colByDate.TryGetValue(@event.Date.Value, out var eventCol))
            {
                var eventLocalPos = eventOffset * (@event.Number.Value - 1);

                var subject = @event.SubjectId.HasValue ? planRepository.GetSubject(@event.SubjectId.Value) : null;
                var theme = @event.ThemeId.HasValue ? planRepository.GetTheme(@event.ThemeId.Value) : null;
                var audience = @event.AudienceId.HasValue ? audienceRepository.Get(@event.AudienceId.Value) : null;
                var teacher = @event.TeacherId.HasValue ? teacherRepository.Get(@event.TeacherId.Value) : null;

                cells.SetCellValue(heightOffset + eventLocalPos, eventCol, subject?.Name);
                cells.SetCellValue(heightOffset + eventLocalPos + 1, eventCol, theme?.Name);
                cells.SetCellValue(heightOffset + eventLocalPos + 2, eventCol, audience?.Name);
                cells.SetCellValue(heightOffset + eventLocalPos + 3, eventCol, string.Join(' ', new[] { teacher?.Rank, teacher?.Name }.Where(x => !string.IsNullOrEmpty(x))));
            }
        }
        
        void AddFormattedText(ExcelRangeBase cell, string? text = null, float? size = null)
        {
            const string empty = "НЕ ЗАДАНО";
            if (string.IsNullOrWhiteSpace(text))
            {
                cell.RichText.Add(empty).Color = Color.Red;
            }

            var richText = cell.RichText.Add(text);
            if (size.HasValue)
            {
                richText.Size = size.Value;
            }
        }
    }

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