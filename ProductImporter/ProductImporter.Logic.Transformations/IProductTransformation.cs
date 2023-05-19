namespace ProductImporter.Logic.Transformations;

public interface IProductTransformation
{
    void Execute();
}


public class DisposingProductTransformation : IProductTransformation, IDisposable
{
    public void Dispose()
    {
        ;
    }

    public void Execute()
    {
        ;
    }
}
