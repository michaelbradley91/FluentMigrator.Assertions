using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Contexts
{
    public class MigrationContext
    {
        public Migration Migration { get; }

        public MigrationContext(Migration migration)
        {
            Migration = migration;
        }

        protected MigrationContext(MigrationContext existingContext) : this(existingContext.Migration) { }
        
        public string GetEmbeddedResource(string resourceName)
        {
            return Migration.GetEmbeddedResource(resourceName);
        }

        public string GetAssertSql(string failureCondition, string errorMessage)
        {
            var errorSql = SqlHelpers.CreateRaiseErrorSql(errorMessage);
            return $"IF ({failureCondition}) BEGIN {errorSql} END;";
        }

        public void Assert(string failureCondition, string errorMessage)
        {
            var assertSql = GetAssertSql(failureCondition, errorMessage);
            Execute(assertSql);
        }

        public void Execute(string sql)
        {
            Migration.Execute.Sql(sql);
        }
    }
}
