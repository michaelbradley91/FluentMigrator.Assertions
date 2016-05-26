namespace FluentMigrator.Assertions.Helpers
{
    public static class StringHelpers
    {
        public static string SurroundWithBrackets(this string str)
        {
            if (str.StartsWith("[") && str.EndsWith("]")) return str;
            return "[" + str + "]";
        }

        public static string EscapeApostraphes(this string str)
        {
            return str.Replace("'", "''");
        }
    }
}
