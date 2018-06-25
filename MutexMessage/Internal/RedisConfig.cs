using System;
using System.Configuration;

namespace MutexMessage.Internal
{
    /// <summary>
    /// Redis-配置
    /// </summary>
    public class RedisConfig:ConfigurationSection
    {
        [ConfigurationProperty("servers",IsRequired =true)]
        public RedisCollection Servers
        {
            get { return this["servers"] as RedisCollection; }
        }
    }
    /// <summary>
    /// Redis-配置中的服务器集合
    /// </summary>
    [ConfigurationCollection(typeof(RedisServer),AddItemName ="server")]
    public class RedisCollection:ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RedisServer();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            var server = element as RedisServer;
            return server.IP;
        }
        public RedisServer this[int i]
        {
            get { return BaseGet(i) as RedisServer; }
        }
        /// <summary>
        /// Redis服务器名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }
        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
        }
        [ConfigurationProperty("timeout")]
        public int Timeout
        {
            get { return (int)this["timeout"]; }
        }
        [ConfigurationProperty("database")]
        public int Database
        {
            get { return (int)this["database"]; }
        }
    }
    /// <summary>
    /// Redis-配置中的单个服务器节点
    /// </summary>
    public class RedisServer:ConfigurationElement
    {
        /// <summary>
        /// Redis服务器Ip
        /// </summary>
        [ConfigurationProperty("ip", IsRequired = true)]
        public string IP
        {
            get { return (string)this["ip"]; }
        }
        /// <summary>
        /// Redis服务器端口号
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
        }
    }
}
