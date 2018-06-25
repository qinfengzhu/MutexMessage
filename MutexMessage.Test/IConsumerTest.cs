using System;
using NUnit.Framework;
using MutexMessage;
using MutexMessage.Consumer;
using System.Configuration;
using MutexMessage.Internal;

namespace Message.Test
{
    /// <summary>
    /// 消费者测试
    /// </summary>
    [TestFixture]
    public class IConsumerTest
    {
        private IConsumer consumer;
        [SetUp]
        public void Init()
        {
            string tentanType = "CTS";
            string tentantId = "Login";
            consumer = new RedisConsumer(tentanType,tentantId);
        }
        [Test]
        public void GetConfig()
        {
            var config = ConfigurationManager.GetSection("redisServer") as RedisConfig;
            Assert.IsNotNull(config);
        }
        /// <summary>
        /// 创建信号量
        /// </summary>
        [Test]
        public void CreateSignal()
        {
            string userId = "12aaf50353844a12b5519f4587b8d564";
            var userInfo = new { UserName="candy",Department="测试部门" };

            //申请一个信号,设置时间为5分钟
            var signal = consumer.ApplyFor(userId, userInfo, 5);
            //释放一个信号
            consumer.ReleaseSignal(signal);
            //查找一个信号
            var findSignal = consumer.GetSignal(userId);
            //信号是否为空
            Assert.IsNull(findSignal);
        }
        /// <summary>
        /// 获取信号量
        /// </summary>
        [Test]
        public void GetSignal()
        {
            string userId = "12aaf50353844a12b5519f4587b8d564";
            var userInfo = new { UserName = "candy", Department = "测试部门" };
            //申请一个信号,设置时间为5分钟
            var signal = consumer.ApplyFor(userId, userInfo, 5);
            //查找一个信号
            var findSignal = consumer.GetSignal(userId);
            //信号是否为空
            Assert.IsNotNull(findSignal);
        }
    }
}
