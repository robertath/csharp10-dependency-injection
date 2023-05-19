using ProductImporter.Logic.Transformations.Util;
using ProductImporter.Model;

namespace ProductImporter.Logic.Transformations;

public class ReferenceAdder : IProductTransformation
{
    public const string ReferencePrefix = "PI";
    private readonly IProductTransformationContext _productTransformationContext;
    private readonly IReferenceGenerator _refenceGenerator;

    public ReferenceAdder(IProductTransformationContext productTransformationContext,
        IReferenceGeneratorFactory refenceGeneratorFactory)
    {
        _productTransformationContext = productTransformationContext;
        _refenceGenerator = refenceGeneratorFactory.CreateReferenceGenerator(ReferencePrefix);        
    }

    public void Execute()
    {
        var product = _productTransformationContext.GetProduct();

        var reference = _refenceGenerator.GetReference();

        var newProduct = new Product(product.Id, product.Name.ToLowerInvariant(), product.Price, product.Stock, reference);

        _productTransformationContext.SetProduct(newProduct);
    }
}