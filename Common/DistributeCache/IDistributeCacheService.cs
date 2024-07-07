using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.DistributeCache
{
    public interface IDistributeCacheService
    {
        /// <summary>
        /// 从缓存读取数据,读取不到再到数据库读取
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueSource"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        Task<TResult> GetOrCreateAsync<TResult>(string key, Func<TResult> valueSource, int expireSeconds = 60);

        /// <summary>
        /// 写入数据 覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        Task SetStringAsync(string key, string value, int expireSeconds);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        Task SetJsonStringAsync<T>(string key, T value, int expireSeconds);

        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="key"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="waitSeconds"></param>
        /// <returns></returns>
        Task<bool> tryLockAsync(string key, string token, int expireSeconds, int waitSeconds);

        /// <summary>
        /// 获取锁,并实现看门狗机制 过期时间小于三分之一时续期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="waitSeconds"></param>
        /// <returns></returns>
        Task<bool> LockAsync(string key,string token,int expireSeconds, int waitSeconds);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> unLockAsync(string key, string token);

        Task<string?> StringGetAsync(string key);
    }
}
