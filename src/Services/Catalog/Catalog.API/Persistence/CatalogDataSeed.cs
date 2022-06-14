using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.API.Persistence
{
    public static class CatalogDataSeed
    {
        public static void Seed(ICatalogContext catalogContext)
        {
            if (!catalogContext.Products.Find(x => true).Any())
            {
                catalogContext.Products.InsertMany(new List<Product>
                {
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "IPhone X",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-1.png",
                        ListPrice = 950.00M,
                        Category = "Smart Phone"
                    },
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Samsung 10",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-2.png",
                        ListPrice = 840.00M,
                        Category = "Smart Phone"
                    },
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Huawei Plus",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-3.png",
                        ListPrice = 650.00M,
                        Category = "White Appliances"
                    },
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Xiaomi Mi 9",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-4.png",
                        ListPrice = 470.00M,
                        Category = "White Appliances"
                    },
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "HTC U11+ Plus",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-5.png",
                        ListPrice = 380.00M,
                        Category = "Smart Phone"
                    },
                    new Product()
                    {
                        Id = Guid.NewGuid(),
                        Title = "LG G7 ThinQ",
                        Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                        ImageUrl = "product-6.png",
                        ListPrice = 240.00M,
                        Category = "Home Kitchen"
                    }
                });
            }
        }
    }
}
