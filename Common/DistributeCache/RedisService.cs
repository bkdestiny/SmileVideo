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

        public async Task SetStringAsync(string key, string value, int expireSeconds)
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
/*            string tryLockScript= "local val=redis.call('exists','"+key+"')\n"+
            " if val==0 then\n" +
            "   redis.call('set','"+key+"','"+token+"')\n" +
            "   redis.call('expire','"+key+"',"+ expireSeconds + ")\n" +
            "   return 1\n" +
            " else\n" +
            " return false\n" +
            " end";*/
            string tryLockScript= "local val=redis.call('exists',@key)\n" +
            " if val==0 then\n" +
            "   redis.call('set',@key,@token)\n" +
            "   redis.call('expire',@key,@expire)\n" +
            "   return 1\n" +
            " else\n" +
            " return false\n" +
            " end";
            long endTime = DateTime.Now.AddSeconds(waitSeconds).Ticks;
            do
            {
                RedisResult redisReult = await GetDatabase().ScriptEvaluateAsync(LuaScript.Prepare(tryLockScript), new { key = key, token = token, expire = expireSeconds });
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
            string tryLockScript = "local r1=redis.call('exists',@key);" +
            " if r1==0 then" +
            "   redis.call('set',@key,@token);" +
            "   redis.call('expire',@key,@expire);" +
            "   return 1;" +
            " else" +
            " local expire=redis.call('ttl',@token);" +
            "   if(expire>=0 and expire<@watchDogExpire) then" +
            "       redis.call('expire',@key,@expire);" +
            "   end" +
            " return false;" +
            " end";
            long endTime = DateTime.Now.AddSeconds(waitSeconds).Ticks;
            do
            {
                RedisResult redisReult = await GetDatabase().ScriptEvaluateAsync(LuaScript.Prepare(tryLockScript), new {key=key,token=token,expire=expireSeconds,watchDogExpire=expireSeconds/3});
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
            string unLockScript = "local token=redis.call('get',@key);" +
                " if(token==@token) then" +
                " redis.call('del',@key);" +
                " return 1;" +
                " end" +
                " return false;";
            RedisResult redisResult=await GetDatabase().ScriptEvaluateAsync(LuaScript.Prepare(unLockScript), new {key=key,token=token});
            return redisResult.IsNull?false:true;
        }


        public async Task<string> StringGetAsync(string key)
        {
            return await GetDatabase().StringGetAsync(key);
        }
    }
}
