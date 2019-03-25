using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aigang.AndroidDataCollector.Client;
using Microsoft.Extensions.Configuration;
using Aigang.Platform.Utils;

namespace Playground
{
    [TestClass]
    public class Test_AndroidDataCollectorClient
    {
        [TestInitialize]
        public void TestInitialize()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            ConfigurationManager.Configuration = builder.Build();
        }
        
        
        [TestMethod]
        public void Test_RequestForDeviceData()
        {
            AndroidDataCollectorClient client = new AndroidDataCollectorClient();

            var result = client.RequestForDeviceData("12").Result;
            
            Assert.IsFalse(false);
        }
        
        [TestMethod]
        public void Test_GetDeviceDataAsync()
        {
            AndroidDataCollectorClient client = new AndroidDataCollectorClient();

            var result = client.GetDeviceDataAsync("12").Result;
            
            Assert.IsFalse(false);
        }
    }
}