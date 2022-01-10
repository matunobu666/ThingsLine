using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThingsLine.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingsLine.Modules.Tests
{
    [TestClass()]
    public class modUDeviceTests
    {
        [TestMethod()]
        public void GetUDeviceTest()
        {
            string test = null;
            Assert.IsNotNull (test);
        }

        [TestMethod()]
        public void GetDeviceModeTest()
        {
            string test = null;
            Assert.IsNull(test);
        }
    }
}