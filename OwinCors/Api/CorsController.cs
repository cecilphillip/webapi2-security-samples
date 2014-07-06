using System.Web.Http;

namespace OwinCors.Api
{
    [RoutePrefix("cors")]
    public class CorsController : ApiController
    {

        [HttpGet]
        [Route("data")]
        public IHttpActionResult GetData()
        {
            return Ok(new { message = "data from cors controller" });
        }
    }
}