using System;

namespace FluentMigrator.Assertions.Constants
{
    public enum ObjectType
    {
        ScalarFunction,
        InlineTableValuedFunction,
        MultiStatementTableValuedFunction,
        StoredProcedure
    }

    public static class ObjectTypeHelpers
    {
        public static string ToSqlIdentifier(this ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.ScalarFunction:
                    return "FN";
                case ObjectType.InlineTableValuedFunction:
                    return "IF";
                case ObjectType.MultiStatementTableValuedFunction:
                    return "TF";
                case ObjectType.StoredProcedure:
                    return "P";
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
        }
    }
}
