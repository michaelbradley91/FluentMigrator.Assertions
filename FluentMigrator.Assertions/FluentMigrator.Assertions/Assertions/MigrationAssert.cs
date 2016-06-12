using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Contexts;
using FluentMigrator.Assertions.Helpers;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IMigrationAssert
    {
        IStoredProcedureAssert StoredProcedureExists(string storedProcedureName);
    }

    public class MigrationAssert : IMigrationAssert
    {
        public MigrationContext Context { get; }

        public MigrationAssert(Migration migration) : this(new MigrationContext(migration)) { }

        public MigrationAssert(MigrationContext context)
        {
            Context = context;
        }

        public void IsTrue(string condition)
        {
            Context.Assert(condition, "The condition was not met.");
        }

        public IStoredProcedureAssert StoredProcedureExists(string storedProcedureName)
        {
            AssertObjectExists(storedProcedureName, ObjectType.StoredProcedure);

            var newContext = new StoredProcedureMigrationContext(Context, storedProcedureName);
            return new StoredProcedureAssert(newContext);
        }

        public IFunctionAssert FunctionExists(string functionName, FunctionType functionType)
        {
            AssertObjectExists(functionName, functionType.ToObjectType());

            var newContext = new FunctionMigrationContext(Context, functionName, functionType);
            return new FunctionAssert(newContext);
        }

        public IObjectAssert ObjectExists(string objectName, ObjectType objectType)
        {
            AssertObjectExists(objectName, objectType);

            var newContext = new ObjectMigrationContext(Context, objectName, objectType);
            return new ObjectAssert(newContext);
        }

        private void AssertObjectExists(string objectName, ObjectType objectType)
        {
            Context.Assert($"OBJECT_ID(N'{objectName.EscapeApostraphes().SurroundWithBrackets()}', " +
                           $"N'{objectType.ToSqlIdentifier()}') IS NULL",
                           $"Object {objectName} does not exist");
        }

        public IDatabasePrincipalAssert DatabasePrincipalExists(string name)
        {
            AssertPrincipalExists(name, PrincipalType.Database);

            var newContext = new DatabasePrincipalMigrationContext(Context, name);
            return new DatabasePrincipalAssert(newContext);
        }

        public IServerPrincipalAssert ServerPrincipalExists(string name)
        {
            AssertPrincipalExists(name, PrincipalType.Server);

            var newContext = new ServerPrincipalMigrationContext(Context, name);
            return new ServerPrincipalAssert(newContext);
        }

        private void AssertPrincipalExists(string name, PrincipalType principalType)
        {
            var escapedName = name.EscapeApostraphes().SurroundWithBrackets();
            Context.Assert($"(SELECT COUNT(*) FROM {principalType.GetUsersTable()} WHERE name = '{escapedName}') = 0",
                           $"User {name} does not exist.");
        }

        public ITableAssert TableExists(string name)
        {
            AssertTableExists(name);

            var newContext = new TableMigrationContext(Context, name);
            return new TableAssert(newContext);
        }

        private void AssertTableExists(string name)
        {
            var escapedName = name.EscapeApostraphes().SurroundWithBrackets();
            Context.Assert($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = SCHEMA_NAME() AND TABLE_NAME = '{escapedName}') = 0",
                           $"Table {name} does not exist.");
        }
    }

    public interface ITableAssert
    {
    }

    public class TableAssert : ITableAssert
    {
        public TableMigrationContext Context { get; }

        public TableAssert(TableMigrationContext context)
        {
            Context = context;
        }

        public IColumnAssert WithColumn(string name)
        {
            AssertColumnExists(name);

            var newContext = new ColumnMigrationContext(Context, name);
            return new ColumnAssert(newContext);
        }

        private void AssertColumnExists(string name)
        {
            var escapedColumnName = name.EscapeApostraphes().SurroundWithBrackets();
            Context.Assert($"(SELECT COUNT(*) FROM sys.columns WHERE Name = N'{escapedColumnName}' AND Object_ID = Object_ID(N'{Context.EscapedTableName}'))",
                           $"Column {name} does not exist.");
        }
    }

    public interface IColumnAssert
    {
    }

    public class ColumnAssert : IColumnAssert
    {
        public ColumnMigrationContext Context { get; }

        public ColumnAssert(ColumnMigrationContext context)
        {
            Context = context;
        }
    }

    public class ColumnMigrationContext : TableMigrationContext
    {
        public string ColumnName { get; }
        public string EscapedColumnName => ColumnName.EscapeApostraphes().SurroundWithBrackets();

        public ColumnMigrationContext(TableMigrationContext context, string name) : base(context)
        {
            ColumnName = name;
        }
    }

    public class TableMigrationContext : MigrationContext
    {
        public string TableName { get; }
        public string EscapedTableName => TableName.EscapeApostraphes().SurroundWithBrackets();

        public TableMigrationContext(TableMigrationContext context) : this(context, context.TableName) { }

        public TableMigrationContext(MigrationContext context, string tableName) : base(context)
        {
            TableName = tableName;
        }
    }
}

