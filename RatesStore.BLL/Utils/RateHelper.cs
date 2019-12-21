using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RatesStore.BLL.Models;
using RatesStore.BLL.Services;

namespace RatesStore.BLL.Utils
{
    internal class RateHelper
    {
        internal static void PrepareRequest(RatesRequest request)
        {
            if (!string.IsNullOrEmpty(request.From))
                request.From = request.From.ToUpper();
            if (!string.IsNullOrEmpty(request.To))
                request.To = request.To.ToUpper();
        }

        internal static void CheckCorrectnessOfRequest(RatesRequest request)
        {
            if (string.IsNullOrEmpty(request.From))
                throw new ArgumentException();
        }

        internal static RateInfo[] GetRatesForBaseRateFromApi(string neededBaseRate)
        {
            var client = new OpenExchangeRateClient();
            var result = client.GetAll();

            if (result.Error)
                throw new InvalidOperationException(result.Message);

            var ratesRelativeBaseRate = result.Rates;

            if (!ratesRelativeBaseRate.ContainsKey(neededBaseRate))
                return null;

            return GetCalculatedRatesRegardingBaseRate(neededBaseRate,
                ratesRelativeBaseRate, DateTime.Now.AddMinutes(Config.ValidPeriodMinutes));
        }

        //second argument is needed to check if it exists in 
        internal static RateInfo[] GetRatesForBaseRateFromApi(string neededBaseRate, string rateTo)
        {
            var client = new OpenExchangeRateClient();
            var result = client.GetAll();

            if (result.Error)
                throw new InvalidOperationException(result.Message);

            var ratesRelativeBaseRate = result.Rates;

            if (!ratesRelativeBaseRate.ContainsKey(neededBaseRate) || !ratesRelativeBaseRate.ContainsKey(rateTo))
                return null;

            return GetCalculatedRatesRegardingBaseRate(neededBaseRate,
                ratesRelativeBaseRate, DateTime.Now.AddMinutes(Config.ValidPeriodMinutes));
        }

        internal static RateInfo[] GetCalculatedRatesRegardingBaseRate(string baseRateNeeded, 
            Dictionary<string, decimal> clientRatesRelativeBaseClientRate, DateTime expiredDate)
        { 
            var newBaseRate = clientRatesRelativeBaseClientRate[baseRateNeeded];
            return clientRatesRelativeBaseClientRate
                .Select(rate => new RateInfo()
                {
                    ExpireAt = expiredDate,
                    To = rate.Key,
                    Rate = Decimal.Round(rate.Value / newBaseRate, Config.RateRoundDecimals)
                })
                .ToArray();            
        }
    }
}
