using FluentMigrator.Assertions.Constants;
using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IMigrationAssert
    {
        IStoredProcedureAssert StoredProcedureExists(string storedProcedureName);
    }

    public class MigrationAssert : IMigrationAssert
    {
        private readonly MigrationContext context;

        public MigrationAssert(Migration migration) : this(new MigrationContext(migration)) { }

        public MigrationAssert(MigrationContext context)
        {
            this.context = context;
        }

        public IStoredProcedureAssert StoredProcedureExists(string storedProcedureName)
        {
            context.AssertObjectExists(storedProcedureName, ObjectType.StoredProcedure);

            var newContext = new StoredProcedureMigrationContext(context, storedProcedureName);
            return new StoredProcedureAssert(newContext);
        }

        public IFunctionAssert FunctionExists(string functionName, FunctionType functionType)
        {
            context.AssertObjectExists(functionName, functionType.ToObjectType());

            var newContext = new FunctionMigrationContext(context, functionName, functionType);
            return new FunctionAssert(newContext);
        }

        public IObjectAssert ObjectExists(string objectName, ObjectType objectType)
        {
            context.AssertObjectExists(objectName, objectType);

            var newContext = new ObjectMigrationContext(context, objectName, objectType);
            return new ObjectAssert(newContext);
        }
    }
}
