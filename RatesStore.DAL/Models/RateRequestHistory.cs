using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Models
{
    public class RateRequestHistory : BaseEntity
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Time { get; set; }
    }
}
