using RatesStore.BLL.Models;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Utils
{
    internal class DatabaseWriter
    {
        internal static void UpdateRatesRelation(RateInfo[] rateInfoArray, string fromRateString)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var baseRate = unitOfWork.Repository<Rate>().Get(rate => rate.Name == fromRateString).First();

                var rateRelations = unitOfWork.Repository<RateRelation>()
                    .Get(rateRelation => rateRelation.RateFrom == baseRate);

                foreach (var rateRelation in rateRelations)
                { 
                    var rateInfo = rateInfoArray.Where(info => info.To == rateRelation.RateTo.Name).FirstOrDefault();
                    rateRelation.Cost = rateInfo.Rate;
                    rateRelation.ExpiresAt = rateInfo.ExpireAt;
                }

                unitOfWork.Save();
            }
        }

        //adds all relation pairs to recreate index structure
        internal static void CreateRatesRelation(RateInfo[] rateInfoArray, string fromRateString)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var ratesFromApi = rateInfoArray.Select(r => r.To);
                var currentRates = unitOfWork.Repository<Rate>().GetAll()
                    .Select(rate => rate.Name)
                    .ToList();

                var ratesShouldBeAdded = ratesFromApi.Where(r => !currentRates.Contains(r));

                foreach (var newRateName in ratesShouldBeAdded)
                {
                    var baseRate = new Rate() { Name = newRateName };
                    unitOfWork.Repository<Rate>().Add(baseRate);
                }
                unitOfWork.Save();

                List<RateRelation> rateRelations = new List<RateRelation>();
                foreach (var newRateName in ratesShouldBeAdded)
                {
                    var baseRate = unitOfWork.Repository<Rate>().Get(r=>r.Name==newRateName).First();
                    foreach (var rateInfo in ratesFromApi)
                    {
                        if (newRateName == rateInfo)
                            continue;
                        rateRelations.Add(new RateRelation()
                        {
                            RateFrom = baseRate,
                            Cost = 0,
                            ExpiresAt = DateTime.Now,
                            RateTo = unitOfWork.Repository<Rate>().Get(rate => rate.Name == rateInfo).First()
                        }) ;

                        rateRelations.Add(new RateRelation()
                        {
                            RateFrom = unitOfWork.Repository<Rate>().Get(rate => rate.Name == rateInfo).First(),
                            RateTo = baseRate,
                            Cost = 0,
                            ExpiresAt = DateTime.Now,
                        });
                    }
                }
                unitOfWork.Repository<RateRelation>().AddRange(rateRelations);
                unitOfWork.Save();
            }

            UpdateRatesRelation(rateInfoArray, fromRateString);
        }
        internal static void LogRequest(RatesRequest request)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.Repository<RateRequestHistory>().Add(new RateRequestHistory()
                {
                    Time = DateTime.Now,
                    From = request.From,
                    To = request.To
                });
                unitOfWork.Save();
            }
        }


        internal static void CreateRatesRelationAsync(RateInfo[] rateInfoArray, string fromRateString)
        {
            Task.Run(() =>
            {
                CreateRatesRelation(rateInfoArray, fromRateString);
            });
        }
        internal static void UpdateRatesRelationAsync(RateInfo[] rateInfoArray, string fromRateString)
        {
            Task.Run(() =>
            {
                UpdateRatesRelation(rateInfoArray, fromRateString);
            });
        }
        internal static void LogRequestAsync(RatesRequest request)
        {
            Task.Run(() =>
            {
                LogRequest(request);
            });
        }
    }
}
