using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class ObjectMigrationContext : MigrationContext
    {
        public string ObjectName { get; }
        public ObjectType ObjectType { get; }

        public ObjectMigrationContext(MigrationContext existingContext, string objectName, ObjectType objectType) : base(existingContext)
        {
            ObjectName = objectName;
            ObjectType = objectType;
        }

        public ObjectMigrationContext(ObjectMigrationContext existingContext) 
            : this(existingContext, existingContext.ObjectName, existingContext.ObjectType) { }
    }
}