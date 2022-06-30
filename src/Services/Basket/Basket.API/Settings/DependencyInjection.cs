using Basket.API.Integrations.DiscountServices;
using Basket.API.Repositories;
//using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using static Discount.Grpc.Protos.DiscountsServiceProto;

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

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var discountApiIntegrationSettings = configuration.GetSection(nameof(DiscountApiIntegrationSettings)).Get<DiscountApiIntegrationSettings>();
            services.AddSingleton(discountApiIntegrationSettings);
            services.AddGrpcClient<DiscountsServiceProtoClient>(config => { config.Address = new Uri(discountApiIntegrationSettings.ServiceAddress); });
            services.AddScoped<IDiscountServiceClient, DiscountServiceClient>();

            return services;
        }
    }
}
