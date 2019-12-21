using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatesStore.BLL.Services;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RatesStore.BLL.Services
{
    [TestClass()]
    public class RatesServiceTests
    {
        [TestMethod()]
        public void GetRatesForRateBaseTest()
        {
            string from = "USD";

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var requestsHistoryCount = unitOfWork.Repository<RateRequestHistory>().GetCount();

                RatesService service = new RatesService();
                var result = service.GetRatesForRateBase(new Models.RatesRequest()
                {
                    From = from
                });

                //wait for all async updates
                Thread.Sleep(10000);
                
                Assert.IsTrue(requestsHistoryCount < unitOfWork.Repository<RateRequestHistory>().GetCount());
                Assert.IsTrue(result.From == from);
                Assert.IsNotNull(result.Rates);
            }
        }

        [TestMethod()]
        public void GetRateForRateBaseTest()
        {
            string from = "USD";
            string to = "RUB";

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var requestsHistoryCount = unitOfWork.Repository<RateRequestHistory>().GetCount();

                RatesService service = new RatesService();
                var result = service.GetRatesForRateBase(new Models.RatesRequest()
                {
                    From = from,
                    To = to
                });
                //wait for all async updates
                Thread.Sleep(10000);
                Assert.IsTrue(requestsHistoryCount < unitOfWork.Repository<RateRequestHistory>().GetCount());
                Assert.IsNotNull(result.Rates);
                Assert.AreEqual(result.Rates.Count(), 1);
                Assert.IsTrue(result.Rates[0].ExpireAt > DateTime.Now);
                Assert.AreEqual(result.Rates[0].To, to);
            }
        }

        
    }
}