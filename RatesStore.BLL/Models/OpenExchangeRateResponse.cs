using RatesStore.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Models
{
    public class OpenExchangeRateResponse
    {
        public string Disclaimer { get; set; }
        public string License { get; set; }        
        public string Base { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
