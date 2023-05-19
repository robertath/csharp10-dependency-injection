using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Shared;
using ProductImporter.Logic.Source;
using ProductImporter.Logic.Target;
using ProductImporter.Logic.Transformation;

namespace ProductImporter.Logic
{
    public static class DIRegistrations
    {
        public static IServiceCollection AddProductImporterLogic(this IServiceCollection services)
        {
            services.AddTransient<IPriceParser, PriceParser>();

            //services.AddTransient<IProductSource, CsvProductSource>();
            services.AddHttpClient<IProductSource, HttpProductSource>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://raw.githubusercontent.com/henrybeen/");
                });

            services.AddTransient<IProductFormatter, ProductFormatter>();

            //Add feature according the flag on appsettings
            services.AddTransient<OldCsvProductTarget>();
            services.AddTransient<CsvProductTarget>();
            services.AddTransient<IProductTarget>((serviceProvider) =>
            {
                var configuration =serviceProvider.GetRequiredService<IConfiguration>();
                if(configuration.GetValue<int>("EnableCsvWriter") == 1) 
                {
                    return serviceProvider.GetRequiredService<CsvProductTarget>();
                }
                return serviceProvider.GetRequiredService<OldCsvProductTarget>();
            });

            services.AddTransient<Logic.ProductImporter>();

            services.AddSingleton<ImportStatistics>();
            services.AddSingleton<IGetImportStatistics, ImportStatistics>((serviceProvider) =>
            {
                return serviceProvider.GetRequiredService<ImportStatistics>();
            });
            services.AddSingleton<IWriteImportStatistics, ImportStatistics>((serviceProvider) =>
            {
                return serviceProvider.GetRequiredService<ImportStatistics>();
            });

            services.AddTransient<Lazy<IProductTransformer>>((serviceProvider) =>
            {
                return new Lazy<IProductTransformer>(() =>
                {
                    var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
                    var importStatisticsWriter = serviceProvider.GetRequiredService<IWriteImportStatistics>();

                    return new ProductTransformer(serviceScopeFactory, importStatisticsWriter);
                });
            });

            //services.AddTransient<IProductTransformer>((serviceProvider) =>
            //{
            //    return new NullProductTransformer();
            //});
            //services.AddTransient<IProductTransformer, NullProductTransformer>();

            services.AddOptions<ProductSourceOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(ProductSourceOptions.SectionName).Bind(options);
                });

            services.AddOptions<CsvProductTargetOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(CsvProductTargetOptions.SectionName).Bind(options);
                });

            return services;
        }
    }
}
