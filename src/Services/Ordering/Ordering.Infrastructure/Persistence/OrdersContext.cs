using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "cristiano.dias";
                        entry.Entity.CreatedDate = DateTime.UtcNow;

                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "cristiano.dias";
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;

                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
