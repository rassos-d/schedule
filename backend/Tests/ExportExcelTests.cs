using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;
using Scheduler.DataAccess.General;
using Scheduler.Services.Export;

namespace Tests
{
    [TestFixture]
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
        public void ExportTest()
        {
            export.Save(Guid.Parse("42635d14-43e2-490e-96ce-0e4fa6c4afa1"));
        }
    }
}
