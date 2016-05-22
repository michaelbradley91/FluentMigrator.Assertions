using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentMigrator.Assertions.Assertions;
using FluentMigrator.Assertions.Tests.Helpers;
using FluentMigrator.Assertions.Tests.Setup;
using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Tests
{
    [TestFixture]
    public class StoredProcedureTests : IntegrationTestBase
    {
        [Test]
        public void AssertStoredProcedureExists_WhenTheStoredProcedureDoesExist_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.StoredProcedureExists("CustOrderHist");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertStoredProcedureExists_WhenTheStoredProcedureDoesNotExist_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.StoredProcedureExists("Gdfsf");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
        }

        [Test]
        public void AssertStoredProcedureWithDefinition_WhenTheDefinitionIsCorrect_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.StoredProcedureExists("CustOrderHist").WithDefinition("FluentMigrator.Assertions.Tests.Scripts.CustOrderHistDefinition.sql");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertStoredProcedureWithDefinition_WhenTheDefinitionIsIncorrect_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.StoredProcedureExists("CustOrderHist").WithDefinition("FluentMigrator.Assertions.Tests.Scripts.CustOrderHistDefinitionWrong.sql");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
        }
    }
}
