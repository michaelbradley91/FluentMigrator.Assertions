using System;
using FluentAssertions;
using FluentMigrator.Assertions.Tests.Helpers;
using FluentMigrator.Assertions.Tests.Setup;
using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Tests
{
    [TestFixture]
    public class DatabasePrincipalAssertTests : IntegrationTestBase
    {
        [Test]
        public void AssertDatabasePrincipalExists_WhenTheDatabasePrincipalDoesExist_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.DatabasePrincipalExists("guest");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertDatabasePrincipalExists_WhenTheDatabasePrincipalDoesNotExist_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.DatabasePrincipalExists("Gdfsf");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }

        [Test]
        public void AssertDatabasePrincipalHasRole_WhenTheRoleHasBeenAssigned_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.DatabasePrincipalExists("dbo").WithRole("db_owner");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertDatabasePrincipalHasRole_WhenTheRoleHasNotBeenAssigned_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.DatabasePrincipalExists("guest").WithRole("sysadmin");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }
    }
}
