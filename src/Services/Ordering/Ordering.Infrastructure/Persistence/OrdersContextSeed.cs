using Microsoft.Extensions.Logging;
using Ordering.Application.Helpers.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public static class OrdersContextSeed
    {
        public static async Task SeedAsync<TContext>(
            OrdersContext context,
            ILogger<TContext> logger,
            CancellationToken cancellationToken = default)
        {
            if (!context.Orders.Any())
            {
                UnobtrusiveLoggingHelper.Log(
                    logger,
                    "Seeding Ordering database...");

                var ordersFaker = new Bogus.Faker<Order>();
                ordersFaker.StrictMode(true);
                ordersFaker.RuleFor(o => o.CreatedBy, f => "cristiano.dias");
                ordersFaker.RuleFor(o => o.CreatedDate, f => DateTime.Now);
                ordersFaker.RuleFor(o => o.LastModifiedBy, f => "cristiano.dias");
                ordersFaker.RuleFor(o => o.LastModifiedDate, f => DateTime.Now);
                ordersFaker.RuleFor(o => o.AddressLine, f => f.Address.StreetAddress());
                ordersFaker.RuleFor(o => o.Expires, f => $"{f.Date.Future():MM/yyyy}");
                ordersFaker.RuleFor(o => o.CreatedDate, f => f.Date.Future());
                ordersFaker.RuleFor(o => o.Cvv, f => f.Finance.CreditCardCvv());
                ordersFaker.RuleFor(o => o.FirstName, f => f.Name.FirstName());
                ordersFaker.RuleFor(o => o.LastName, f => f.Name.LastName());
                ordersFaker.RuleFor(o => o.CardName, (f, u) => $"{u.FirstName} {u.LastName}");
                ordersFaker.RuleFor(o => o.CardNumber, f => f.Finance.CreditCardNumber());
                ordersFaker.RuleFor(o => o.Country, f => f.Address.CountryCode());
                ordersFaker.RuleFor(o => o.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));
                ordersFaker.RuleFor(o => o.Id, f => f.Random.Uuid());
                ordersFaker.RuleFor(o => o.OrderTotal, f => f.Finance.Amount(650, 50000));
                ordersFaker.RuleFor(o => o.PaymentMethod, f => (int)f.PickRandom<PaymentMethods>());
                ordersFaker.RuleFor(o => o.State, f => f.Address.State());
                ordersFaker.RuleFor(o => o.UserName, (f,u) => f.Internet.UserName(u.FirstName, u.LastName));
                ordersFaker.RuleFor(o => o.ZipCode, f => f.Address.ZipCode("#####-###"));

                var orders = ordersFaker.Generate(30);

                await context.Orders.AddRangeAsync(orders, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                UnobtrusiveLoggingHelper.Log(
                    logger,
                    "Ordering database seed operation completed succesfully.");
            }
        }
    }
}
