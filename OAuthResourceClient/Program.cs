using System;
using System.Net.Http;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace OAuthResourceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = ClientCredentialFlow();

            Task.WaitAll(task);

            Console.ReadLine();
        }

        private async static Task ClientCredentialFlow()
        {
            try
            {
                var oauthClient = new OAuth2Client(new Uri("http://localhost:1642/token"), "angular", "secret");
                var tokenResponse = oauthClient.RequestClientCredentialsAsync("read").Result;

                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);

                var response = await client.GetStringAsync(new Uri("http://localhost:1642/api/secure/data"));
                Console.WriteLine(response);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async static Task ResourceOwnerFlow()
        {
            var oauthClient = new OAuth2Client(new Uri("http://localhost:1642/token"));
            var tokenResponse = oauthClient.RequestResourceOwnerPasswordAsync("cecil", "cecil").Result;

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetStringAsync(new Uri("http://localhost:1642/api/secure/data"));

            Console.WriteLine(response);
        }
    }
}
