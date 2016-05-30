using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IServerPrincipalAssert : IPrincipalAssert { }

    public class ServerPrincipalAssert : PrincipalAssert, IServerPrincipalAssert
    {
        public ServerPrincipalAssert(ServerPrincipalMigrationContext context) : base(context) { }
    }
}