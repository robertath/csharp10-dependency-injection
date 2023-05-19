using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Transformations;
using ProductImporter.Logic;

namespace ProductImporter.CompositionRoot
{
    public static class ProductImporterCompositionRoot
    {
        public static IServiceCollection AddProductImporter(this IServiceCollection services)
        {

            services.AddProductImporterLogic();
            services.AddProductTransformations(o => o.EnableCurrencyNormalizer = false);            

            return services;
        }

    }
}
