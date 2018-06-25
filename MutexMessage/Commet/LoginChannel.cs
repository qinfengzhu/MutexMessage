using MutexMessage.Consumer;
using System;

namespace MutexMessage.Commet
{
    /// <summary>
    /// 登出通道->方便与Hub之间进行登出登入的消息推送(目前功能定义于登出)
    /// </summary>
    public class LoginChannel
    {
        public static readonly LoginChannel ChannelInstance;
        static LoginChannel()
        {
            ChannelInstance = new LoginChannel();
        }
        public event LogoutEventHandler LogoutEvent;
        public event LoginEventHandler LoginEvent;
        private WebLoginRedisConsumer WebLoginRedisConsumer;
        private LoginChannel()
        {
        }
        protected void OnLogoutEvent(LogoutArgs e)
        {
            if (LogoutEvent != null)
            {
                LogoutEvent(this, e);
            }
        }
        protected void OnLoginEvent(LoginArg e)
        {
            if (LoginEvent != null)
            {
                LoginEvent(this, e);
            }
        }
        public void Init(string tentantType,string tentantId)
        {
            WebLoginRedisConsumer = new WebLoginRedisConsumer(tentantType, tentantId);
            ChannelInstance.LogoutEvent += WebLoginRedisConsumer.ChannelLogout;
            ChannelInstance.LoginEvent += WebLoginRedisConsumer.ChannelLogin;
        }
        /// <summary>
        /// 登出事件
        /// </summary>
        /// <param name="userRedisModel">用户信息</param>
        public void Logout(UserRedisModel userRedisModel)
        {
            var eventArgs = new LogoutArgs()
            {
                UserName = userRedisModel.UserName,
                UserId = userRedisModel.UserId,
                FullName = userRedisModel.FullName
            };
            OnLogoutEvent(eventArgs);
        }
        /// <summary>
        /// 登出事件
        /// </summary>
        /// <param name="userRedisModel">用户信息</param>
        public void Login(UserRedisModel userRedisModel)
        {
            var eventArgs = new LoginArg()
            {
                UserName = userRedisModel.UserName,
                UserId = userRedisModel.UserId,
                FullName = userRedisModel.FullName
            };
            OnLoginEvent(eventArgs);
        }
        /// <summary>
        /// 当前用户是否被后续登录者顶替
        /// </summary>
        /// <param name="userRedisModel">用户信息</param>
        /// <returns>true:被顶替了,false:没有被顶替</returns>
        public bool IsCurrentUserReplaced(UserRedisModel userRedisModel)
        {
            if(WebLoginRedisConsumer==null)
            {
                throw new Exception("please call the init method of LoginChannel !");
            }
            var result = WebLoginRedisConsumer.IsReplace(userRedisModel);
            return result;
        }
    }
    /// <summary>
    /// 登出事件参数
    /// </summary>
    public class LogoutArgs:EventArgs
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
    /// <summary>
    /// 登入事件参数
    /// </summary>
    public class LoginArg:EventArgs
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
    /// <summary>
    /// 登出事件委托
    /// </summary>
    /// <param name="sender">触发器</param>
    /// <param name="e">登出参数</param>
    public delegate void LogoutEventHandler(object sender, LogoutArgs e);
    public delegate void LoginEventHandler(object sender, LoginArg e);
}
