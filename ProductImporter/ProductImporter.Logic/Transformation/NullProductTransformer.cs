using ProductImporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductImporter.Logic.Transformation;

public class NullProductTransformer : IProductTransformer
{
    public Product ApplyTransformations(Product product)
    {
        return product;
    }
}
