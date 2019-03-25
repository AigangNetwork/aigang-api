using System;
using Aigang.Platform.Domain.Insurance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Aigang.Platform.Utils;
using Aigang.Platform.Repository.InsuranceRepository;

namespace Playground
{
    [TestClass]
    public class Test_InsuranceRepository
    {
        [TestInitialize]
        public void TestInitialize()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            ConfigurationManager.Configuration = builder.Build();
        }
        
        
        [TestMethod]
        public void Test_UpdatePolicyAsync()
        {
            var repo = new InsuranceRepository();

            var policy = new Policy()
            {
                Id = "123",
                Status = PolicyStatus.Draft,
                ClaimProperties = "{\"WL\":\"100\",\"DB\":\"123\",\"SAMSUNG\":\"RU\",\"DA\":3,\"CL\":36,\"DC\":3000}",
                PayoutUtc = DateTime.Now
            };

            var r = repo.UpdatePolicyAsync(policy).Result;

            Assert.IsTrue(true);
        }
    }
}