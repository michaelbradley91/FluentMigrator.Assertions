using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class DatabasePrincipalMigrationContext : PrincipalMigrationContext
    {
        public DatabasePrincipalMigrationContext(MigrationContext existingContext, string principalName)
            : base(existingContext, principalName, PrincipalType.Database) { }
    }
}