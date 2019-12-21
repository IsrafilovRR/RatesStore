using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RatesStore.BLL.Models;
using RatesStore.BLL.Services;
using RatesStore.BLL.Utils;

namespace RatesStore.Controllers
{
    public class RatesController : ApiController
    {
        RatesService service = new RatesService();

        [Route("api/rates/{from}")]
        public IHttpActionResult Get(string from)
        {
            try
            {
                var result = service.GetRatesForRateBase(new RatesRequest() { From = from });
                return Ok(new { results = result });
            }
            catch(ArgumentException ex)
            {
                var mesage = string.Format("Incorrect arguments. Request: from - {0}", from);
                DbLogger.LogError(ex, mesage);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                var message = string.Format("The request message was already sent by the HttpClient instance. Request: from - {0}", from);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (HttpRequestException ex)
            {
                var message = string.Format(@"The request failed due to an underlying issue such as network connectivity,
                    DNS failure, server certificate validation or timeout. Request: from - {0}", from);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (TaskCanceledException ex)
            {
                var message = string.Format("The request timed-out or the user canceled the request's Task. Request: from - {0}", from);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                DbLogger.LogError(ex, "Undefined exception");
                return InternalServerError(ex);
            }
        }

        [Route("api/rates/{from}/{to}")]
        public IHttpActionResult Get([FromUri] RatesRequest request)
        {
            try
            {
                var result = service.GetRatesForRateBase(new RatesRequest() { From = request.From, To = request.To });
                return Ok(new { results = result });
            }
            catch (ArgumentException ex)
            {
                DbLogger.LogError(ex, string.Format("Incorrect arguments. Request: from - {0}, to - {1}", request.From, request.To));
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                var message = string.Format(@"The request message was already sent by the HttpClient instance. 
                    Request: from - {0}, to - {1}", request.From, request.To);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (HttpRequestException ex)
            {
                var message = string.Format(@"The request failed due to an underlying issue such as network connectivity,
                    DNS failure, server certificate validation or timeout. Request: from - {0}", request.From, request.To);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (TaskCanceledException ex)
            {
                var message = string.Format(@"The request timed-out or the user canceled the request's Task.
                    Request: from - {0}", request.From, request.To);
                DbLogger.LogError(ex, message);
                return InternalServerError(ex);
            }
            catch (Exception ex)
            {
                DbLogger.LogError(ex, "Undefined exception");
                return InternalServerError(ex);
            }
        }
    }
}
