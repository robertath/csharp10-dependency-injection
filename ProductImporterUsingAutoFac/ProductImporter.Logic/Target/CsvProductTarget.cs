using CsvHelper;
using Microsoft.Extensions.Options;
using ProductImporter.Logic.Shared;
using ProductImporter.Model;

namespace ProductImporter.Logic.Target;

public class CsvProductTarget : IProductTarget, IDisposable
{
    private readonly IOptions<CsvProductTargetOptions> _csvProductTargetOptions;
    private readonly IWriteImportStatistics _importStatistics;

    private CsvWriter _csvWriter;
    

    public CsvProductTarget(IOptions<CsvProductTargetOptions> csvProductTargetOptions, 
        IWriteImportStatistics importStatistics)
    {
        _csvProductTargetOptions = csvProductTargetOptions;
        _importStatistics = importStatistics;
    }

    public void Open()
    {
        var streamWriter = new StreamWriter(_csvProductTargetOptions.Value.TargetCsvPath);
        _csvWriter = new CsvWriter(streamWriter, 
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    false);

        _csvWriter.WriteHeader<Product>();
        _csvWriter.NextRecord();
    }

    public void AddProduct(Product product)
    {
        if (_csvWriter == null)
            throw new InvalidOperationException("Cannot add products to a target that is not yet open");

        _csvWriter.WriteRecord(product);
        _csvWriter.NextRecord();

        _importStatistics.IncrementOutputCount();
    }

    public void Close()
    {
        if (_csvWriter == null)
            throw new InvalidOperationException("Cannot close a target that is not yet open");

        _csvWriter.Flush();
    }

    public void Dispose()
    {
        _csvWriter.Dispose();
    }
}

