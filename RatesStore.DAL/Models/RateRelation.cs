using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Models
{
    public class RateRelation : BaseEntity
    {
        [Index]
        public virtual Rate RateFrom { get; set; }
        [Index]
        public virtual Rate RateTo { get; set; }
        public decimal Cost { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
