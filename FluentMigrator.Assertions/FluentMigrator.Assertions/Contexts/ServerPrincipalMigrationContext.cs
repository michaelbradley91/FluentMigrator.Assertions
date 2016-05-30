using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class ServerPrincipalMigrationContext : PrincipalMigrationContext
    {
        public ServerPrincipalMigrationContext(MigrationContext existingContext, string principalName)
            : base(existingContext, principalName, PrincipalType.Server) { }
    }
}