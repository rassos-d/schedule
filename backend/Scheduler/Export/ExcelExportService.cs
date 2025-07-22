using System.Drawing;
using OfficeOpenXml;
using Scheduler.Dto.Constants;
using Scheduler.DataAccess;
using Scheduler.DataAccess.General;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.General;
using Scheduler.Entities.Schedule;
using Scheduler.Entities.Plan;
using Microsoft.OpenApi.Extensions;
using Scheduler.Extensions;

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

    public string Save(Guid scheduleId)
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
        return resultPath;
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
                Size = new Size(20, 9)
            }
        };
    }

    private void WriteSheet(
        ExcelWorkbook workbook,
        Template template,
        SchedulePage page)
    {
        // создаем лист сразу с шапкой
        var sheet = workbook.Worksheets.Add($"{(int)page.StudyYear} год", template.Header.Sheet);

        var squads = page.Squads.Select(squadId => GetSquad(page, squadRepository.Get(squadId)!));
        FillHeader(squads, sheet.Cells, page.Dates, page.Semester);

        var totalHeight = template.Header.Size.Height + 1;

        foreach (var squad in squads)
        {
            // ставим взвод
            template.Body.Range.Copy(sheet.Cells[totalHeight, 1]);
            FillSquad(squad, sheet.Cells, page.Dates, totalHeight);
            totalHeight += template.Body.Size.Height;
        }

        // ставим подвал
        template.Footer.Range.Copy(sheet.Cells[totalHeight, 1]);
        totalHeight += template.Footer.Size.Height;

        // устанавливаем область печати
        sheet.PrinterSettings.PrintArea = sheet.Cells[1, 1, totalHeight, template.Body.Size.Width];
        sheet.PrinterSettings.FitToPage = true;
    }

    private void FillHeader(IEnumerable<SquadExcel> squads, ExcelRange cells, List<DateOnly> dates, Semester semester)
    {
        const int vucsTextIndex = 71;
        const int semesterTextIndex = 74;
        const int yearsTextIndex = 84;

        var header = cells.TakeSingleCell(1, 0);
        var day = cells.TakeSingleCell(3, 0);

        var endYear = dates.First().Year;
        var startYear = (int)semester % 2 == 1 ? endYear - 1 : endYear + 1;
        (startYear, endYear) = endYear > startYear ? (startYear, endYear) : (endYear, startYear);

        var semesterText = (int)semester % 2 == 1 ? "весеннем" : "осеннем";
        var yearsText = $"{startYear}-{endYear}";
        var vucsText = string.Join(", ", squads
            .Select(x => x.DirectionName.Split('-').Last())
            .Where(x => !string.IsNullOrWhiteSpace(x)));
        var dayText = dates.First().DayOfWeek.ToRussian();

        header.SetCellValue(0, 0, header.Text
            .Insert(yearsTextIndex, yearsText)
            .Insert(semesterTextIndex, semesterText)
            .Insert(vucsTextIndex, vucsText));

        day.SetCellValue(0, 0, dayText);
    }

    private SquadExcel GetSquad(SchedulePage page, Squad squad)
    {
        var teacher = GetOrDefault(squad.DaddyId, teacherRepository.Get);
        var teacherRank = RankExpander.GetFullOrDefault(teacher?.Rank);
        var direction = GetOrDefault(squad.DirectionId, planRepository.GetDirection);

        return new SquadExcel()
        {
            Name = squad.Name,
            DirectionName = direction?.Name!,
            DaddyName = string.Join('\n', new[] { teacherRank, teacher?.Name }.Where(x => !string.IsNullOrEmpty(x))),
            Events = page.Events.Where(x => x.SquadId == squad.Id).ToList()
        };
    }

    private void FillSquad(SquadExcel squad, ExcelRange cells, List<DateOnly> dates, int heightOffset)
    {
        FillSquadName(squad, cells, heightOffset);

        const int colOffset = 3;
        var colByDate = dates
            .Zip(Enumerable.Range(colOffset, dates.Count + colOffset))
            .ToDictionary(x => x.First, x => x.Second);

        FillDates(cells, dates, heightOffset, colByDate);
        FillSquadEvents(squad, cells, heightOffset, colByDate);
    }

    private static void FillSquadName(SquadExcel squad, ExcelRange cells, int heightOffset)
    {
        var squadName = cells.TakeSingleCell(heightOffset, 0);
        AddFormattedText(squadName, "Взвод ", 36);
        AddFormattedText(squadName, $"{squad.Name}\n\n", 36);
        AddFormattedText(squadName, $"{squad.DirectionName}\n\n", 26);
        AddFormattedText(squadName, $"Ответственный\nпреподаватель\n", 22);
        AddFormattedText(squadName, $"{squad.DaddyName}", 22);

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

    private void FillSquadEvents(SquadExcel squad, ExcelRange cells, int heightOffset, Dictionary<DateOnly, int> colByDate)
    {
        const int eventOffset = 4;
        foreach (var @event in squad.Events)
        {
            if (@event.Date.HasValue && @event.Number.HasValue && colByDate.TryGetValue(@event.Date.Value, out var eventCol))
            {
                var eventLocalPos = eventOffset * (@event.Number.Value - 1);

                var subject = GetOrDefault(@event.SubjectId, planRepository.GetSubject);
                var audience = GetOrDefault(@event.AudienceId, audienceRepository.Get);
                var teacher = GetOrDefault(@event.TeacherId, teacherRepository.Get);

                var theme = GetOrDefault(@event.ThemeId, planRepository.GetTheme);
                var lesson = GetOrDefault(@event.LessonId, planRepository.GetLesson);
                var themeText = $"т.{theme?.Number}/{lesson?.Number} {lesson?.Type.GetView()}";

                cells.SetCellValue(heightOffset + eventLocalPos, eventCol, subject?.GetShortName());
                cells.SetCellValue(heightOffset + eventLocalPos + 1, eventCol, themeText);
                cells.SetCellValue(heightOffset + eventLocalPos + 2, eventCol, audience?.Name);
                cells.SetCellValue(heightOffset + eventLocalPos + 3, eventCol, string.Join(' ', new[] { teacher?.Rank, teacher?.Name }.Where(x => !string.IsNullOrEmpty(x))));
            }
        }
    }

    private static void FillDates(ExcelRange cells, List<DateOnly> dates, int heightOffset, Dictionary<DateOnly, int> colByDate)
    {
        for (var dateIndex = 0; dateIndex < dates.Count; dateIndex++)
        {
            var date = dates[dateIndex];
            var col = colByDate[date];
            cells.SetCellValue(heightOffset - 1, col, date.ToString("dd.MM"));
        }
    }

    private T1? GetOrDefault<T1, T2>(T2? input, Func<T2, T1> getter) where T2 : struct
        => input.HasValue ? getter(input.Value) : default;
}