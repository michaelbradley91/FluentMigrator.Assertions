using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IObjectAssert
    {
        void WithDefinition(string embeddedDefinition);
    }

    public class ObjectAssert : IObjectAssert
    {
        private readonly ObjectMigrationContext context;
        
        public ObjectAssert(ObjectMigrationContext context)
        {
            this.context = context;
        }

        public void WithDefinition(string embeddedDefinition)
        {
            context.AssertDefinitionsAreEqual(context.ObjectName, context.ObjectType, embeddedDefinition);
        }
    }
}
