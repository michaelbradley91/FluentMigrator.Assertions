using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Setup
{
    public class IntegrationTestBase
    {
        protected RunnerContainer RunnerContainer;

        [SetUp]
        public void SetUpRunnerContainer()
        {
            SqlExecutor.RestoreDatabase();
            RunnerContainer = RunnerContainer.Create();
        }

        [TearDown]
        public void DisposeRunnerContainerAndRestoreDatabase()
        {
            RunnerContainer.Dispose();
            SqlExecutor.RestoreDatabase();
        }
    }
}
