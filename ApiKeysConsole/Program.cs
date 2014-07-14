using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApiKeys;

namespace ApiKeysConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(2000);
            SendSignedRequest().Wait();
        }

        private async static Task SendSignedRequest()
        {
            var client = HttpClientFactory.Create(new ApiKeyClientHandler());
            var resp = await client.GetAsync("http://localhost.fiddler:2525/api/secure/data");
            Console.WriteLine(resp.StatusCode);
            Console.Read();
        }
    }

    public class ApiKeyClientHandler : DelegatingHandler
    {
        private const string API_KEY = "client-one-key";
        private const string API_KEY_HEADER = "X-API-KEY";
        private const string API_SIGNATURE_HEADER = "X-API-SIGNATURE";
        private const string API_TIMESTAMP_HEADER = "X-API-TIMESTAMP";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(API_KEY_HEADER, API_KEY);
            request.Headers.Host = "localhost:2525";
            var utcNow = DateTime.UtcNow;
          

            var timestamp = utcNow.ToUnixTime().ToString();
            request.Headers.Add(API_TIMESTAMP_HEADER, timestamp);
            
            request.Headers.Add(API_SIGNATURE_HEADER, GenerateSignature(request, timestamp));
            return base.SendAsync(request, cancellationToken);
        }

        private string GenerateSignature(HttpRequestMessage request, string timestamp)
        {
            var requestMethod = ((request.Method == HttpMethod.Get) ? "" :
                request.Content.Headers.ContentType.MediaType);
            
            var signatureText = request.Headers.Host+ "\n" +
                                request.RequestUri.AbsolutePath + "\n" +
                                requestMethod + "\n" +
                                timestamp;

            var apiSecret = ApiKeyStore.GetSecrectForKey(API_KEY);

            var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            var signatureBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(signatureText));
            var signatureStr = Convert.ToBase64String(signatureBytes);

            return signatureStr;
        }
    }
}