namespace MutexMessage
{
    /// <summary>
    /// 信号接口
    /// </summary>
    public interface ISignal
    {
        /// <summary>
        /// 租户类型
        /// </summary>
        string TenantType { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        string TenantId { get; set; }
        /// <summary>
        /// 租户唯一Id
        /// </summary>
        string UId { get; set; }
    }
}
