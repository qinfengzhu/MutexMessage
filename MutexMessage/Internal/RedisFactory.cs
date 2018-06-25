using StackExchange.Redis;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MutexMessage.Internal
{
    /// <summary>
    /// Redis-工厂
    /// </summary>
    public class RedisFactory
    {
        private static Dictionary<string, ConnectionMultiplexer> RedisPool;
        private static object lockObj = new object();
        private static int databaseIndex = 0;
        private static readonly RedisFactory Instance = new RedisFactory();
        private RedisFactory() { }
        /// <summary>
        /// 获取RedisServer连接端
        /// </summary>
        public ConnectionMultiplexer RedisServer
        {
            get
            {
                if (RedisPool == null||RedisPool.Count==0)
                {
                    lock (lockObj)
                    {
                        if (RedisPool == null || RedisPool.Count == 0)
                        {
                            RedisPool = new Dictionary<string, ConnectionMultiplexer>();
                            var config = ConfigurationManager.GetSection("redisServer") as RedisConfig;
                            databaseIndex = config.Servers.Database;
                            var options = new ConfigurationOptions()
                            {
                                Password = config.Servers.Password,
                                DefaultDatabase = config.Servers.Database
                            };
                            foreach (RedisServer server in config.Servers)
                            {
                                options.EndPoints.Add(server.IP, server.Port);
                            }
                            var connector = ConnectionMultiplexer.Connect(options);
                            RedisPool.Add(config.Servers.Name, connector);
                        }
                    }
                }
                return RedisPool.FirstOrDefault().Value;
            }
        }
        /// <summary>
        /// 获取Redis数据库
        /// </summary>
        /// <param name="database">数据库index</param>
        /// <returns>Redis数据库连接</returns>
        public static IDatabase GetDataBase(int database=-1)
        {
            if(database==-1)
            {
                return Instance.RedisServer.GetDatabase(databaseIndex);
            }
            else
            {
                return Instance.RedisServer.GetDatabase(database);
            }
        }
    }
}
