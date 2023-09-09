using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Runtime.Caching;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Day20.Controllers
{
    public class EcpayController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ecpay/AddPayInfo")]
        public HttpResponseMessage AddPayInfo(JObject info)
        {
            try
            {
                var cache = MemoryCache.Default;
                cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Ecpay/AddAccountInfo")]
        public HttpResponseMessage AddAccountInfo(JObject info)
        {
            try
            {
                var cache = MemoryCache.Default;
                cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }
        private HttpResponseMessage ResponseError()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("0|Error");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
        private HttpResponseMessage ResponseOK()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("1|OK");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}