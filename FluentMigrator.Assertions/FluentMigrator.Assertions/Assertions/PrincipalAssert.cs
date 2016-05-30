using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Contexts;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IPrincipalAssert
    {
        void WithRole(string role);
    }

    public class PrincipalAssert : IPrincipalAssert
    {
        private readonly PrincipalMigrationContext context;

        public PrincipalAssert(PrincipalMigrationContext context)
        {
            this.context = context;
        }

        public void WithRole(string role)
        {
            var escapedName = context.PrincipalName.EscapeApostraphes();
            var escapedRole = role.EscapeApostraphes();

            context.Assert($"(SELECT COUNT(*) FROM {context.PrincipalType.GetRolesTable()} AS m " +
                           $"INNER JOIN {context.PrincipalType.GetUsersTable()} AS r ON m.role_principal_id = r.principal_id " +
                           $"INNER JOIN {context.PrincipalType.GetUsersTable()} AS u ON u.principal_id = m.member_principal_id " +
                           $"WHERE r.name = '{escapedRole}' AND u.name = '{escapedName}') > 0",
                           $"Principal {context.PrincipalName} did not have role {role}");
        }
    }
}