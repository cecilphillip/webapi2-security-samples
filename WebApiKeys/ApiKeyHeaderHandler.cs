using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiKeys
{
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        private const string API_KEY_HEADER = "X-API-KEY";
        private const string API_SIGNATURE_HEADER = "X-API-SIGNATURE";
        private const string API_TIMESTAMP_HEADER = "X-API-TIMESTAMP";
        private const int API_TIMESTAMP_WITHIN_SECONDS = 15;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiHeaders = new[] { API_TIMESTAMP_HEADER, API_SIGNATURE_HEADER, API_KEY_HEADER };

            IEnumerable<string> headerKeys = request.Headers
                                             .Where(h => h.Value.Any() && !string.IsNullOrEmpty(h.Value.First()))
                                             .Select(h => h.Key);

            if (!apiHeaders.Except(headerKeys).Any())
            {
                string apiTimestamp = request.Headers.First(h => h.Key == API_TIMESTAMP_HEADER).Value.FirstOrDefault();
                DateTime timestampValue = long.Parse(apiTimestamp).UnixTimeToDateTime();

                DateTime serverTime = DateTime.UtcNow;
                if (serverTime.Subtract(TimeSpan.FromSeconds(API_TIMESTAMP_WITHIN_SECONDS)) <= timestampValue && timestampValue <= serverTime)
                {
                    string generatedSignature = GenerateSignature(request);
                    string retrievedSignature = request.Headers.First(h => h.Key == API_SIGNATURE_HEADER).Value.FirstOrDefault();

                    if (!string.IsNullOrEmpty(generatedSignature) && generatedSignature.Equals(retrievedSignature))
                    {
                        return base.SendAsync(request, cancellationToken);
                    }
                }
            }

            var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("API Key Credentials Invalid"));
            return Task.FromResult(response);
        }

        private string GenerateSignature(HttpRequestMessage request)
        {
            // METHOD URI TYPE TIME
            var apiKeyHeader = request.Headers.First(h => h.Key == API_KEY_HEADER).Value.FirstOrDefault();

            var apiTimestamp = request.Headers.First(h => h.Key == API_TIMESTAMP_HEADER).Value.FirstOrDefault();

            var apiSecret = ApiKeyStore.GetSecrectForKey(apiKeyHeader);

            var requestMethod = ((request.Method == HttpMethod.Get) ? "" : request.Content.Headers.ContentType.MediaType);

            var signatureText = request.Headers.Host + "\n" +

                                request.RequestUri.AbsolutePath + "\n" +
                                requestMethod + "\n" +
                                apiTimestamp;

            var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            var signatureBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(signatureText));
            var signatureStr = Convert.ToBase64String(signatureBytes);

            return signatureStr;
        }
    }
}