using System;

namespace FluentMigrator.Assertions.Constants
{
    public enum FunctionType
    {
        Scalar,
        InlineTableValued,
        MultiStatementTableValued
    }

    public static class FunctionTypeHelpers
    {
        public static ObjectType ToObjectType(this FunctionType functionType)
        {
            switch (functionType)
            {
                case FunctionType.Scalar:
                    return ObjectType.ScalarFunction;
                case FunctionType.InlineTableValued:
                    return ObjectType.InlineTableValuedFunction;
                case FunctionType.MultiStatementTableValued:
                    return ObjectType.MultiStatementTableValuedFunction;
                default:
                    throw new ArgumentOutOfRangeException(nameof(functionType), functionType, null);
            }
        }
    }
}
