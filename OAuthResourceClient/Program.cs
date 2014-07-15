using System;
using System.Net.Http;
using Thinktecture.IdentityModel.Client;

namespace OAuthResourceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourceOwnerFlow();

            Console.ReadLine();
        }

        private static void ResourceOwnerFlow()
        {
            var oauthClient = new OAuth2Client(new Uri("http://localhost:1642/token"));
            var tokenResponse = oauthClient.RequestResourceOwnerPasswordAsync("cecil", "cecil").Result;
            
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = client.GetStringAsync(new Uri("http://localhost:1642/api/secure/data")).Result;

            Console.WriteLine(response);
        }
    }
}
