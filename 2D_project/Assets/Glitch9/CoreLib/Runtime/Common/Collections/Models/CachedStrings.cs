using System.Collections.Concurrent;

namespace Glitch9.Collections
{
    public class CachedStrings<TKey> : ConcurrentDictionary<TKey, string>
    {
    }
}