namespace FluentMigrator.Assertions.Helpers
{
    public static class MigrationHelpers
    {
        public static string GetEmbeddedResource(this Migration migration, string resourceName)
        {
            return migration.GetType().Assembly.GetEmbeddedResource(resourceName);
        }
    }
}
