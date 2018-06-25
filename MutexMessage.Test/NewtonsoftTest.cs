using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MutexMessage.Test
{
    [TestFixture]
    public class NewtonsoftTest
    {
        [Test]
        public void StringJsonTest()
        {
            string userName = "bestkf";
            var result = JsonConvert.SerializeObject(userName);

            Assert.AreEqual("\"bestkf\"", result);
        }
        [Test]
        public void JsonToStringTest()
        {
            var jsonStr = "\"bestkf\"";
            string userName = JsonConvert.DeserializeObject<string>(jsonStr);
            Assert.AreEqual("bestkf", userName);
        }
        [Test]
        public void IntJsonTest()
        {
            int page = 20;
            var result = JsonConvert.SerializeObject(page);
            Assert.AreEqual("20", result);
        }
        [Test]
        public void JsonToIntTest()
        {
            var jsonStr = "20";
            int page = JsonConvert.DeserializeObject<int>(jsonStr);
            Assert.AreEqual(20, page);
        }
        [Test]
        public void DynamicObjectJsonTest()
        {
            var obj = new
            {
                UserName = "bestkf",
                Age = 22
            };
            var result = JsonConvert.SerializeObject(obj);
            Assert.AreEqual("{\"UserName\":\"bestkf\",\"Age\":22}", result);
        }
    }
}
