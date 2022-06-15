using Catalog.API.Entities;
using Catalog.API.Persistence;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellation = default)
        {
            return await _catalogContext.Products.Find(x => true)
                                                 .ToListAsync(cancellation);
        }

        public async Task<IEnumerable<Product>> GetByCategory(string category, CancellationToken cancellation = default)
        {
            var filter = Builders<Product>.Filter.Regex(x => x.Category, new BsonRegularExpression(category, "i"));

            return await _catalogContext.Products.Find(filter)
                                                 .ToListAsync(cancellation);
        }

        public async Task<Product> GetById(Guid id, CancellationToken cancellation = default)
        {
            return await _catalogContext.Products.Find(x => x.Id == id)
                                                 .FirstOrDefaultAsync(cancellationToken: cancellation);
        }

        public async Task<IEnumerable<Product>> GetByTitle(string title, CancellationToken cancellation = default)
        {
            var filter = Builders<Product>.Filter.Regex(x => x.Title, new BsonRegularExpression(title, "i"));

            return await _catalogContext.Products.Find(filter)
                                                 .ToListAsync(cancellation);
        }

        public async Task<bool> Update(Product product, CancellationToken cancellation = default)
        {
            var updateResult = await _catalogContext.Products
                                                    .ReplaceOneAsync(f => f.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Create(Product product, CancellationToken cancellation = default)
        {
            await _catalogContext.Products.InsertOneAsync(product, cancellationToken: cancellation);

            return true;
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellation = default)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            var result = await _catalogContext.Products.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
