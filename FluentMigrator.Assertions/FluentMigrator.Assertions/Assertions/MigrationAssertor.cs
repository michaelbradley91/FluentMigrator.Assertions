using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IMigrationAssertor
    {
        IStoredProcedureAssert StoredProcedureExists(string storedProcedureName);
    }

    public class MigrationAssertor : IMigrationAssertor
    {
        private readonly Migration migration;

        public MigrationAssertor(Migration migration)
        {
            this.migration = migration;
        }

        public IStoredProcedureAssert StoredProcedureExists(string storedProcedureName)
        {
            migration.Assert($"OBJECT_ID(N'{storedProcedureName.EscapeApostraphes().SurroundWithBrackets()}', N'P') IS NULL",
                             $"Stored procedure {storedProcedureName} does not exist");

            return new StoredProcedureAssert(migration, storedProcedureName);
        }
    }
}
