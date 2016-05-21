using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using System;
using System.Reflection;

namespace FluentMigrator.Helpers.Tests
{
    public static class Runner
    {
        public class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }

        public static void MigrateToLatest(string connectionString)
        {
            // var announcer = new NullAnnouncer();
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = "FluentMigrator.Helpers.Tests.Migrations"
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory =
                new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();

            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);
            }
        }
    }

    public class RunnerContainer : IDisposable
    {
        private IMigrationProcessor processor;
        public MigrationRunner Runner;

        public RunnerContainer(IMigrationProcessor processor, MigrationRunner runner)
        {
            this.processor = processor;
            Runner = runner;
        }

        public void Dispose()
        {
            processor.Dispose();
        }
    }
}
