using Basket.API.Entities;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountsServiceProto;

namespace Basket.API.Integrations.DiscountServices
{
    public class DiscountServiceClient : IDiscountServiceClient
    {
        private readonly DiscountsServiceProtoClient _discountClient;
        private readonly ILogger<DiscountServiceClient> _logger;

        public DiscountServiceClient(
            ILogger<DiscountServiceClient> logger,
            DiscountsServiceProtoClient discountClient
        )
        {
            _discountClient = discountClient ?? throw new ArgumentNullException($"Argument {discountClient} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {logger} cannot be null.");
        }

        public async Task ApplyDiscountsAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await Task.WhenAll(
                cart.Items?.Select(
                    it => Task.Run(async () => it.ApplyDiscount((decimal)await GetDiscountForProductAsync(it.ProductName, cancellationToken))
            )));
        }

        private async Task<double> GetDiscountForProductAsync(string productName, CancellationToken cancellationToken = default)
        {
            var discount = 0D;

            try
            {
                LogInfo("Trying to get discount for product {ProductName}", productName);

                var getDiscountResult = await _discountClient.GetDiscountByProductAsync(
                    new() { ProductName = productName },
                    cancellationToken: cancellationToken);

                discount = getDiscountResult.Coupon?.Discount ?? 0D;
            }
            catch (RpcException rpcExc)
            {
                LogError(rpcExc, "Error when trying to get discount for product {ProductName}", productName);
            }
            catch (Exception exc)
            {
                LogError(exc, "Error when trying to get discount for product {ProductName}", productName);
            }

            return discount;
        }

        private void LogInfo(string info, params object[] args)
        {
            Task.Run(() =>
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation(info, args);
            });
        }

        private void LogError(Exception exception = default, string message = default, params object[] args)
        {
            Task.Run(() =>
            {
                if (_logger.IsEnabled(LogLevel.Error))
                    _logger.LogError(exception, message, args);
            });
        }
    }
}
