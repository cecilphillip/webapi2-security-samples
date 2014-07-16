using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace OAuthResourceServer.Providers
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // OAuth2 supports the notion of client authentication
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if(!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.Validated();

                return Task.FromResult(0);
            }

            //check your client info!!
            if(clientId == "angular")
            {
                context.Validated();
            }

            return Task.FromResult(0);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            if(context.ClientId != null)
            {
                var id = new ClaimsIdentity(context.Options.AuthenticationType);
                id.AddClaim(new Claim("sub", context.ClientId));
                id.AddClaim(new Claim("role", "user"));
                context.Validated(id);
            }

            return Task.FromResult(0);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // validate user credentials (demo!)
            // user credentials should be stored securely (salted, iterated, hashed yada)
            if(context.UserName != context.Password)
            {
                context.Rejected();
                return Task.FromResult(0);
            }

            // create identity
            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("sub", context.UserName));
            id.AddClaim(new Claim("role", "user"));

            context.Validated(id);
            return Task.FromResult(0);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //Called at the final stage of a successful Token endpoint request. do any final modification of the claims being used to issue access or refresh tokens
            return base.TokenEndpoint(context);
        }
    }
}