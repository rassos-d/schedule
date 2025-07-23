using System.Drawing;
using OfficeOpenXml;

namespace Scheduler.Services.Export;

internal record Template
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