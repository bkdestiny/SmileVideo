using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.DistributeCache
{
    public class RedisService : IDistributeCacheService,IDisposable
    {
        private string _connectionString; //连接字符串
        private string _instanceName; //实例名称
        private int _defaultDB; //默认数据库
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

        public RedisService(string connectionString, string instanceName, int defaultDB = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDB = defaultDB;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="db">默认为0：优先代码的db配置，其次config中的配置</param>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase(_defaultDB);
        }

        private IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        private ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }

        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }

        public async Task<TResult> GetOrCreateAsync<TResult>(string key, Func<TResult> valueSource, int expireSeconds = 60)
        {
            var db = GetDatabase();
            string? value= await db.StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                TResult result=valueSource.Invoke();
                string resultJsonStr=JsonSerializer.Serialize(result, typeof(TResult));
                await db.StringSetAsync(key,resultJsonStr);
                return result;
            }
            else
            {
                return JsonSerializer.Deserialize<TResult>(value)!;
            }
        }

        public async Task SetStringAsync<T>(string key, string value, int expireSeconds)
        {
            await GetDatabase().StringSetAsync(key, value, TimeSpan.FromSeconds(expireSeconds));
        }
        public async Task SetJsonStringAsync<T>(string key, T value, int expireSeconds)
        {
            string jsonStr = JsonSerializer.Serialize(value, typeof(T));
            await GetDatabase().StringSetAsync(key, jsonStr, TimeSpan.FromSeconds(expireSeconds));
        }

        public async Task<bool> tryLockAsync(string key,string token, int expireSeconds, int waitSeconds)
        {
            string tryLockExec = "local val=redis.call('exists',"+key+");"+
            " if val==0 then" +
            "   redis.call('set',"+key+","+token+");" +
            "   redis.call('expire',"+key+","+ expireSeconds + ");" +
            "   return 1;" +
            " else" +
            " return false;" +
            " end";
            long endTime = DateTime.Now.AddSeconds(waitSeconds).Ticks;
            do
            {
                RedisResult redisReult = await GetDatabase().ScriptEvaluateAsync(tryLockExec);
                if (!redisReult.IsNull)
                {
                    return true;
                }
            }
            while (waitSeconds!=0&&DateTime.Now.Ticks<endTime);
            return false;
        }
        public async Task<bool> LockAsync(string key, string token, int expireSeconds, int waitSeconds)
        {
            string tryLockExec = "local r1=redis.call('exists'," + key + ");" +
            " if r1==0 then" +
            "   redis.call('set'," + key + "," + token + ");" +
            "   redis.call('expire'," + key + "," + expireSeconds + ");" +
            "   return 1;" +
            " else" +
            " local expire=redis.call('ttl'," + key + ");" +
            "   if(expire>=0 and expire<" + expireSeconds / 3 + ") then" +
            "       redis.call('expire'," + key + "," + expireSeconds + ");" +
            "   end" +
            " return false;" +
            " end";
            long endTime = DateTime.Now.AddSeconds(waitSeconds).Ticks;
            do
            {
                RedisResult redisReult = await GetDatabase().ScriptEvaluateAsync(tryLockExec);
                if (!redisReult.IsNull)
                {
                    return true;
                }
            }
            while (waitSeconds != 0 && DateTime.Now.Ticks < endTime);
            return false;
        }

        public async Task<bool> unLockAsync(string key, string token)
        {
            string unLockExec = "local token=redis.call('get',"+key+");" +
                " if(token=="+token+") then" +
                " redis.call('del',"+key+");" +
                " return 1;" +
                " end" +
                " return false;";
            RedisResult redisResult=await GetDatabase().ScriptEvaluateAsync(unLockExec);
            return redisResult.IsNull?false:true;
        }

        public Task SetStringAsync(string key, string value, int expireSeconds)
        {
            throw new NotImplementedException();
        }


    }
}
