using FluentMigrator.Assertions.Migrations;

namespace FluentMigrator.Assertions.Tests.Examples
{
    public class UpdateStoredProcedureExample : MigrationWithAssertions
    {
        private const string ProcedureName = "MyProcedure";
        private const string NewProcedureSql = "NewProcedure.sql";
        private const string OldProcedureSql = "OldProcedure.sql";

        public override void Up()
        {
            Assert.StoredProcedureExists(ProcedureName).WithDefinition(OldProcedureSql);
            Execute.Sql($"DROP PROCEDURE {ProcedureName}");
            Execute.EmbeddedScript(NewProcedureSql);
        }

        public override void Down()
        {
            Assert.StoredProcedureExists(ProcedureName).WithDefinition(NewProcedureSql);
            Execute.Sql($"DROP PROCEDURE {ProcedureName}");
            Execute.EmbeddedScript(OldProcedureSql);
        }
    }
}
