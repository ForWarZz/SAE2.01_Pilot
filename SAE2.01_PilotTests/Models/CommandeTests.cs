using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE2._01_Pilot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models.Tests
{
    [TestClass()]
    public class CommandeTests
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            try
            {
                DataAccess.Instance.SetupTestBDD();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}