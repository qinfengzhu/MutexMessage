using System;

namespace MutexMessage
{
    /// <summary>
    /// 用户的登录-登出模型
    /// </summary>
    public class UserRedisModel
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户全名
        /// </summary>
        public string FullName { get; set; }
    }
}
