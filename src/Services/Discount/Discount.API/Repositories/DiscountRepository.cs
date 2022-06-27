using Dapper;
using Discount.API.Entities;
using Discount.API.Settings;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Discount.API.Settings.ConnectionStringHelper;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public DiscountRepository(DatabaseSettings databaseSettings)
            => _databaseSettings = databaseSettings;

        public async Task<Coupon> AddDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
        {
            coupon.Id = Guid.NewGuid();
            var commandText = $"INSERT INTO {_databaseSettings.SchemaOwner}.{_databaseSettings.TableName} " +
                              "(Id, ProductName, Description, Discount) " +
                              "VALUES (@Id, @ProductName, @Description, @Discount);";

            using var conn = new NpgsqlConnection(BuildConnectionString(_databaseSettings));
            var command = new CommandDefinition(
                commandText,
                parameters: new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Discount = coupon.Discount
                },
                cancellationToken: cancellationToken);

            var rowsAffected = await conn.ExecuteAsync(command);

            return coupon;
        }

        public async Task DeleteDiscountAsync(string productName, CancellationToken cancellationToken)
        {
            var commandText = $"DELETE FROM {_databaseSettings.SchemaOwner}.{_databaseSettings.TableName} " +
                              "WHERE ProductName = @ProductName;";

            using var conn = new NpgsqlConnection(BuildConnectionString(_databaseSettings));
            var command = new CommandDefinition(
                commandText,
                parameters: new
                {
                    ProductName = productName
                },
                cancellationToken: cancellationToken);

            await conn.ExecuteAsync(command);
        }

        public async Task<Coupon> GetDiscountByProductNameAsync(string productName, CancellationToken cancellationToken)
        {
            var commandText = "SELECT Id, ProductName, Description, Discount " +
                              $"FROM {_databaseSettings.SchemaOwner}.{_databaseSettings.TableName} " +
                              "WHERE UPPER(ProductName) LIKE '%' || UPPER(COALESCE(@ProductName,'')) || '%';";

            using var conn = new NpgsqlConnection(BuildConnectionString(_databaseSettings));
            var queryCommand = new CommandDefinition(
                commandText,
                parameters: new
                {
                    ProductName = productName,
                },
                cancellationToken: cancellationToken);
            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>(queryCommand);

            return coupon ?? Coupon.None;
        }

        public async Task<IEnumerable<Coupon>> GetDiscountsAsync(CancellationToken cancellationToken)
        {
            var commandText = "SELECT Id, ProductName, Description, Discount " +
                              $"FROM {_databaseSettings.SchemaOwner}.{_databaseSettings.TableName};";

            using var conn = new NpgsqlConnection(BuildConnectionString(_databaseSettings));
            var queryCommand = new CommandDefinition(
                commandText,
                cancellationToken: cancellationToken);

            var coupons = await conn.QueryAsync<Coupon>(queryCommand);

            return coupons ?? Enumerable.Empty<Coupon>();
        }

        public async Task<Coupon> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
        {
            var commandText = $"UPDATE {_databaseSettings.SchemaOwner}.{_databaseSettings.TableName} " +
                              "SET ProductName = @ProductName, Description = @Description, Discount = @Discount " +
                              "WHERE Id = @Id;";

            using var conn = new NpgsqlConnection(BuildConnectionString(_databaseSettings));
            var command = new CommandDefinition(
                commandText,
                parameters: new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Discount = coupon.Discount
                },
                cancellationToken: cancellationToken);

            await conn.ExecuteAsync(command);

            return coupon;
        }
    }
}
