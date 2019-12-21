using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RatesStore.BLL.Interfaces;
using RatesStore.BLL.Models;
using RatesStore.BLL.Util;
using RatesStore.BLL.Utils;
using RatesStore.DAL.Models;
using RatesStore.DAL.Repository;

namespace RatesStore.BLL.Services
{
    public class RatesService
    {
        public RatesResponse GetRatesForRateBase(RatesRequest request)
        {
            RateHelper.PrepareRequest(request);
            RateHelper.CheckCorrectnessOfRequest(request);

            // async log request history
            DatabaseWriter.LogRequestAsync(request);

            if (string.IsNullOrEmpty(request.To))
                return GetAllRatesForRateBase(request.From);

            return GetConreteRateForRateBase(request.From, request.To);
        }

        #region private methods

        //return all pairs of rates, if TO is empty
        private RatesResponse GetAllRatesForRateBase(string baseRate)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var rateRelations = unitOfWork.Repository<RateRelation>()
                    .Get(rateRelation => rateRelation.RateFrom.Name == baseRate);

                //check if rate doesn't contain in the DB, but contains in API
                //if doesn't contain in API - throw exception
                if (rateRelations.Count() == 0)
                {
                    //check api
                    var calculatedRates = RateHelper.GetRatesForBaseRateFromApi(baseRate);
                    if (calculatedRates == null)
                        throw new ArgumentNullException(baseRate);

                    //async update relations in db
                    DatabaseWriter.CreateRatesRelationAsync(calculatedRates, baseRate);

                    return new RatesResponse()
                    {
                        From = baseRate,
                        Rates = calculatedRates
                    };
                }

                //then this rate is in DB,
                //check if all relations are not expired then return from db
                if (!rateRelations.Any(relation => relation.ExpiresAt <= DateTime.Now))
                    return new RatesResponse()
                    {
                        From = baseRate,
                        Rates = rateRelations
                                .Select(relation => new RateInfo()
                                {
                                    ExpireAt = relation.ExpiresAt,
                                    Rate = relation.Cost,
                                    To = relation.RateTo.Name
                                })
                                .ToArray()
                    };

                //if someone is expired, get from api
                var newCalculatedRates = RateHelper.GetRatesForBaseRateFromApi(baseRate);

                //async update relations in db
                DatabaseWriter.UpdateRatesRelationAsync(newCalculatedRates, baseRate);
                
                return new RatesResponse()
                {
                    From = baseRate,
                    Rates = newCalculatedRates
                };
            }
        }

        //return only one pair of rates, if TO is not empty
        private RatesResponse GetConreteRateForRateBase(string baseRate, string toRate)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var rateRelation = unitOfWork.Repository<RateRelation>()
                    .Get(relation =>
                    relation.RateFrom.Name == baseRate &&
                    relation.RateTo.Name == toRate)
                    .FirstOrDefault();

                //if pair is not found in the DB
                //means that both can be invalid or new
                if (rateRelation == null)
                {
                    //check api to be sure that toRate is new
                    //if not throw exception
                    var calculatedRates = RateHelper.GetRatesForBaseRateFromApi(baseRate, toRate);
                    if (calculatedRates == null)
                        throw new ArgumentNullException("FromRate or ToRate");

                    //async update relations in db
                    DatabaseWriter.CreateRatesRelationAsync(calculatedRates, baseRate);

                    return new RatesResponse()
                    {
                        From = baseRate,
                        Rates = calculatedRates.Where(rateInfo => rateInfo.To == toRate).ToArray()
                    };
                }

                //pair is found and non expired
                if (rateRelation.ExpiresAt > DateTime.Now)
                    return new RatesResponse()
                    {
                        From = baseRate,
                        Rates = new RateInfo[1]{
                            new RateInfo()
                            {
                                 ExpireAt = rateRelation.ExpiresAt,
                                 Rate = rateRelation.Cost,
                                 To  = toRate
                            }
                        }
                    };

                //if rateRelation is expired, get from api
                var newCalculatedRates = RateHelper.GetRatesForBaseRateFromApi(baseRate);

                //call async method to update relations in DB
                DatabaseWriter.UpdateRatesRelationAsync(newCalculatedRates, baseRate);

                //return response during DB updating
                return new RatesResponse()
                {
                    From = baseRate,
                    Rates = newCalculatedRates.Where(rateInfo => rateInfo.To == toRate).ToArray()
                };
            }
        }        
        #endregion
    }
}
