namespace GEMC.ServerHost.Controllers
{
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("api")]
    public class MessageController : ApiController
    {
        [Route("info")]
        [HttpGet]
        public IHttpActionResult GetApiInfo()
        {
            return this.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
