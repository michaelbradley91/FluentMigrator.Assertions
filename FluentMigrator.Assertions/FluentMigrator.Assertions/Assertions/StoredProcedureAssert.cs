using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IStoredProcedureAssert : IObjectAssert { }

    public class StoredProcedureAssert : ObjectAssert, IStoredProcedureAssert
    {
        public StoredProcedureAssert(StoredProcedureMigrationContext context) : base(context) { }
    }
}
