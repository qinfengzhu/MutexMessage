using System;

namespace MutexMessage
{
    /// <summary>
    /// 生产者接口
    /// </summary>
    internal interface IProducer
    {
        #region 生产信号
        /// <summary>
        /// 产生一个信号
        /// </summary>
        /// <param name="value">信号量值</param>
        /// <returns>信号</returns>
        ISignal CreateSignal(object value);
        ISignal CreateSignal(object value, int expireMinutes);
        /// <summary>
        /// 产生一个信号
        /// </summary>
        /// <param name="funcKey">信号Key委托</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号</returns>
        ISignal CreateSignal(Func<string> funcKey,object value);
        ISignal CreateSignal(Func<string> funcKey, object value, int expireMinutes);
        /// <summary>
        /// 产生一个信号
        /// </summary>
        /// <param name="key">信号Key</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号</returns>
        ISignal CreateSignal(string key,object value);
        ISignal CreateSignal(string key, object value, int expireMinutes);
        /// <summary>
        /// 产生一个信号
        /// </summary>
        /// <param name="key">信号Key</param>
        /// <param name="value">信号量值</param>
        /// <returns>信号</returns>
        ISignal CreateSignal(object key,object value);
        ISignal CreateSignal(object key, object value, int expireMinutes);
        #endregion

        #region 查找信号
        ISignal FindSignal(string key);
        ISignal FindSignal(object key);
        ISignal FindSignal(Func<string> funcKey);
        #endregion

        #region 移除信号
        void RemoveSignal(string key);
        void RemoveSignal(object key);
        void RemoveSignal(Func<string> key);
        #endregion
    }
}
