using RatesStore.BLL.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Init
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Database initialization is started...");
            InitDatabaseHelper.InitRates();
            Console.WriteLine("Database initialization is ended...");
        }
    }
}
