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
