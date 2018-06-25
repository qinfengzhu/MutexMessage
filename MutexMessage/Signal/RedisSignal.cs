using System;
namespace MutexMessage.Signal
{
    /// <summary>
    /// Redis信号
    /// </summary>
    public class RedisSignal:ISignal
    {
        public const string SplitChar = ":";
        /// <summary>
        /// 租户类型
        /// </summary>
        public string TenantType { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// 租户唯一Id
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// 信号值
        /// </summary>
        public object Value { get; set; }
    }
    /// <summary>
    /// 泛类型Redis信号
    /// </summary>
    /// <typeparam name="T">值的类型</typeparam>
    public class RedisSignal<T>:ISignal
    {
        /// <summary>
        /// 租户类型
        /// </summary>
        public string TenantType { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// 租户唯一Id
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// 信号值
        /// </summary>
        public T Value { get; set; }
    }
}
