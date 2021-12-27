using System.Collections.Generic;
using System.Linq;

namespace BL.API.Services.Extensions
{
    public static class EnumerableExtensions
    {
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return source.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
