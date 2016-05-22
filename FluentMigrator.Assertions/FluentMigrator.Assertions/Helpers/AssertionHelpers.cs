namespace FluentMigrator.Assertions.Helpers
{
    public static class AssertionHelpers
    {
        public static string CreateRaiseErrorSql(string errorMessage)
        {
            return $"RAISERROR('{errorMessage}', 15, -1); RETURN";
        }
    }
}
