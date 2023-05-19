using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductImporter.Logic.Transformations.Util;

public class DateTimeProvider : IDateTimeProvider, IDisposable
{
    private readonly DateTime _currentDateTime;

    public DateTimeProvider()
    {
        _currentDateTime = DateTime.UtcNow;
    }

    public void Dispose()
    {
        ;
    }

    public DateTime GetUtcDateTime()
    {
        return _currentDateTime;
    }
}
