using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Threading
{
    public interface IAmbientScopeProvider<T>
    {
        T GetValue(string contextKey);

        IDisposable BeginScope(string contextKey, T value);
    }
}