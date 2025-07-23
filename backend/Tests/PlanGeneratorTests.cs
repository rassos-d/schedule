
namespace Tests
{
    [TestFixture]
    public class PlanGeneratorTests
    {
        PlanGenerator generator;

        [SetUp]
        public void Setup()
        {
            generator = new PlanGenerator();
        }

        [Test]
        public void GenerateTest()
        {
            generator.Generate();
        }
    }
}