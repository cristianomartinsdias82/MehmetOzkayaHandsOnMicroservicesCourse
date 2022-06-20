using Basket.API.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default);

        Task<ShoppingCart> SaveBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);

        Task<ShoppingCart> RemoveBasket(string userName, CancellationToken cancellationToken = default);
    }
}
