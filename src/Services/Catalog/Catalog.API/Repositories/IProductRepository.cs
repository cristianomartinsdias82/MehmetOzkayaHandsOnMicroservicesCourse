using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll(CancellationToken cancellation = default);
        Task<Product> GetById(Guid id, CancellationToken cancellation = default);
        Task<IEnumerable<Product>> GetByTitle(string title, CancellationToken cancellation = default);
        Task<IEnumerable<Product>> GetByCategory(string category, CancellationToken cancellation = default);
        Task<bool> Create(Product product, CancellationToken cancellation = default);
        Task<bool> Update(Product product, CancellationToken cancellation = default);
        Task<bool> Delete(Guid id, CancellationToken cancellation = default);
    }
}
