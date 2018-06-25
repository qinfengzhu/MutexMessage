using System;

namespace MutexMessage
{
    /// <summary>
    /// 消费者接口
    /// </summary>
    public interface IConsumer
    {
        #region 信号的申请
        ISignal ApplyFor(string key, object value);
        ISignal ApplyFor(object key, object value);
        ISignal ApplyFor(Func<string> funcKey, object value);
        ISignal ApplyFor(string key, object value,int exprireMinutes);
        ISignal ApplyFor(object key, object value, int exprireMinutes);
        ISignal ApplyFor(Func<string> funcKey, object value, int exprireMinutes);
        #endregion

        #region 信号的获取
        /// <summary>
        /// 获取一个信号
        /// </summary>
        /// <param name="funcKey">信号Key委托</param>
        /// <returns>信号</returns>
        ISignal GetSignal(Func<string> funcKey);
        /// <summary>
        /// 获取一个信号
        /// </summary>
        /// <param name="key">信号Key</param>
        /// <returns>信号</returns>
        ISignal GetSignal(string key);
        /// <summary>
        /// 获取一个信号
        /// </summary>
        /// <param name="key">信号Key</param>
        /// <returns>信号</returns>
        ISignal GetSignal(object key);
        #endregion

        #region 信号的移除
        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="signal">信号</param>
        void ReleaseSignal(ISignal signal);
        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="key">信号Key</param>
        void ReleaseSignal(string key);
        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="key">信号Key委托</param>
        void ReleaseSignal(Func<string> key);
        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="key">信号Key委托</param>
        void ReleaseSignal(object key);
        #endregion
    }
}
