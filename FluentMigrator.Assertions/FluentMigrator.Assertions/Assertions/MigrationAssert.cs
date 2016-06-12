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
            var escapedName = name.EscapeApostraphes();
            Context.Assert($"(SELECT COUNT(*) FROM {principalType.GetUsersTable()} WHERE name = '{escapedName}') = 0",
                           $"User {name} does not exist.");
        }
    }
}
