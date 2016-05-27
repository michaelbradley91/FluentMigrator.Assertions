using FluentMigrator.Assertions.Migrations;

namespace FluentMigrator.Assertions.Tests.Examples
{
    public class UpdateStoredProcedureExample : MigrationWithAssertions
    {
        private const string ProcedureName = "MyProcedure";
        private const string NewProcedureDefinition = "NewProcedure.sql";
        private const string OldProcedureDefinition = "OldProcedure.sql";

        public override void Up()
        {
            Assert.StoredProcedureExists(ProcedureName).WithDefinition(OldProcedureDefinition);
            Execute.Sql($"DROP PROCEDURE {ProcedureName}");
            Execute.EmbeddedScript(NewProcedureDefinition);
        }

        public override void Down()
        {
            Assert.StoredProcedureExists(ProcedureName).WithDefinition(NewProcedureDefinition);
            Execute.Sql($"DROP PROCEDURE {ProcedureName}");
            Execute.EmbeddedScript(OldProcedureDefinition);
        }
    }
}
