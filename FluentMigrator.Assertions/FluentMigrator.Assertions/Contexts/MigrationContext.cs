using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Contexts
{
    public class MigrationContext
    {
        public Migration Migration { get; }

        public MigrationContext(Migration migration)
        {
            Migration = migration;
        }

        protected MigrationContext(MigrationContext existingContext) : this(existingContext.Migration) { }
        
        public void AssertDefinitionsAreEqual(string objectName, ObjectType objectType, string embeddedDefinition)
        {
            var escapedEmbeddedDefinition = GetEmbeddedResource(embeddedDefinition).EscapeApostraphes();
            var escapedObjectName = objectName.EscapeApostraphes().SurroundWithBrackets();

            var storedDefinitonSql = SqlHelpers.CreateRemoveNewLinesSql($"OBJECT_DEFINITION(OBJECT_ID(N'{escapedObjectName}', " +
                                                                        $"N'{objectType.ToSqlIdentifier()}'))");
            var embeddedDefinitionSql = SqlHelpers.CreateRemoveNewLinesSql($"'{escapedEmbeddedDefinition}'");

            Assert($"{storedDefinitonSql} != {embeddedDefinitionSql}",
                   $"The definition of object {objectName} did not match the definition in the embedded resource.");
        }

        public void AssertObjectExists(string objectName, ObjectType objectType)
        {
            Assert($"OBJECT_ID(N'{objectName.EscapeApostraphes().SurroundWithBrackets()}', " +
                   $"N'{objectType.ToSqlIdentifier()}') IS NULL",
                   $"Object {objectName} does not exist");
        }

        public string GetEmbeddedResource(string resourceName)
        {
            return Migration.GetEmbeddedResource(resourceName);
        }

        public void Assert(string condition, string errorMessage)
        {
            Migration.Assert(condition, errorMessage);
        }
    }
}
