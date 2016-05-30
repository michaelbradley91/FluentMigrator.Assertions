using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IDatabasePrincipalAssert : IPrincipalAssert { }

    public class DatabasePrincipalAssert : PrincipalAssert, IDatabasePrincipalAssert
    {
        public DatabasePrincipalAssert(DatabasePrincipalMigrationContext context) : base(context) { }
    }
}