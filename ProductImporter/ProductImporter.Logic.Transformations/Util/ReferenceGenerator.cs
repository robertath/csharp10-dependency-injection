using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductImporter.Logic.Transformations.Util;

public class ReferenceGenerator : IReferenceGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IIncrementingCounter _incrementingCounter;
    private int counter = -1;
    private readonly string _prefix;

    public ReferenceGenerator(IDateTimeProvider dateTimeProvider, 
        IIncrementingCounter incrementingCounter,
        string prefix)
    {
        _dateTimeProvider = dateTimeProvider;
        _incrementingCounter = incrementingCounter;
        _prefix = prefix;
    }

    public string GetReference()
    {
        counter++;
        var dateTime = _dateTimeProvider.GetUtcDateTime();

        var reference = $"{_prefix}_{dateTime:yyyy-MM-ddTHH:mm:ss.FFF}-{_incrementingCounter.GetNext():D4}";

        return reference;
    }    
}

public interface IReferenceGeneratorFactory
{
    IReferenceGenerator CreateReferenceGenerator(string prefix);
}

public class ReferenceGeneratorFactory : IReferenceGeneratorFactory
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IIncrementingCounter _increaseingCounter;

    public ReferenceGeneratorFactory(IDateTimeProvider dateTimeProvider, IIncrementingCounter increaseingCounter)
    {
        _dateTimeProvider = dateTimeProvider;
        _increaseingCounter = increaseingCounter;
    }

    public IReferenceGenerator CreateReferenceGenerator(string prefix)
    {
        return new ReferenceGenerator(_dateTimeProvider, _increaseingCounter, prefix);   
    }
}