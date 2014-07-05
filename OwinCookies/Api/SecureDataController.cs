using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace OwinCookies.Api
{
    [RoutePrefix("api/secure")]
    [Authorize]
    public class SecureDataController : ApiController
    {
        [Route("data")]
        [HttpGet]
        public IHttpActionResult GetData()
        {
            var user = this.User.Identity;
            return Ok(new
            {
                UserName = user.Name,
                IsAuthenticated = user.IsAuthenticated,
                Claims = (user as ClaimsIdentity).Claims.Select(c=> new{ Value = c.Value, Type = c.Type}),
                Date = DateTime.Now.TimeOfDay
            });
        }
    }
}
