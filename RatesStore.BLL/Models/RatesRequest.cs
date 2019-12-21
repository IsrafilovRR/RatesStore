using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Models
{
    public class RatesRequest
    {
        [MinLength(3), MaxLength(3)]
        public string From { get; set; }

        [MinLength(3), MaxLength(3)]
        public string To { get; set; }
    }
}
