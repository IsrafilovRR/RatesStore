using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Utils
{
    public static class Config
    {
        public static string ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiUrl"];
            }
        }

        public static string ApiMethod
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiMethod"];
            }
        }

        public static string ApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiKey"];
            }
        }

        public static int RateRoundDecimals
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["RateRoundDecimals"]);
            }
        }
        public static int ValidPeriodMinutes
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ValidPeriodMinutes"]);
            }
        }
        
    }

}
