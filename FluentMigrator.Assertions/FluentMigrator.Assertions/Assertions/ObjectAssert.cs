using System.IO;
using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Contexts;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IObjectAssert
    {
        void WithDefinition(string embeddedDefinition);
        void WithDefinitionFromString(string definition);
        void WithDefinitionFromEmbeddedResource(string embeddedDefinition);
        void WithDefinitionFromFile(string filePath);
    }

    public class ObjectAssert : IObjectAssert
    {
        private readonly ObjectMigrationContext context;
        
        public ObjectAssert(ObjectMigrationContext context)
        {
            this.context = context;
        }

        public void WithDefinition(string definition)
        {
            AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, definition);
        }

        public void WithDefinitionFromString(string definition)
        {
            WithDefinition(definition);
        }

        public void WithDefinitionFromEmbeddedResource(string embeddedDefinition)
        {
            AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, context.GetEmbeddedResource(embeddedDefinition));
        }

        public void WithDefinitionFromFile(string filePath)
        {
            AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, File.ReadAllText(filePath));
        }

        private void AssertDefinitionsAreEqual(string objectName, ObjectType objectType, string definition)
        {
            var escapedDefinition = definition.EscapeApostraphes();
            var escapedObjectName = objectName.EscapeApostraphes().SurroundWithBrackets();

            var storedDefinitonSql = SqlHelpers.CreateRemoveNewLinesSql($"OBJECT_DEFINITION(OBJECT_ID(N'{escapedObjectName}', " +
                                                                        $"N'{objectType.ToSqlIdentifier()}'))");
            var embeddedDefinitionSql = SqlHelpers.CreateRemoveNewLinesSql($"'{escapedDefinition}'");

            context.Assert($"{storedDefinitonSql} != {embeddedDefinitionSql}",
                           $"The definition of object {objectName} did not match the definition in the embedded resource.");
        }
    }
}
