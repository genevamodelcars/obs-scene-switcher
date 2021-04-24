namespace GEMC.ServerHost.Controllers
{
    using System.Reflection;
    using System.Web.Http;
    using GEMC.Common;

    [RoutePrefix("api")]
    public class MessageController : ApiController
    {
        [Route("info")]
        [HttpGet]
        public IHttpActionResult GetApiInfo()
        {
            return this.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        [Route("message")]
        [HttpGet]
        public IHttpActionResult GetMessage()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();
            
            return this.Ok(container.GetMessage());
        }

        [Route("metadata")]
        [HttpGet]
        public IHttpActionResult GetMetadata()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Event.Metadata);
        }

        [Route("remaining")]
        [HttpGet]
        public IHttpActionResult GetRemainingTime()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Event.Metadata.RemainingTime);
        }

        [Route("racestatus")]
        [HttpGet]
        public IHttpActionResult GetRaceStatus()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Status);
        }

        [Route("raceresult")]
        [HttpGet]
        public IHttpActionResult GetRaceResult()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetRaceResultMessage());
        }
    }
}
