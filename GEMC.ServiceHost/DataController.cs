using System.Reflection;
using System.Web.Http;
using GEMC.Common;

namespace GEMC.ServiceHost
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [ActionName("info")]
        [HttpGet]
        public IHttpActionResult GetApiInfo()
        {
            return this.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        [ActionName("message")]
        [HttpGet]
        public IHttpActionResult GetMessage()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();
            
            return this.Ok(container.GetMessage());
        }

        [ActionName("metadata")]
        [HttpGet]
        public IHttpActionResult GetMetadata()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Event.Metadata);
        }

        [ActionName("remaining")]
        [HttpGet]
        public IHttpActionResult GetRemainingTime()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Event.Metadata.RemainingTime);
        }

        [ActionName("racestatus")]
        [HttpGet]
        public IHttpActionResult GetRaceStatus()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetMessage().Status.ToString());
        }

        [ActionName("raceresult")]
        [HttpGet]
        public IHttpActionResult GetPreviousRaceData()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetPreviousRaceMessage());
        }

        [ActionName("nextracedata")]
        [HttpGet]
        public IHttpActionResult GetNextRaceData()
        {
            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            return this.Ok(container.GetPreviousRaceMessage());
        }
    }
}
