using FluentMigrator.Assertions.Constants;

namespace FluentMigrator.Assertions.Contexts
{
    public class FunctionMigrationContext : ObjectMigrationContext
    {
        public string FunctionName { get; }
        public FunctionType FunctionType { get; }

        public FunctionMigrationContext(MigrationContext existingContext, string functionName, FunctionType functionType) 
            : base(existingContext, functionName, functionType.ToObjectType())
        {
            FunctionName = functionName;
            FunctionType = functionType;
        }

        public FunctionMigrationContext(FunctionMigrationContext existingContext) :
            this(existingContext, existingContext.FunctionName, existingContext.FunctionType) { }
    }
}