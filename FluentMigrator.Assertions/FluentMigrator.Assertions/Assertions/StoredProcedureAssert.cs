using System.CodeDom;
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
            var escapedEmbeddedDefinition = migration.GetEmbeddedResource(embeddedStoredProcedureDefinition).EscapeApostraphes();
            var escapedStoredProcedureName = storedProcedureName.EscapeApostraphes().SurroundWithBrackets();

            var storedDefinitonSql = SqlHelpers.CreateRemoveNewLinesSql($"OBJECT_DEFINITION(OBJECT_ID(N'{escapedStoredProcedureName}'))");
            var embeddedDefinitionSql = SqlHelpers.CreateRemoveNewLinesSql($"'{escapedEmbeddedDefinition}'");

            migration.Assert($"{storedDefinitonSql} != {embeddedDefinitionSql}",
                             $"Stored procedure {storedProcedureName} definition did not match.");
        }
    }
}
