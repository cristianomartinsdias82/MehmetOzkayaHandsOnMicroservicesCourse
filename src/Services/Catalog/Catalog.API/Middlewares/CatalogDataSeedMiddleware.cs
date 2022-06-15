using Catalog.API.Persistence;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Catalog.API.Middlewares
{
    public class CatalogDataSeedMiddleware
    {
        private readonly RequestDelegate _next;

        public CatalogDataSeedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICatalogContext catalogContext)
        {
            //CatalogDataSeed.Seed(catalogContext);

            await _next.Invoke(context);
        }
    }
}