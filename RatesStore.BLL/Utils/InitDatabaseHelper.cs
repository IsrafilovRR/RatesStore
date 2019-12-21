using RatesStore.BLL.Interfaces;
using RatesStore.BLL.Models;
using RatesStore.BLL.Services;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Util
{
    //it is needed to fill database with all pairs of rates
    //to create index for searching
    //if we are going to add rate pairs little by little, SQL needs to rebuild the tree of the indexes
    //indexes provide us high-performance select and update
    //starts manually

    public class InitDatabaseHelper
    {
        public static void InitRates()
        {
            var client = new OpenExchangeRateClient();
            ClearRatesTables();

            //getting all rates from API
            var rates = client.GetAll().Rates
                .Select(dictionaryElement => new Rate()
                {
                    Name = dictionaryElement.Key
                })
                .ToList();

            var ratesRelations = rates.SelectMany(fromRate =>
            {
                return rates.Select(toRate =>
                {
                    return new RateRelation()
                    {
                        ExpiresAt = DateTime.Now,
                        RateFrom = fromRate,
                        RateTo = toRate
                    };
                });
            });

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.Repository<RateRelation>().AddRange(ratesRelations);
                unitOfWork.Save();
            }
        }

        private static bool ClearRatesTables()
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Repository<Rate>().RemoveAll();
                    unitOfWork.Repository<RateRelation>().RemoveAll();
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
