using Autofac;
using Autofac.Extras.DynamicProxy;
using ProductImporter.Logic.Transformations.Util;

namespace ProductImporter.Logic.Transformations;

public class ProductImporterTransformationsModule : Module
{
    private readonly Action<ProductTransformationOptions> _optionsProvider;

    public ProductImporterTransformationsModule(Action<ProductTransformationOptions> optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var options = new ProductTransformationOptions();
        _optionsProvider(options);

        //to provide all implementatinos of IProductTransformation, as bellow commented
        builder
            .RegisterAssemblyTypes(typeof(ProductImporterTransformationsModule).Assembly)
            .Where(x => x.IsAssignableTo(typeof(IProductTransformation)))
            .InstancePerLifetimeScope()
            .PropertiesAutowired()
            .AsImplementedInterfaces()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(SystemOutLogger));


        builder.RegisterType<ProductTransformationContext>().As<IProductTransformationContext>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        
        //builder.RegisterType<NameDecapitaliser>().As<IProductTransformation>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        //builder.RegisterType<DisposableProductTransformation>().As<IProductTransformation>().InstancePerDependency().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));

        //if (options.EnableCurrencyNormalizer)
        //{
        //    builder.RegisterType<CurrencyNormalizer>().As<IProductTransformation>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        //}
        //else
        //{
        //    builder.RegisterType<NullCurrencyNormalizer>().As<IProductTransformation>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        //}

        builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        //builder.RegisterType<ReferenceAdder>().As<IProductTransformation>().InstancePerLifetimeScope().PropertiesAutowired().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        builder.RegisterType<ReferenceGeneratorFactory>().As<IReferenceGeneratorFactory>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));
        builder.RegisterType<IncrementingCounter>().As<IIncrementingCounter>().SingleInstance().EnableInterfaceInterceptors().InterceptedBy(typeof(SystemOutLogger));

        builder.Register(_ => new SystemOutLogger());
    }
}