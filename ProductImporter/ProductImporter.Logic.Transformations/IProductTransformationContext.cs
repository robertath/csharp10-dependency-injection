using ProductImporter.Model;

namespace ProductImporter.Logic.Transformations;

public interface IProductTransformationContext
{
    void SetProduct(Product product);
    public Product GetProduct();
    bool IsProductChanged();
}
