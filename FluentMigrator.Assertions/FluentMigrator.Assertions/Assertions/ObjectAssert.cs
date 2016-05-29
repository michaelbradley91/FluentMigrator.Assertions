using System.IO;
using FluentMigrator.Assertions.Contexts;

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
            context.AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, definition);
        }

        public void WithDefinitionFromString(string definition)
        {
            WithDefinition(definition);
        }

        public void WithDefinitionFromEmbeddedResource(string embeddedDefinition)
        {
            context.AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, context.GetEmbeddedResource(embeddedDefinition));
        }

        public void WithDefinitionFromFile(string filePath)
        {
            context.AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, File.ReadAllText(filePath));
        }
    }
}
