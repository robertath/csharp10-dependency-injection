﻿using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Logic.Shared;
using ProductImporter.Logic.Source;
using ProductImporter.Logic.Target;
using ProductImporter.Logic.Transformation;

namespace ProductImporter.Logic;

public class ProductImporter
{
    private readonly IProductSource _productSource;    
    private readonly Lazy<IProductTransformer> _productTransformer;
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductTarget _productTarget;
    private readonly IGetImportStatistics _importStatistics;

    public ProductImporter(
            IProductSource productSource,
            Lazy<IProductTransformer> productTransformer,
            IProductTarget productTarget,
            IGetImportStatistics importStatistics)
    {
        _productSource = productSource;
        _productTransformer = productTransformer;
        _productTarget = productTarget;
        _importStatistics = importStatistics;
    }


    public async Task RunAsync()
    {
        await _productSource.OpenAsync();

        _productTarget.Open();

        while (_productSource.hasMoreProducts())
        {
            var product = _productSource.GetNextProduct();

            var transformedProduct = _productTransformer.Value.ApplyTransformations(product);

            _productTarget.AddProduct(transformedProduct);
        }

        _productSource.Close();
        _productTarget.Close();

        Console.WriteLine("Importing complete!");
        Console.WriteLine(_importStatistics.GetStatistics());
    }
}