using Scheduler.Export;
using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;

namespace Tests
{
    public class ExportExcelTests
    {
        ExcelExportService export;

        [SetUp]
        public void Setup()
        {
            export = new ExcelExportService(
                new ScheduleRepository()
            );
        }

        [Test]
        public void Test1()
        {
            export.Save(Guid.Parse("f3f9f8cc-ec27-4c99-b1ea-05ab2f6cefee"));
        }
    }
}
