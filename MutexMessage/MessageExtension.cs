using MutexMessage.Internal;
using MutexMessage.Signal;
using System;
using System.Text;

namespace MutexMessage
{
    /// <summary>
    /// Message-底层类库操作扩展
    /// </summary>
    internal static class MessageExtension
    {
        internal static readonly IPersistentStore PersistentStore;
        internal static readonly GetRedisKey GetRedisKey;
        static MessageExtension()
        {
            PersistentStore = new RedisStore();
            GetRedisKey = (tentanType,tentantId,uid) =>
            {
                StringBuilder builder = new StringBuilder();
                if (tentanType.IsNotEmpty())
                {
                    builder.AppendFormat("{0}{1}", tentanType, RedisSignal.SplitChar);
                }
                if (tentantId.IsNotEmpty())
                {
                    builder.AppendFormat("{0}{1}", tentantId, RedisSignal.SplitChar);
                }
                if (!uid.IsNotEmpty())
                {
                    uid = RandomKey.GetKey();
                }
                builder.AppendFormat("{0}", uid);
                return builder.ToString();
            };
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="redisSignal">Redis信号</param>
        /// <returns>Redis一个Key的值</returns>
        public static string GetKey(this RedisSignal redisSignal)
        {
            string rKey = GetRedisKey(redisSignal.TenantType, redisSignal.TenantId, redisSignal.UId);
            return rKey;
        }
        /// <summary>
        /// 字符串是否为空或者空白
        /// </summary>
        /// <param name="key">字符串</param>
        /// <returns>false空或者空白,true非空与空白</returns>
        public static bool IsNotEmpty(this string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                return false;
            return true;
        }
        /// <summary>
        /// 存储到Redis库中
        /// </summary>
        /// <param name="redisSignal"></param>
        public static void Store(this RedisSignal redisSignal)
        {
            var key = redisSignal.GetKey();
            PersistentStore.Store(key, redisSignal.Value);
        }
        public static void Store(this RedisSignal redisSignal,int expireMinutes)
        {
            var key = redisSignal.GetKey();
            PersistentStore.Store(key, redisSignal.Value, expireMinutes);
        }
        /// <summary>
        /// 从Redis库中删除
        /// </summary>
        /// <param name="redisSignal"></param>
        public static void Remove(this RedisSignal redisSignal)
        {
            var key = redisSignal.GetKey();
            PersistentStore.Remove(key);
        }
    }
    /// <summary>
    /// 随机Key
    /// </summary>
    public static class RandomKey
    {
        /// <summary>
        /// 获取一个Key
        /// </summary>
        /// <returns>字符类型Key</returns>
        public static string GetKey()
        {
            string key = Guid.NewGuid().ToString().ToLower();
            return key;
        }
    }
    /// <summary>
    /// 委托构造
    /// </summary>
    /// <returns></returns>
    internal delegate string GetRedisKey(string tentanType,string tentantId,string uid);
}
