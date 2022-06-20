using Basket.API.Entities;
using Basket.API.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cachingService;
        private readonly CachingSettings _cachingSettings;

        public BasketRepository(
            IDistributedCache cachingService,
            CachingSettings cachingSettings)
        {
            _cachingService = cachingService ?? throw new ArgumentNullException($"Argument {nameof(cachingService)} cannot be null.");
            _cachingSettings = cachingSettings ?? throw new ArgumentNullException($"Argument {nameof(cachingSettings)} cannot be null.");
        }

        public async Task<ShoppingCart> GetBasket(
            string userName,
            CancellationToken cancellationToken = default)
        {
            var basketRawData = await _cachingService.GetStringAsync(
                                                        userName,
                                                        cancellationToken);

            if (!string.IsNullOrWhiteSpace(basketRawData))
            {
                try
                {
                    return JsonConvert.DeserializeObject<ShoppingCart>(basketRawData);
                }
                catch (Exception)
                {
                }
            }

            return new(userName);
        }

        public async Task<ShoppingCart> RemoveBasket(
            string userName,
            CancellationToken cancellationToken = default)
        {
            await _cachingService.RemoveAsync(
                userName,
                cancellationToken);

            return new(userName);
        }

        public async Task<ShoppingCart> SaveBasket(
            ShoppingCart shoppingCart,
            CancellationToken cancellationToken = default)
        {
            if (shoppingCart is null)
                return null;

                await _cachingService.SetStringAsync(
                    shoppingCart.UserName,
                    JsonConvert.SerializeObject(shoppingCart),
                    new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(_cachingSettings.SlidingExpirationTTLInMinutes) },
                    cancellationToken);

            return shoppingCart;
        }
    }
}
