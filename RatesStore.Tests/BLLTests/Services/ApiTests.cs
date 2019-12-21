using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RatesStore.BLL.Services
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void ApiReturnsNotNulls()
        {
            var client = new OpenExchangeRateClient();
            var result = client.GetAll();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Error);
            Assert.IsNotNull(result.Base);
            Assert.IsNotNull(result.Rates);
        }

        [TestMethod]
        public void ApiThrowsCorrectException()
        {
            var client = new OpenExchangeRateClient();
            var result = client.GetAllForContcreteBase("RUB");
            Assert.IsTrue(result.Error);
        }
    }
}
