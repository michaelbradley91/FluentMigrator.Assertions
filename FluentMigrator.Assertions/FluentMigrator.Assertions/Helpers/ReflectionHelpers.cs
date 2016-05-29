using System;

namespace FluentMigrator.Assertions.Helpers
{
    public static class ReflectionHelpers
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString())[0];
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }
    }
}
