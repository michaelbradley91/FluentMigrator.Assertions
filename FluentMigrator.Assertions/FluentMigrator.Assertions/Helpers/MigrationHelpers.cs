namespace FluentMigrator.Assertions.Helpers
{
    public static class MigrationHelpers
    {
        public static string GetEmbeddedResource(this Migration migration, string embeddedResource)
        {
            return migration.GetType().Assembly.GetEmbeddedResource(embeddedResource);
        }

        public static void Assert(this Migration migration, string condition, string errorMessage)
        {
            var errorSql = SqlHelpers.CreateRaiseErrorSql(errorMessage);
            migration.Execute.Sql($"IF {condition} BEGIN {errorSql} END;");
        }
    }
}
