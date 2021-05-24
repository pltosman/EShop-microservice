using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EShop.Core.Helpers
{
    public static class RedisHelper
    {
        public async static Task<T> GetT<T>(this IDistributedCache cache, string key)
        {
            string json = await cache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        public async static void Set(this IDistributedCache cache, string key, object value)
        {
            string json = JsonConvert.SerializeObject(value);

            await cache.SetAsync(key, Encoding.UTF8.GetBytes(json));
        }
    }
}
