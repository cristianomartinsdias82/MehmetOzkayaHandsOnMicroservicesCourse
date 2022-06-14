using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;

namespace Catalog.API.Persistence
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(DatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            Products = client.GetDatabase(databaseSettings.DatabaseName)
                                       .GetCollection<Product>(databaseSettings.CollectionName);

            CatalogDataSeed.Seed(this); //TODO: Move this to Program.cs
        }

        public IMongoCollection<Product> Products { get; }
    }
}
