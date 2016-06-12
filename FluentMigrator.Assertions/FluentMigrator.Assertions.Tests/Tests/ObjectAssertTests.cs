using System;
using FluentAssertions;
using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Tests.Helpers;
using FluentMigrator.Assertions.Tests.Setup;
using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Tests
{
    [TestFixture]
    public class ObjectAssertTests : IntegrationTestBase
    {
        [Test]
        public void AssertObjectExists_WhenTheObjectDoesExist_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.ObjectExists("CustOrderHist", ObjectType.StoredProcedure);
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertObjectExists_WhenTheObjectDoesNotExist_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.ObjectExists("Gdfsf", ObjectType.StoredProcedure);
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }

        [Test]
        public void AssertObjectWithDefinition_WhenTheDefinitionIsCorrect_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.ObjectExists("CustOrderHist", ObjectType.StoredProcedure).WithDefinitionFromEmbeddedResource("CustOrderHistDefinition.sql");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertObjectWithDefinition_WhenTheDefinitionIsIncorrect_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.ObjectExists("CustOrderHist", ObjectType.StoredProcedure).WithDefinitionFromEmbeddedResource("CustOrderHistDefinitionWrong.sql");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }
    }
}
