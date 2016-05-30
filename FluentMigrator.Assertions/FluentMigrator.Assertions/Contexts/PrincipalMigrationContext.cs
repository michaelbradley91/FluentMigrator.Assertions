using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class PrincipalMigrationContext : MigrationContext
    {
        public string PrincipalName { get; }
        public PrincipalType PrincipalType { get; }

        public PrincipalMigrationContext(MigrationContext existingContext, string principalName, PrincipalType principalType) : base(existingContext)
        {
            PrincipalName = principalName;
            PrincipalType = principalType;
        }
    }
}