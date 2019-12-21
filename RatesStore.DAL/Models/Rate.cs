using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Models
{
    public class Rate : BaseEntity
    {
        [MinLength(3) , MaxLength(3), Index]
        public string Name { get; set; }
    }
}
