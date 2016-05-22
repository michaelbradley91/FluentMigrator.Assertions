using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace FluentMigrator.Helpers.Tests.Setup
{
    public class RunnerContainer : IDisposable
    {
        private readonly IMigrationProcessor processor;
        private readonly Stack<IMigration> migrations;

        public readonly MigrationRunner Runner;

        public RunnerContainer(IMigrationProcessor processor, MigrationRunner runner)
        {
            this.processor = processor;
            this.Runner = runner;
            migrations = new Stack<IMigration>();
        }

        public void MigrateUp(IMigration migration)
        {
            Runner.Up(migration);
        }

        public void MigrateDownOne()
        {
            Runner.Down(migrations.Pop());
        }

        public void MigrateDownAll()
        {
            while (migrations.Any())
            {
                Runner.Down(migrations.Pop());
            }
        }

        public void Dispose()
        {
            processor.Dispose();
        }

        public static RunnerContainer Create(IAnnouncer announcer = null)
        {
            announcer = announcer ?? new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer);
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();

            var processor = factory.Create(Constants.FluentMigratorDbConnectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            return new RunnerContainer(processor, runner);
        }
    }
}