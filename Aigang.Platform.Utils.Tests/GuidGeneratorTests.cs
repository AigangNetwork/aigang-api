using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aigang.Platform.Utils.Tests
{
    [TestClass]
    public class GuidGeneratorTests
    {
        [TestMethod]
        public void TestGuidLength()
        {
            var guid = GuidGenerator.Generate();
            Assert.IsTrue(guid.Length == 32);
        }
    }
}
