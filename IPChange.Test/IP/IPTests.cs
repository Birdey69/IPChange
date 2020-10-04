using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPChange.Test.IP
{
    [TestClass]
    public class IPTests
    {
        [TestMethod]
        public void CheckIpIsValidFalseTest()
        {
            string[] invalid = new string[] { "192.1.a.b", "10.2.3.256" };
            foreach (var ip in invalid)
            {
                Assert.IsFalse(IPChange.IP.CheckIpIsValid(ip, StaticTypes.IP));
            }
        }

        [TestMethod]
        public void CheckIpIsValidTrueTest()
        {
            string[] valid = new string[] { "192.168.1.120", "10.10.10.10" };
            foreach (var ip in valid)
            {
                Assert.IsTrue(IPChange.IP.CheckIpIsValid(ip, StaticTypes.IP));
            }
        }
    }
}
