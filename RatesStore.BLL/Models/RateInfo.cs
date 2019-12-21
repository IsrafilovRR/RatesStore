using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Models
{
    public struct RateInfo
    {
        public string To { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
