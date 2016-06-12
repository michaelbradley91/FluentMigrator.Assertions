using System;
using FluentAssertions;
using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Tests.Helpers;
using FluentMigrator.Assertions.Tests.Setup;
using NUnit.Framework;

namespace FluentMigrator.Assertions.Tests.Tests
{
    [TestFixture]
    public class FunctionAssertTests : IntegrationTestBase
    {
        [Test]
        public void AssertFunctionExists_WhenTheFunctionDoesExist_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.FunctionExists("Echo", FunctionType.Scalar);
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertFunctionExists_WhenTheFunctionDoesNotExist_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.FunctionExists("Gdfsf", FunctionType.InlineTableValued);
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }

        [Test]
        public void AssertFunctionWithDefinition_WhenTheDefinitionIsCorrect_DoesNothing()
        {
            RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                a.FunctionExists("Echo", FunctionType.Scalar).WithDefinition(@"CREATE FUNCTION Echo(	@Param int)RETURNS intASBEGIN	RETURN @ParamEND");
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
            }));
            SqlExecutor.TableExists("Hello").Should().BeTrue();
        }

        [Test]
        public void AssertFunctionWithDefinition_WhenTheDefinitionIsIncorrect_AbortsTheMigration()
        {
            Assert.Throws<Exception>(() => RunnerContainer.MigrateUp(new UpOnlyMigration((m, a) =>
            {
                m.Create.Table("Hello").WithColumn("Id").AsInt32().PrimaryKey();
                a.FunctionExists("Echo", FunctionType.Scalar).WithDefinition("dsdfdf");
                m.Create.Table("World").WithColumn("Id").AsInt32().PrimaryKey();
            })));
            SqlExecutor.TableExists("Hello").Should().BeFalse();
            SqlExecutor.TableExists("World").Should().BeFalse();
        }
    }
}
