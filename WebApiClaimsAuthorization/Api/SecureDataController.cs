using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApiClaimsAuthorization.Api
{
    [RoutePrefix("secure")]
    [Authorize]
    public class SecureDataController : ApiController
    {
        [Route("data")]
        [HttpGet]
        [ResourceAuthorize("Read", "Data", "Inventory", "Records")]
        public IHttpActionResult GetData()
        {
            var user = this.User.Identity as ClaimsIdentity;
            return Ok(new
            {
                UserName = user.Name,
                IsAuthenticated = user.IsAuthenticated,
                Claims = user.Claims.Select(c => new { Value = c.Value, Type = c.Type }),
                Date = DateTime.Now.TimeOfDay
            });
        }
    }
}
