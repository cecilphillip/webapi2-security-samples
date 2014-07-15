using System;
using System.Net.Http;
using Thinktecture.IdentityModel.Client;

namespace OAuthResourceOwnerClient
{
    class Program
    {
        static void Main(string[] args)
        {         
            var response = GetToken();
            CallService(response.AccessToken);
            Console.ReadLine();
        }

        private static TokenResponse GetToken()
        {
            var client = new OAuth2Client(new Uri("http://localhost:1642/token"));
            return client.RequestResourceOwnerPasswordAsync("cecil", "cecil").Result;
        }

        private static void CallService(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);
            var response = client.GetStringAsync(new Uri("http://localhost:1642/api/secure/data")).Result;

            Console.WriteLine(response);
        }
    }
}
