namespace FluentMigrator.Assertions.Helpers
{
    public static class SqlHelpers
    {
        public static string CreateRaiseErrorSql(string errorMessage)
        {
            return $"RAISERROR('{errorMessage.EscapeApostraphes()}', 15, -1); RETURN";
        }

        public static string CreateRemoveNewLinesSql(string sqlString)
        {
            return $"REPLACE(REPLACE({sqlString}, CHAR(10), ''), CHAR(13), '')";
        }
    }
}
