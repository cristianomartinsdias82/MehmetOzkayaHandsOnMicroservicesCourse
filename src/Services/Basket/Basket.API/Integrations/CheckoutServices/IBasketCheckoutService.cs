using Basket.API.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Integrations.CheckoutServices
{
    public interface IBasketCheckoutService
    {
        ValueTask<Solution.SharedKernel.OperationResult> CheckoutAsync(
            CheckoutBasket basket,
            CancellationToken cancellationToken = default);
    }
}
