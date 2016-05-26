using System;
using FluentMigrator.Assertions.Assertions;

namespace FluentMigrator.Assertions.Tests.Helpers
{
    public class UpOnlyMigration : Migration
    {
        private readonly MigrationAssertor assertor;
        private readonly Action<Migration, MigrationAssertor> up;

        public UpOnlyMigration(Action<Migration, MigrationAssertor> up)
        {
            assertor = new MigrationAssertor(this);
            this.up = up;
        }

        public override void Up()
        {
            up(this, assertor);
        }

        public override void Down()
        {
        }
    }
}
