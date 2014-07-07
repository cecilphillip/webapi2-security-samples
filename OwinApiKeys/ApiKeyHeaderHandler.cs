using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OwinApiKeys
{
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        private const string API_KEY_HEADER = "X-API-KEY";
        private const string API_SIGNATURE_HEADER = "X-API-SIGNATURE";
        private const string API_TIMESTAMP_HEADER = "X-API-TIMESTAMP";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiHeaders = new [] { API_TIMESTAMP_HEADER, API_SIGNATURE_HEADER, API_KEY_HEADER };

            IEnumerable<string> headerKeys = request.Headers.Select(h => h.Key);
           
            if (!apiHeaders.Except(headerKeys)  .Any())
            {
                var apiKeyHeader = request.Headers.FirstOrDefault(h => h.Key == API_KEY_HEADER);
                var apiSignature = request.Headers.FirstOrDefault(h => h.Key == API_SIGNATURE_HEADER);
                var apiTimestamp = request.Headers.FirstOrDefault(h => h.Key == API_TIMESTAMP_HEADER);
            }
           


            var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("API Key Credentials Invalid"));

            return Task.FromResult(response);
        }
    }
}