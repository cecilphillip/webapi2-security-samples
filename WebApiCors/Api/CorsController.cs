using System.Web.Http;

namespace WebApiCors.Api
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