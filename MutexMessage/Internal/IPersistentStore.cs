using System;
using System.Collections.Generic;

namespace MutexMessage.Internal
{
    /// <summary>
    /// 持久化接口
    /// </summary>
    public interface IPersistentStore
    {
        /// <summary>
        /// 存储键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Store(string key, object value);
        void Store(string key, object value, int expireMinutes);
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        object Get(string key);
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key);
        /// <summary>
        /// 存储键值集合
        /// </summary>
        /// <param name="collection">键值集合</param>
        void StoreCollection(Dictionary<string, object> collection);
        void StoreCollection<T>(Dictionary<string, T> collection);
        /// <summary>
        /// 根据键集合获取键值对集合
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>键值对集合</returns>
        Dictionary<string, object> GetCollection(List<string> keys);
        Dictionary<string, T> GetCollection<T>(List<string> keys);
        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);
        /// <summary>
        /// 删除键集合
        /// </summary>
        /// <param name="keys">键集合</param>
        void Remove(List<string> keys);
    }
}
