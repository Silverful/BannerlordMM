using System;
using System.Threading.Tasks;

namespace BL.API.Core.Abstractions.Services
{
    public interface ICacheProvider
    {
        object GetCachedResponse(object key, Func<Task<object>> getDataFunc);
        bool TryGetValue(object key, out object value);
        bool TryRemoveValue(object key);
    }
}
