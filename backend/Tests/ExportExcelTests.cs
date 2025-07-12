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
                // new GeneralRepository(),
                // new PlanRepository(),
                // new ScheduleRepository()
            );
        }

        [Test]
        public void Test1()
        {
            export.Save(Guid.NewGuid());
        }
    }
}
