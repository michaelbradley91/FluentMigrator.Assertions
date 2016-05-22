using System.Configuration;
using System.IO;
using System.Reflection;

namespace FluentMigrator.Assertions.Helpers
{
    public static class AssemblyHelpers
    {
        private static string DefaultEmbeddedResourceNamespace => ConfigurationManager.AppSettings["FluentMigrator.Assertions.DefaultEmbeddedResourceNamespace"];

        public static string GetEmbeddedResource(this Assembly assembly, string resource)
        {
            Stream stream = null;
            try
            {
                stream = assembly.GetManifestResourceStream(resource);
                if (stream == null && !string.IsNullOrEmpty(DefaultEmbeddedResourceNamespace))
                {
                    stream = assembly.GetManifestResourceStream($"{DefaultEmbeddedResourceNamespace}.{resource}");
                }
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}
