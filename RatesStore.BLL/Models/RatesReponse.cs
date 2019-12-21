using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Models
{
    public class RatesResponse
    {
        public string From { get; set; }
        public RateInfo[] Rates { get; set; }
    }
}
