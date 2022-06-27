using Dapper;
using Discount.Grpc.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Threading;
using static Discount.Grpc.Settings.ConnectionStringHelper;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            var retryUntilAvailable = retry.Value;

            using var scope = host.Services.CreateScope();
            var databaseSettings = host.Services.GetRequiredService<DatabaseSettings>();
            var connectionString = BuildConnectionString(databaseSettings);
            var logger = host.Services.GetService<ILogger<TContext>>();

            logger?.LogInformation("Attempting to migrate PostgreSql database...");
            var transaction = default(NpgsqlTransaction);
            var conn = default(NpgsqlConnection);
            try
            {
                conn = new NpgsqlConnection(connectionString);
                conn.Open();

                transaction = conn.BeginTransaction();

                //conn.Execute(
                //    "CREATE DATABASE DiscountDb;",
                //    transaction: transaction);

                conn.Execute(
                    "DROP TABLE IF EXISTS Coupon;",
                    transaction: transaction);

                conn.Execute(
                    "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";",
                    transaction: transaction);

                conn.Execute(
                    "CREATE TABLE public.Coupon" +
                    "(" +
                        "Id uuid NULL DEFAULT uuid_generate_v4(), " +
                        "ProductName VARCHAR(50) NOT NULL, " +
                        "Description VARCHAR(256) NULL, " +
                        "Discount NUMERIC(1000, 2) NULL" +
                    ")",
                    transaction: transaction);

                conn.Execute(
                    "INSERT INTO public.Coupon (ProductName, Description, Discount) VALUES ('IPhone X', 'IPhone X Discount', 35);",
                    transaction: transaction);

                conn.Execute(
                    "INSERT INTO public.Coupon (ProductName, Description, Discount) VALUES ('Samsung 10', 'Samsung 10 Discount', 80);",
                    transaction: transaction);

                transaction.Commit();

                logger?.LogInformation("PostgreSql database migration successful!");
            }
            catch(NpgsqlException npgsqlExc)
            {
                transaction?.Rollback();

                logger?.LogError(npgsqlExc, "Error while attempting to migrate PostgreSql database! {Message}", npgsqlExc.Message);

                if (retryUntilAvailable++ < 50)
                {
                    Thread.Sleep(2500);
                    MigrateDatabase<TContext>(host, retryUntilAvailable);
                }
            }
            catch (Exception exc)
            {
                transaction?.Rollback();

                logger?.LogError(exc, "Error while attempting to migrate PostgreSql database! {Message}", exc.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return host;
        }
    }
}
