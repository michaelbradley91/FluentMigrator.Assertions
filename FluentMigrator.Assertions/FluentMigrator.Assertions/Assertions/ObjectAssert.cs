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
        public ObjectMigrationContext Context { get; }
        
        public ObjectAssert(ObjectMigrationContext context)
        {
            Context = context;
        }

        public void WithDefinition(string definition)
        {
            AssertDefinitionsAreEqual(Context.ObjectName, Context.ObjectType, definition);
        }

        public void WithDefinitionFromString(string definition)
        {
            WithDefinition(definition);
        }

        public void WithDefinitionFromEmbeddedResource(string embeddedDefinition)
        {
            AssertDefinitionsAreEqual(Context.ObjectName, Context.ObjectType, Context.GetEmbeddedResource(embeddedDefinition));
        }

        public void WithDefinitionFromFile(string filePath)
        {
            AssertDefinitionsAreEqual(Context.ObjectName, Context.ObjectType, File.ReadAllText(filePath));
        }

        private void AssertDefinitionsAreEqual(string objectName, ObjectType objectType, string definition)
        {
            var escapedDefinition = definition.EscapeApostraphes();
            var escapedObjectName = objectName.EscapeApostraphes().SurroundWithBrackets();

            var storedDefinitonSql = SqlHelpers.CreateRemoveNewLinesSql($"OBJECT_DEFINITION(OBJECT_ID(N'{escapedObjectName}', " +
                                                                        $"N'{objectType.ToSqlIdentifier()}'))");
            var embeddedDefinitionSql = SqlHelpers.CreateRemoveNewLinesSql($"'{escapedDefinition}'");

            Context.Assert($"{storedDefinitonSql} != {embeddedDefinitionSql}",
                           $"The definition of object {objectName} did not match the definition in the embedded resource.");
        }
    }
}
