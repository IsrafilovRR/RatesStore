using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Models
{
    public class Log : BaseEntity
    {
        public string Message { get; set; }
        public LogType Type { get; set; }
        public DateTime LogTime { get; set; }
    }
}

public enum LogType
{
    Log,
    Error
}
