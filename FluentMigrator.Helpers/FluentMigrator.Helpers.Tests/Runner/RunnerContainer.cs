using System;
using FluentMigrator.Runner;

namespace FluentMigrator.Helpers.Tests.Runner
{
    public class RunnerContainer : IDisposable
    {
        private readonly IMigrationProcessor processor;
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