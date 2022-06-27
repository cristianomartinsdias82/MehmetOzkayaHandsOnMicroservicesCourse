using Basket.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Settings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var cachingSettings = configuration.GetSection(nameof(CachingSettings)).Get<CachingSettings>();
            services.AddSingleton(cachingSettings);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cachingSettings.ServerAddress;

                //IT AUTOMATICALLY REGISTERS THE IDistributedCache DEPENDENCY FOR US!
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }
    }
}
