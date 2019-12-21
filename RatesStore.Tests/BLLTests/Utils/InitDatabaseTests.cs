using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatesStore.BLL.Util;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Utils
{
    [TestClass()]
    public class InitDatabaseTests
    {
        [TestMethod()]
        public void InitRatesTest()
        {
            InitDatabaseHelper.InitRates();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var ratesCount = unitOfWork.Repository<Rate>().GetCount();
                var ratesRelationsCount = unitOfWork.Repository<RateRelation>().GetCount();

                Assert.IsTrue(ratesCount > 0);
                Assert.IsTrue(ratesRelationsCount > 0);
                Assert.IsTrue(ratesCount * ratesCount == ratesRelationsCount);
            }
        }
    }
}