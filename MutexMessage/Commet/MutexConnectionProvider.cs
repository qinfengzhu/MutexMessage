using Microsoft.AspNet.SignalR;
using MutexMessage.Common;
using System;

namespace MutexMessage.Commet
{
    /// <summary>
    /// SignalR自定义ConnectionId
    /// </summary>
    public class MutexConnectionProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var signalrCookie = request.GetHttpContext().Request.Cookies[CookieKey.SignalRId];
            if (signalrCookie != null)
            {
                return signalrCookie.Value;
            }
            return string.Empty;
        }
    }
}
