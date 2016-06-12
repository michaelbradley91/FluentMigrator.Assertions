using System;
using FluentAssertions;
using FluentMigrator.Assertions.Tests.Helpers;
using FluentMigrator.Assertions.Tests.Setup;
using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Tests
{
    [TestFixture]
    public class ServerPrincipalAssertTests : IntegrationTestBase
    {
        [Test]
        public void AssertServerPrincipalExists_WhenTheServerPrincipalDoesExist_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.ServerPrincipalExists("sa");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertServerPrincipalExists_WhenTheServerPrincipalDoesNotExist_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.ServerPrincipalExists("Gdfsf");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }

        [Test]
        public void AssertServerPrincipalHasRole_WhenTheRoleHasBeenAssigned_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.ServerPrincipalExists("sa").WithRole("sysadmin");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertServerPrincipalHasRole_WhenTheRoleHasNotBeenAssigned_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.ServerPrincipalExists("sa").WithRole("sadsda");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }
    }
}
