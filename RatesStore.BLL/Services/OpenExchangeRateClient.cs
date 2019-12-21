using RatesStore.BLL.Interfaces;
using RatesStore.BLL.Models;
using RatesStore.BLL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Services
{
    public class OpenExchangeRateClient : IRateClient<OpenExchangeRateResponse>
    {
        public OpenExchangeRateResponse GetAll()
        {
            var url = $"{Config.ApiUrl}/{Config.ApiMethod}?app_id={Config.ApiKey}";
            return HttpHelper.Get<OpenExchangeRateResponse>(url);
        }
        
        public OpenExchangeRateResponse GetAllForContcreteBase(string baseRate)
        {
            var url = $"{Config.ApiUrl}/{Config.ApiMethod}?app_id={Config.ApiKey}&base={baseRate}";
            return HttpHelper.Get<OpenExchangeRateResponse>(url);
        }        
    }
}