using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Contexts;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IPrincipalAssert
    {
        void WithRole(string role);
    }

    public abstract class PrincipalAssert : IPrincipalAssert
    {
        public PrincipalMigrationContext Context { get; }

        public PrincipalAssert(PrincipalMigrationContext context)
        {
            Context = context;
        }

        public void WithRole(string role)
        {
            var escapedName = Context.PrincipalName.EscapeApostraphes();
            var escapedRole = role.EscapeApostraphes();

            Context.Assert($"(SELECT COUNT(*) FROM {Context.PrincipalType.GetRolesTable()} AS m " +
                           $"INNER JOIN {Context.PrincipalType.GetUsersTable()} AS r ON m.role_principal_id = r.principal_id " +
                           $"INNER JOIN {Context.PrincipalType.GetUsersTable()} AS u ON u.principal_id = m.member_principal_id " +
                           $"WHERE r.name = '{escapedRole}' AND u.name = '{escapedName}') = 0",
                           $"Principal {Context.PrincipalName} did not have role {role}");
        }
    }
}