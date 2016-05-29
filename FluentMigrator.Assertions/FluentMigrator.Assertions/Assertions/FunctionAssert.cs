using FluentMigrator.Assertions.Contexts;

namespace FluentMigrator.Assertions.Assertions
{
    public interface IFunctionAssert : IObjectAssert { }

    public class FunctionAssert : ObjectAssert, IFunctionAssert
    {
        public FunctionAssert(FunctionMigrationContext context) : base(context) { }
    }
}