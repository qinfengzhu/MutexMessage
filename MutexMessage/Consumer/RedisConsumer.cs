using MutexMessage.Producer;
using MutexMessage.Signal;
using System;

namespace MutexMessage.Consumer
{
    /// <summary>
    /// Redis-消费者
    /// </summary>
    public class RedisConsumer : IConsumer
    {
        private string _tentantType;
        private string _tentantId;
        private IProducer _producer;
        public RedisConsumer()
            :this(string.Empty,string.Empty)
        {
        }
        public RedisConsumer(string tentantType = null, string tenantId = null)
        {
            _tentantType = tentantType;
            _tentantId = tenantId;
            _producer = new RedisProducer(tentantType, tenantId);
        }
        #region 申请信号量
        public ISignal ApplyFor(string key, object value)
        {
            var signal = _producer.CreateSignal(key, value);
            return signal;
        }
        public ISignal ApplyFor(object key, object value)
        {
            var signal = _producer.CreateSignal(key, value);
            return signal;
        }
        public ISignal ApplyFor(Func<string> funcKey, object value)
        {
            var signal = _producer.CreateSignal(funcKey, value);
            return signal;
        }
        public ISignal ApplyFor(string key,object value, int exprireMinutes)
        {
            var signal = _producer.CreateSignal(key, value, exprireMinutes);
            return signal;
        }
        public ISignal ApplyFor(object key,object value, int exprireMinutes)
        {
            var signal = _producer.CreateSignal(key, value, exprireMinutes);
            return signal;
        }
        public ISignal ApplyFor(Func<string> funcKey,object value, int exprireMinutes)
        {
            var signal = _producer.CreateSignal(funcKey, value, exprireMinutes);
            return signal;
        }
        #endregion

        #region 获取信号量
        public ISignal GetSignal(object key)
        {
            var signal = _producer.FindSignal(key);
            return signal;
        }

        public ISignal GetSignal(string key)
        {
            var signal = _producer.FindSignal(key);
            return signal;
        }

        public ISignal GetSignal(Func<string> funcKey)
        {
            var signal = _producer.FindSignal(funcKey);
            return signal;
        }
        #endregion

        #region 释放信号量
        public void ReleaseSignal(object key)
        {
            _producer.RemoveSignal(key);
        }

        public void ReleaseSignal(Func<string> key)
        {
            _producer.RemoveSignal(key);
        }

        public void ReleaseSignal(string key)
        {
            _producer.RemoveSignal(key);
        }

        public void ReleaseSignal(ISignal signal)
        {
            var rsignal = signal as RedisSignal;
            if(signal!=null)
                rsignal.Remove();
        }
        #endregion
    }
}
