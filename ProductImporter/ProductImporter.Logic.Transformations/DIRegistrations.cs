using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductImporter.Logic.Transformations.Util;

namespace ProductImporter.Logic.Transformations;

public static class DIRegistrations
{
    public static object ProductSourceOptions { get; private set; }

    public static IServiceCollection AddProductTransformations(
        this IServiceCollection services,
        Action<ProductTransformationOptions> optionsModifier)
    {
        var options = new ProductTransformationOptions();
        optionsModifier(options);

        services.AddScoped<IProductTransformationContext, ProductTransformationContext>();
        services.AddScoped<IProductTransformation, NameDecapitaliser>();
        services.AddTransient<IProductTransformation, DisposingProductTransformation>();

        if (options.EnableCurrencyNormalizer)
        {
            services.AddScoped<IProductTransformation, CurrencyNormalizer>();
        }
        else
        {
            services.AddScoped<IProductTransformation, NullCurrencyNormalizer>();
        }

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IProductTransformation, ReferenceAdder>();
        services.AddScoped<IReferenceGeneratorFactory, ReferenceGeneratorFactory>();
        services.AddSingleton<IIncrementingCounter, IncrementingCounter>();



        return services;
    }
}
