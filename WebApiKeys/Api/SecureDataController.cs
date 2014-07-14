using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace WebApiKeys.Api
{
    [RoutePrefix("secure")]
    public class SecureDataController : ApiController
    {
        [Route("data")]
        [HttpGet]
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
