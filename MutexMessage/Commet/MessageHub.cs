using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MutexMessage.Common;
using System.Web;

namespace MutexMessage.Commet
{
    [HubName("MessageHub")]
    public class MessageHub:Hub
    {
        /// <summary>
        /// 订阅LoginChannel的登出操作
        /// </summary>
        internal void RecordUserLogout(object sender, LogoutArgs e)
        {
            var userCookie = HttpContext.Current.Request.Cookies[CookieKey.SignalRId];
            if (userCookie != null)
            {
                Clients.User(userCookie.Value).logout();
            }
        }
        /// <summary>
        /// 定义注册
        /// </summary>
        public void Register()
        {

        }
    }
}
