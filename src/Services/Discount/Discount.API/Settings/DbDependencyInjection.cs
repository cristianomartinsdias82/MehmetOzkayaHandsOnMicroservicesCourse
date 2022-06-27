using Discount.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.API.Settings
{
    public static class DbDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>());
            services.AddScoped<IDiscountRepository, DiscountRepository>();

            return services;
        }
    }
}
