using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(
            this IHost host,
            Action<TContext, IServiceProvider> seedAction,
            int? retry = 0)
            where TContext : DbContext
        {
            var retryUntilAvailable = retry.Value;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<TContext>();
            var logger = services.GetService<ILogger<TContext>>();

            logger?.LogInformation("Attempting to migrate database...");
            
            try
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();

                seedAction?.Invoke(dbContext, services);

                logger?.LogInformation("Database migration successful!");
            }
            catch (SqlException sqlExc)
            {
                logger?.LogError(sqlExc, "Error while attempting to migrate database! {Message}", sqlExc.Message);

                if (retryUntilAvailable++ < 50)
                {
                    Thread.Sleep(2500);
                    MigrateDatabase(host, seedAction, retryUntilAvailable);
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Error while attempting to migrate database! {Message}", exc.Message);
            }

            return host;
        }
    }
}
