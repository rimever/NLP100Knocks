using System;
using Chapter07.Core;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Chapter07.Migrator
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 引用　https://fluentmigrator.github.io/articles/migration-runners.html?tabs=vs-pkg-manager-console
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = new ConnectionString();
            var serviceProvider = CreateServices(connectionString.Value);

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString).ScanIn(typeof(CreateTableMigration).Assembly).For
                    .Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}