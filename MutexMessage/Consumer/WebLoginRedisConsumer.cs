using MutexMessage.Commet;
using MutexMessage.Common;
using MutexMessage.Signal;
using Microsoft.AspNet.SignalR;
using System;
using System.Web;

namespace MutexMessage.Consumer
{
    /// <summary>
    /// Web-Login登录Redis消费者
    /// </summary>
    public class WebLoginRedisConsumer
    {
        private IConsumer internalConsumer;
        public WebLoginRedisConsumer(string tentantType,string tentantId)
        {
            internalConsumer = new RedisConsumer(tentantType, tentantId);
        }
        /// <summary>
        /// 登录并且记录当前用户的登录标记
        /// </summary>
        /// <param name="userRedisModel">当前登录信息</param>
        /// <returns>登录标记</returns>
        public string SignIn(UserRedisModel userRedisModel)
        {
            var signal = internalConsumer.GetSignal(userRedisModel.UserName);
            string loginKey = RandomKey.GetKey();
            #region 释放已经登录信号,创建新的登录信号
            if(signal != null)
            {
                LoginChannel.ChannelInstance.Logout(userRedisModel);
                internalConsumer.ReleaseSignal(signal);
            }
            internalConsumer.ApplyFor(userRedisModel.UserName, loginKey);
            #endregion
            #region 客户端写入SignalRId
            var signalRId = new HttpCookie(CookieKey.SignalRId)
            {
                Name = CookieKey.SignalRId,
                Expires = DateTime.Now.AddDays(1),
                Value=loginKey
            };
            HttpContext.Current.Response.Cookies.Add(signalRId);
            #endregion
            return loginKey;
        }
        /// <summary>
        /// 登出并且删除
        /// </summary>
        /// <param name="userRedisModel">当前登录信息</param>
        public void SignOut(UserRedisModel userRedisModel)
        {
            var signal = (RedisSignal)internalConsumer.GetSignal(userRedisModel.UserName);
            if (signal != null)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                hubContext.Clients.User(signal.Value.ToString()).logout();
            }
            var signalRCookie = HttpContext.Current.Request.Cookies[CookieKey.SignalRId];
            if (signalRCookie != null)
            {
                if (signal != null && signal.Value.ToString() == signalRCookie.Value)
                {
                    internalConsumer.ReleaseSignal(signal);
                }
                signalRCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(signalRCookie);
            }
        }
        /// <summary>
        /// 是否用户被替代了
        /// </summary>
        /// <param name="userRedisModel">当前登录信息</param>
        /// <returns>true:用户被替代,false:用户没有被替代</returns>
        public bool IsReplace(UserRedisModel userRedisModel)
        {
            var signal = (RedisSignal)internalConsumer.GetSignal(userRedisModel.UserName);
            var cookie = HttpContext.Current.Request.Cookies[CookieKey.SignalRId];
            bool isReplaced = false;
            if(cookie!=null&&signal!=null&&cookie.Value==signal.Value.ToString())
            {
                isReplaced = false;
            }
            else
            {
                isReplaced = true;
            }
            return isReplaced;
        }
        internal void ChannelLogout(object sender, LogoutArgs e)
        {
            var userRedisModel = new UserRedisModel()
            {
                UserName = e.UserName,
                UserId = e.UserId,
                FullName = e.FullName
            };
            SignOut(userRedisModel);
        }
        internal void ChannelLogin(object sender, LoginArg e)
        {
            var userRedisModel = new UserRedisModel()
            {
                UserName = e.UserName,
                UserId = e.UserId,
                FullName = e.FullName
            };
            SignIn(userRedisModel);
        }
    }
}
