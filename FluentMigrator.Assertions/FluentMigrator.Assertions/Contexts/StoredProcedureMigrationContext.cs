using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class StoredProcedureMigrationContext : ObjectMigrationContext
    {
        public string StoredProcedureName { get; }

        public StoredProcedureMigrationContext(MigrationContext existingContext, string storedProcedureName) 
            : base(existingContext, storedProcedureName, ObjectType.StoredProcedure)
        {
            StoredProcedureName = storedProcedureName;
        }

        public StoredProcedureMigrationContext(StoredProcedureMigrationContext existingContext) :
            this(existingContext, existingContext.StoredProcedureName) { }
    }
}
