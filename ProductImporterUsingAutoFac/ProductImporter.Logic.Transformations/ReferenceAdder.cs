using ProductImporter.Logic.Transformations.Util;
using ProductImporter.Model;

namespace ProductImporter.Logic.Transformations;

public class ReferenceAdder : IProductTransformation
{
    public const string ReferencePrefix = "PI";
    private readonly IProductTransformationContext _productTransformationContext;
    private readonly IReferenceGenerator _refenceGenerator;

    public IHaveNoImplementation OptionalDependency { get; set; }
    public IProductTransformationContext ProductTransformationContext { get; set; }

    public ReferenceAdder(IProductTransformationContext productTransformationContext,
        Lazy<IReferenceGeneratorFactory> refenceGeneratorFactory)
    {
        _productTransformationContext = productTransformationContext;
        _refenceGenerator = refenceGeneratorFactory.Value.CreateReferenceGenerator(ReferencePrefix);        
    }

    public void Execute()
    {
        var product = ProductTransformationContext.GetProduct();

        var reference = _refenceGenerator.GetReference();

        var newProduct = new Product(product.Id, product.Name.ToLowerInvariant(), product.Price, product.Stock, reference);

        ProductTransformationContext.SetProduct(newProduct);
    }
}