using System.Data;
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
            string actualDefinitionVariableName;
            string expectedDefinitionVariableName;

            var escapedDefinition = definition.EscapeApostraphes();
            var escapedObjectName = objectName.EscapeApostraphes().SurroundWithBrackets();

            var getActualDefinition = SqlHelpers.RemoveCommentsAndWhiteSpace(out actualDefinitionVariableName, 
                $"OBJECT_DEFINITION(OBJECT_ID(N'{escapedObjectName}', N'{objectType.ToSqlIdentifier()}'))");

            var getExpectedDefinition = SqlHelpers.RemoveCommentsAndWhiteSpace(out expectedDefinitionVariableName, $"'{escapedDefinition}'");
            
            var assert = Context.GetAssertSql($"@{actualDefinitionVariableName} != @{expectedDefinitionVariableName}",
                $"The definition of object {objectName} did not match the definition in the embedded resource.");

            Context.Execute(string.Join("\r\n", getActualDefinition, getExpectedDefinition, assert));
        }
    }
}
