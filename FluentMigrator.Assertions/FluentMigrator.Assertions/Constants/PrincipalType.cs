using System;

namespace FluentMigrator.Assertions.Constants
{
    public enum PrincipalType
    {
        Database,
        Server
    }

    public static class PrincipalTypeHelpers
    {
        public static string GetUsersTable(this PrincipalType principalType)
        {
            switch (principalType)
            {
                case PrincipalType.Database:
                    return "sys.database_principals";
                case PrincipalType.Server:
                    return "sys.server_principals";
                default:
                    throw new ArgumentOutOfRangeException(nameof(principalType), principalType, null);
            }
        }

        public static string GetRolesTable(this PrincipalType principalType)
        {
            switch (principalType)
            {
                case PrincipalType.Database:
                    return "sys.database_role_members";
                case PrincipalType.Server:
                    return "sys.server_role_members";
                default:
                    throw new ArgumentOutOfRangeException(nameof(principalType), principalType, null);
            }
        }
    }
}
