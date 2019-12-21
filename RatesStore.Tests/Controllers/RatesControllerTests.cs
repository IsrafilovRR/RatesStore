using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RatesStore.Controllers
{
    [TestClass]
    public class RatesControllerTests
    {
        [TestMethod]
        public void CheckResults()
        {
            string incorrectRateName = "Bitcoin";

            RatesController ratesController = new RatesController();
            ratesController.Request = new HttpRequestMessage();
            ratesController.Configuration = new HttpConfiguration();

            var result = ratesController.Get(incorrectRateName)
                .ExecuteAsync(new System.Threading.CancellationToken());
            result.Wait();

            Assert.AreEqual(result.Result.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
    }
}
