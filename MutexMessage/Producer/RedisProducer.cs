using MutexMessage.Signal;
using System;

namespace MutexMessage.Producer
{
    /// <summary>
    /// Redis-生产者
    /// </summary>
    internal class RedisProducer : IProducer
    {
        private string _tentantType;
        private string _tentantId;
        public RedisProducer()
            :this(string.Empty,string.Empty)
        {
        }
        public RedisProducer(string tentantType=null,string tenantId=null)
        {
            _tentantType = tentantType;
            _tentantId = tenantId;
        }
        #region 构造信号
        /// <summary>
        /// 创建一个Redis信号量
        /// </summary>
        /// <param name="value">信号量值</param>
        /// <returns>信号量</returns>
        public ISignal CreateSignal(object value)
        {
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = RandomKey.GetKey(),
                Value=value
            };
            signal.Store();
            return signal;
        }
        public ISignal CreateSignal(object value,int expireMinutes)
        {
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = RandomKey.GetKey(),
                Value = value
            };
            signal.Store(expireMinutes);
            return signal;
        }
        /// <summary>
        /// 创建一个Redis信号量
        /// </summary>
        /// <param name="key">信号量关键字</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号量</returns>
        public ISignal CreateSignal(object key,object value)
        {
            var signal = CreateSignal(key.ToString(),value);
            return signal;
        }
        public ISignal CreateSignal(object key, object value,int expireMinutes)
        {
            var signal = CreateSignal(key.ToString(), value, expireMinutes);
            return signal;
        }
        /// <summary>
        /// 根据关键字创建一个信号量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号量</returns>

        public ISignal CreateSignal(string key,object value)
        {
            var signal = new RedisSignal()
            {
                TenantType=_tentantType,
                TenantId=_tentantId,
                UId=key,
                Value=value
            };
            signal.Store();
            return signal;
        }
        public ISignal CreateSignal(string key, object value, int expireMinutes)
        {
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = key,
                Value = value
            };
            signal.Store(expireMinutes);
            return signal;
        }
        /// <summary>
        /// 根据关键字委托创建一个信号量
        /// </summary>
        /// <param name="funcKey">关键字委托</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号量</returns>
        public ISignal CreateSignal(Func<string> funcKey,object value)
        {
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = funcKey(),
                Value=value
            };
            signal.Store();
            return signal;
        }
        public ISignal CreateSignal(Func<string> funcKey, object value, int expireMinutes)
        {
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = funcKey(),
                Value = value
            };
            signal.Store(expireMinutes);
            return signal;
        }
        #endregion

        #region 查找信号
        public ISignal FindSignal(string key)
        {
            string rkey = MessageExtension.GetRedisKey(_tentantType, _tentantId, key);
            var value = MessageExtension.PersistentStore.Get(rkey);
            if (value == null)
                return null;
            var signal = new RedisSignal()
            {
                TenantType = _tentantType,
                TenantId = _tentantId,
                UId = key,
                Value = value
            };
            return signal;
        }
        public ISignal FindSignal(object key)
        {
            var lkey = key.ToString();
            return FindSignal(lkey);
        }
        public ISignal FindSignal(Func<string> funcKey)
        {
            var lkey = funcKey();
            return FindSignal(lkey);
        }
        #endregion

        #region 移除信号
        public void RemoveSignal(string key)
        {
            string rkey = MessageExtension.GetRedisKey(_tentantType, _tentantId, key);
            MessageExtension.PersistentStore.Remove(rkey);
        }
        public void RemoveSignal(object key)
        {
            string lkey = key.ToString();
            RemoveSignal(lkey);
        }
        public void RemoveSignal(Func<string> funcKey)
        {
            string lkey = funcKey();
            RemoveSignal(lkey);
        }
        #endregion
    }
}
