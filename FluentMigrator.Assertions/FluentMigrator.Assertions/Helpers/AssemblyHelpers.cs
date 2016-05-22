using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FluentMigrator.Assertions.Helpers
{
    public static class AssemblyHelpers
    {
        public static string GetEmbeddedResource(this Assembly assembly, string resource)
        {
            using (var stream = assembly.GetManifestResourceStream(resource))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
