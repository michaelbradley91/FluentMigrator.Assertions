using FluentMigrator.Assertions.Assertions;

namespace FluentMigrator.Assertions.Migrations
{
    public abstract class MigrationWithAssertions : Migration
    {
        protected readonly MigrationAssert Assert;

        protected MigrationWithAssertions()
        {
            Assert = new MigrationAssert(this);
        }
    }
}
