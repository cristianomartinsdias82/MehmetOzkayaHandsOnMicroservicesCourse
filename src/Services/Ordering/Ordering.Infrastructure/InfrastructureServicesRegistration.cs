using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrdersContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));
            });
            
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(Repository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddSingleton(configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>());
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }

    }
}
