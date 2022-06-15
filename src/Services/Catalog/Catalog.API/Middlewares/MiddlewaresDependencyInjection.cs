using Microsoft.AspNetCore.Builder;

namespace Catalog.API.Middlewares
{
    public static class MiddlewaresDependencyInjection
    {
        public static IApplicationBuilder UseCatalogDataSeeding(this IApplicationBuilder appBuilder)
            => appBuilder.UseMiddleware<CatalogDataSeedMiddleware>();
    }
}
