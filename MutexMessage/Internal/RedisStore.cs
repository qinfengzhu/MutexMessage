using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MutexMessage.Internal
{
    /// <summary>
    /// Redis-持久层
    /// </summary>
    public class RedisStore : IPersistentStore
    {
        private IDatabase redisDatabase;
        public RedisStore()
        {
            redisDatabase = RedisFactory.GetDataBase();
        }
        /// <summary>
        /// 根据Key获取Value值
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value值</returns>
        public object Get(string key)
        {
            var value = redisDatabase.StringGet(key);
            return value.ToObject();
        }

        public T Get<T>(string key)
        {
            var value = redisDatabase.StringGet(key);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 根据Key集合获取对应的字典集合
        /// </summary>
        /// <param name="keys">Key集合</param>
        /// <returns>字典集合</returns>
        public Dictionary<string, object> GetCollection(List<string> keys)
        {
            int length = keys.Count;
            var values = redisDatabase.StringGet(keys.ToRedisKey());
            var result = new Dictionary<string, object>(length);
            for(int i=0,l= length; i<l;i++)
            {
                result.Add(keys[i], values[i].ToObject());
            }
            return result;
        }

        public Dictionary<string, T> GetCollection<T>(List<string> keys)
        {
            int length = keys.Count;
            var values = redisDatabase.StringGet(keys.ToRedisKey());
            var result = new Dictionary<string, T>(length);
            for (int i = 0, l = length; i < l; i++)
            {
                result.Add(keys[i], values[i].ToObject<T>());
            }
            return result;
        }

        /// <summary>
        /// 删除Key集合
        /// </summary>
        /// <param name="keys">Key集合</param>
        public void Remove(List<string> keys)
        {
            redisDatabase.KeyDelete(keys.ToRedisKey());
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            redisDatabase.KeyDelete(key);
        }

        /// <summary>
        /// 存储KV值
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void Store(string key, object value)
        {
            redisDatabase.StringSet(key, value.ToJson());
        }
        /// <summary>
        /// 存储KV值过期时间
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expireMinutes">过期分钟</param>
        public void Store(string key,object value,int expireMinutes)
        {
            TimeSpan expire = TimeSpan.FromMinutes(expireMinutes);
            redisDatabase.StringSet(key, value.ToJson(), expire);
        }
        /// <summary>
        /// 存储KV集合
        /// </summary>
        /// <param name="collection">KV集合</param>
        public void StoreCollection(Dictionary<string, object> collection)
        {
            redisDatabase.StringSet(collection.ToKVPair());
        }

        public void StoreCollection<T>(Dictionary<string, T> collection)
        {
            redisDatabase.StringSet(collection.ToKVPair());
        }
    }
    /// <summary>
    /// 对Redis中操作的扩展
    /// </summary>
    internal static class RedisExtension
    {
        /// <summary>
        /// string 集合转换为 RedisKey 集合
        /// </summary>
        /// <param name="keys">string 集合</param>
        /// <returns>RedisKey 集合</returns>
        public static RedisKey[] ToRedisKey(this List<string> keys)
        {
            var redisKeys=keys.Select(t => (RedisKey)t).ToArray();
            return redisKeys;
        }

        /// <summary>
        /// 对象转换为Json
        /// </summary>
        /// <param name="redisValue">object对象</param>
        /// <returns>object-json序列化后字符串</returns>
        public static string ToJson(this object redisValue)
        {
            if (redisValue == null)
                return "";
            var jsonString = JsonConvert.SerializeObject(redisValue);
            return jsonString;
        }
        public static object ToObject(this RedisValue redisValue)
        {
            if (redisValue.IsNullOrEmpty)
                return default(object);
            return JsonConvert.DeserializeObject(redisValue);
        }
        /// <summary>
        /// RedisValue转换为实体对象
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="redisValue">实体对象字符串</param>
        /// <returns>T实体对象</returns>
        public static T ToObject<T>(this RedisValue redisValue)
        {
            if(redisValue.HasValue && !redisValue.IsNullOrEmpty)
            {
                var obj = JsonConvert.DeserializeObject<T>(redisValue);
                return obj;
            }
            return default(T);
        }

        /// <summary>
        /// 字典转换为Redis键值对集合
        /// </summary>
        /// <param name="collection">字典集合</param>
        /// <returns>Redis键值对数组集合</returns>
        public static KeyValuePair<RedisKey,RedisValue>[] ToKVPair(this Dictionary<string,object> collection)
        {
            int length = collection.Count;
            int index = 0;
            var result = new KeyValuePair<RedisKey, RedisValue>[length];
            foreach(KeyValuePair<string,object> kv in collection)
            {
                result[index] = new KeyValuePair<RedisKey, RedisValue>(kv.Key, kv.Value.ToJson());
                index++;
            }
            return result;
        }
        public static KeyValuePair<RedisKey, RedisValue>[] ToKVPair<T>(this Dictionary<string, T> collection)
        {
            int length = collection.Count;
            int index = 0;
            var result = new KeyValuePair<RedisKey, RedisValue>[length];
            foreach (KeyValuePair<string, T> kv in collection)
            {
                result[index] = new KeyValuePair<RedisKey, RedisValue>(kv.Key, kv.Value.ToJson());
                index++;
            }
            return result;
        }
    }
}
