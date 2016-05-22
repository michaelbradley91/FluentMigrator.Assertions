using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace FluentMigrator.Helpers.Tests.Runner
{
    public static class Runner
    {
        public static RunnerContainer Create(string connectionString, string nameSpace, IAnnouncer announcer = null)
        {
            announcer = announcer ?? new NullAnnouncer();
//            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer) { Namespace = nameSpace };
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();

            var processor = factory.Create(connectionString, announcer, options))
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            return new RunnerContainer(processor, runner);
        }
    }
}
