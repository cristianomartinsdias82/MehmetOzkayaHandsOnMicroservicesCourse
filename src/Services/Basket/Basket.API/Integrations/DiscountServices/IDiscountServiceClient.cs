using Basket.API.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Integrations.DiscountServices
{
    public interface IDiscountServiceClient
    {
        Task ApplyDiscountsAsync(ShoppingCart cart, CancellationToken cancellationToken = default);
    }
}