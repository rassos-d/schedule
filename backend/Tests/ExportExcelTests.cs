using Scheduler.Export;
using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;
using Scheduler.DataAccess.General;

namespace Tests
{
    public class ExportExcelTests
    {
        ExcelExportService export;

        [SetUp]
        public void Setup()
        {
            export = new ExcelExportService(
                new ScheduleRepository(),
                new TeacherRepository(),
                new AudienceRepository(),
                new SquadRepository(),
                new PlanRepository()
            );
        }

        [Test]
        public void Test1()
        {
            export.Save(Guid.Parse("4b9121a8-92a6-48f0-b256-60ade7f9a03f"));
        }
    }
}
