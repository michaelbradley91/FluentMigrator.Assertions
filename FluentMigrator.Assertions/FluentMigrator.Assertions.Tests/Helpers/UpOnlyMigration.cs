using System;
using FluentMigrator.Assertions.Assertions;

namespace FluentMigrator.Assertions.Tests.Helpers
{
    public class UpOnlyMigration : Migration
    {
        private readonly MigrationAssert assert;
        private readonly Action<Migration, MigrationAssert> up;

        public UpOnlyMigration(Action<Migration, MigrationAssert> up)
        {
            assert = new MigrationAssert(this);
            this.up = up;
        }

        public override void Up()
        {
            up(this, assert);
        }

        public override void Down()
        {
        }
    }
}
