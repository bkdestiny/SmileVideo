using Common.Utils;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.DistributeCache
{
    public class DistributedCacheService : IDistributeCacheService
    {
        private readonly IDistributedCache cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }
        private static DistributedCacheEntryOptions CreateOptions(int expiresSeconds)
        {
            //过期时间.Random.Shared 是.NET6新增的
            double sec = Random.Shared.NextDouble(expiresSeconds, expiresSeconds * 2);
            TimeSpan expiration = TimeSpan.FromSeconds(sec);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiration;
            return options;
        }
        public async Task SetStringAsync(string key,string value, int expireSeconds)
        {
            DistributedCacheEntryOptions options = CreateOptions(expireSeconds);
            await cache.SetStringAsync(key, value, options);
        }
        public async Task SetJsonStringAsync<T>(string key, T value, int expireSeconds)
        {
            DistributedCacheEntryOptions options = CreateOptions(expireSeconds);
            string jsonStr = JsonSerializer.Serialize(value, typeof(T));
            await cache.SetStringAsync(key, jsonStr, options);
        }
        public async Task<TResult> GetOrCreateAsync<TResult>(string key, Func<TResult> valueSource, int expireSeconds = 60)
        {
            string? value=cache.GetString(key);
            if(string.IsNullOrEmpty(value))
            {
                DistributedCacheEntryOptions options=CreateOptions(expireSeconds);
                TResult? result=valueSource.Invoke();
                string resultJsonStr=JsonSerializer.Serialize(result, typeof(TResult));
                await cache.SetStringAsync(key,(string)resultJsonStr, options);
                return result;
            }
            else
            {
                await cache.RefreshAsync(key);
                return JsonSerializer.Deserialize<TResult>(value)!;
            }
        }
        public async Task<bool> tryLockAsync(string key, string token,int expiresSeconds,int waitSeconds)
        {
            long endTime = DateTime.Now.AddSeconds(waitSeconds).Ticks;
            while (DateTime.Now.Ticks < endTime)
            {
                int count = 0;
                Console.WriteLine(token+"第" +(++ count) + "获取锁");
                if (cache.GetString(key) == null)
                {
                    Console.WriteLine("获取锁成功");
                    cache.SetString(key, token);
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> unLockAsync(string key, string token)
        {
            string value=await cache.GetStringAsync(key);
            if (value!=null&& value.Equals(token))
            {
                await cache.RemoveAsync(key);
                return true;
            }
            return false;
        }

        public Task<bool> LockAsync(string key, string token, int expireSeconds, int waitSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
