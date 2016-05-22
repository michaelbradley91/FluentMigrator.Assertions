using NUnit.Framework;

namespace FluentMigrator.Helpers.Tests.Setup
{
    public class IntegrationTestBase
    {
        protected RunnerContainer RunnerContainer;

        [SetUp]
        public void SetUpRunnerContainer()
        {
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
