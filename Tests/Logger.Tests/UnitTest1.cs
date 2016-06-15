using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Net;
using Logger.Models;
using Newtonsoft.Json;

namespace Logger.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var model = new Logs();
            model.Browser = "Chrome";
            model.CreateDate = DateTime.Now;
            model.Ip = "10.40.32.87";
            model.Level = 1;
            model.Url = "http://localhost:25769/home/index";
            model.Uid = "s85fs-2gg4";
            string postData = JsonConvert.SerializeObject(model);
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("Content-Type", "application/json");
            var responseString = client.UploadString("http://localhost:81/api/logs", "post", postData);
            Assert.IsNotNull(responseString);
        }
    }
}
