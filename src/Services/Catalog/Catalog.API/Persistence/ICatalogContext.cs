using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Persistence
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
