using Discount.Grpc.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Coupon>> GetDiscountsAsync(CancellationToken cancellationToken);
        Task<Coupon> GetDiscountByProductNameAsync(string productName, CancellationToken cancellationToken);
        Task<Coupon> AddDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
        Task<Coupon> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
        Task DeleteDiscountAsync(string productName, CancellationToken cancellationToken);
    }
}
