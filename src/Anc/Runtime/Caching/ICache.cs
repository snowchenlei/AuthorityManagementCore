using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Runtime.Caching
{
    public interface ICache : IDisposable
    {
        object Get(object key);

        TItem Get<TItem>(object key);

        bool TryGetValue<TItem>(object key, out TItem value);

        TItem Set<TItem>(object key, TItem value);

        TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration);

        TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
    }
}