using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IMigrationAssert
    {
        IStoredProcedureAssert StoredProcedureExists(string storedProcedureName);
    }

    public class MigrationAssert : IMigrationAssert
    {
        private readonly Migration migration;

        public MigrationAssert(Migration migration)
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
