using EventBus.Messages.Common;
using EventBus.Messages.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.Integrations.BasketCheckout;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            services.AddScoped<BasketCheckoutConsumer>();
            services.AddMassTransit(busConfig =>
            {
                //Subscribe configuration
                busConfig.AddConsumer<BasketCheckoutConsumer>();

                busConfig.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(eventBusSettings.HostAddress);

                    //Subscribe configuration
                    cfg.ReceiveEndpoint(
                        EventBusConstants.BasketCheckoutQueue,
                        cfg2 =>
                        {
                            cfg2.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                        });
                });
            });

            return services;
        }
    }
}
