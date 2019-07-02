//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 14:21:12
//    说明：Redis管理类
//    版权所有：个人
//***********************************************************
using Newtonsoft.Json;
using OARMS.Core.Cache;
using OARMS.Core.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OARMS.Redis
{
    public partial class RedisCacheManage : IRedisCache
    {
        #region 定义
        private readonly IRedisCache _perRequestCacheManager;
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db; 
        #endregion

        #region 构造函数 
        public RedisCacheManage(IRedisCache redisCache, IRedisConnectionWrapper redisConnectionWrapper, OarmsConfig config)
        {
            if (string.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis连接字符串不能为空！");
            this._perRequestCacheManager = redisCache;
            this._connectionWrapper = redisConnectionWrapper;
            this._db = _connectionWrapper.GetDatabase();
        }
        #endregion

        #region 类内实现的Reids操作方法
        /// <summary>
        /// 异步的方式获取缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetAsync<T>(string key)
        {
            if (_perRequestCacheManager.IsExist(key))
                return _perRequestCacheManager.Get<T>(key, () => default(T), 0);
            var serializedItem = await _db.StringGetAsync(key);
            if (!serializedItem.HasValue)
                return default(T);
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);
            _perRequestCacheManager.Set(key, item, 0);
            return item;
        }
        /// <summary>
        /// 异步方式设置缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;
            //设置缓存时间
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            //序列化data
            var serializedItem = JsonConvert.SerializeObject(data);

            //将数据添加到Redis中
            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }
        /// <summary>
        /// 异步方式判断是否已存在key集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExistAsync(string key)
        {
            if (_perRequestCacheManager.IsExist(key))
                return true;
            return await _db.KeyExistsAsync(key);
        }
        /// <summary>
        /// 通过key检索异步方式移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task RemoveAsync(string key)
        {
            //始终保留保护key的数据
            if (key.Equals(OarmsCachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
                return;
            //从Redis中移除key
            await _db.KeyDeleteAsync(key);
            _perRequestCacheManager.Remove(key);
        }
        /// <summary>
        /// 通过正则表达式移除数据
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual async Task RemoveByPatternAsync(string pattern)
        {
            _perRequestCacheManager.RemoveByPattern(pattern);
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endPoint);
                var keys = server.Keys(database: _db.Database, pattern: $"*{pattern}*");

                //始终保留保护key的数据
                keys = keys.Where(key => !key.ToString().Equals(OarmsCachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));

                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }
        /// <summary>
        /// 异步方式清除Reids中的数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task ClearAsync()
        {
            _perRequestCacheManager.Clear();

            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endPoint);
                //使用下面注释的方法也可以实现，但是需要管理员权限 - ",allowAdmin=true"
                //server.FlushDatabase();
                //所以通过手动删除所有的元素
                var keys = server.Keys(database: _db.Database);
                //始终保留保护key的数据
                keys = keys.Where(key => !key.ToString().Equals(OarmsCachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));

                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }
        #endregion

        #region IRedisCache 接口实现
        public virtual async void Clear()
        {
            await this.ClearAsync();
        }

        public virtual void Dispose()
        {
        }

        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            if (this.IsExistAsync(key).Result)
                return this.GetAsync<T>(key).Result;
            //使用函数创建
            var result = acquire();
            //在缓存中设置 (如果缓存定义了缓存时间)
            if ((cacheTime ?? OarmsCachingDefaults.CacheTime) > 0)
                this.SetAsync(key, result, cacheTime ?? OarmsCachingDefaults.CacheTime).Wait();
            return result;
        }
        public virtual bool IsExist(string key)
        {
            return this.IsExistAsync(key).Result;
        }

        public virtual async void Remove(string key)
        {
            await this.RemoveAsync(key);
        }

        public virtual async void RemoveByPattern(string pattern)
        {
            await this.RemoveByPatternAsync(pattern);
        }

        public virtual async void Set(string key, object data, int cacheTime)
        {
            await this.SetAsync(key, data, cacheTime);
        } 
        #endregion
    }
}
