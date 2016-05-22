using System.Linq;
using System.Reflection;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IStoredProcedureAssert
    {
        void WithDefinition(string embeddedStoredProcedureDefinition);
    }

    public class StoredProcedureAssert : IStoredProcedureAssert
    {
        private readonly Migration migration;
        private readonly string storedProcedureName;
        
        public StoredProcedureAssert(Migration migration, string storedProcedureName)
        {
            this.migration = migration;
            this.storedProcedureName = storedProcedureName;
        }

        public void WithDefinition(string embeddedStoredProcedureDefinition)
        {
            var definition = migration.GetType().Assembly.GetEmbeddedResource(embeddedStoredProcedureDefinition);
            var escapedDefinition = definition.Replace("'", "''");
            var safeStoredProcedureName = StringHelpers.SurroundWithBrackets(storedProcedureName);
            var errorMessage = AssertionHelpers.CreateRaiseErrorSql($"Stored procedure {safeStoredProcedureName} definition did not match.");
            migration.Execute.Sql($"IF REPLACE(REPLACE(OBJECT_DEFINITION(OBJECT_ID(N'{safeStoredProcedureName}')),CHAR(10),''),CHAR(13),'') != REPLACE(REPLACE('{escapedDefinition}',CHAR(10),''),CHAR(13),'') " +
                                  $"BEGIN {errorMessage} END;");
        }
    }
}
