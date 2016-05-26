using FluentMigrator.Assertions.Assertions;

namespace FluentMigrator.Assertions.Migrations
{
    public abstract class MigrationWithAssertions : Migration
    {
        protected readonly MigrationAssertor Assert;

        protected MigrationWithAssertions()
        {
            Assert = new MigrationAssertor(this);
        }
    }
}
