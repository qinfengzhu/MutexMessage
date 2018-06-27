using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using MutexMessage.Common;
using MutexMessage.Internal;
using Owin;
using System.Configuration;

[assembly:OwinStartup(typeof(MutexMessage.Commet.SignalRPubMessage))]
namespace MutexMessage.Commet
{
    public class SignalRPubMessage
    {
        public void Configuration(IAppBuilder app)
        {
            //使用Redis共享
            var config = ConfigurationManager.GetSection("redisServer") as RedisConfig;
            var defaultRedis = config.Servers[0];
            GlobalHost.DependencyResolver.UseRedis(defaultRedis.IP, defaultRedis.Port, config.Servers.Password, CookieKey.DefaultAppName());
            //定制Signalr的UserId
            var userIdProvider = new MutexConnectionProvider();           
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => userIdProvider);
            //开启SignalR通道
            app.MapSignalR();
        }
    }
}
