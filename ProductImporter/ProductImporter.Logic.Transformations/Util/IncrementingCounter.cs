using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductImporter.Logic.Transformations.Util;

public class IncrementingCounter : IIncrementingCounter, IDisposable
{
    private int _counter = -1;

    public void Dispose()
    {
        ;
    }

    public int GetNext()
    {
        _counter++;

        return _counter;
    }
}

