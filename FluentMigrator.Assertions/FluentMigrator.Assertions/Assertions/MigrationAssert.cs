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
            var safeStoredProcedureName = StringHelpers.SurroundWithBrackets(storedProcedureName);
            var errorMessage = AssertionHelpers.CreateRaiseErrorSql($"Stored procedure {safeStoredProcedureName} does not exist");
            migration.Execute.Sql($"IF OBJECT_ID(N'{safeStoredProcedureName}', N'P') IS NULL " +
                                  $"BEGIN {errorMessage} END;");

            return new StoredProcedureAssert(migration, storedProcedureName);
        }
    }
}
