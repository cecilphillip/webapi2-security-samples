using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace WebApiClaimsAuthorization
{
    public class CustomAuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            // check access credentials here
            if (context.Principal.Identity.IsAuthenticated)
            {
                var resource = context.Resource.First();
                var action = context.Action.First();

                if (context.Principal.HasClaim(c => c.Type == resource.Value && c.Value == action.Value))
                {
                    return Ok();
                }
            }
            return Nok();
        }
    }
}