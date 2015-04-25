using System.Web.Http;
using SharpBizz.WebApi.Results;
using SharpBizz.WebApi.Serialization;

namespace SharpBizz.WebApi
{
    public abstract class CommandController : ApiController
    {
        // Dependency
        public IEmmittedEventsSerializer EventSerializer { get; set; }

        public CommandController()
        {

        }

        protected EmitEventsResult EmitEvents(params object[] events)
        {
            return EmitEvents(areCreationEvents: false, events: events);
        }

        protected EmitEventsResult EmitEvents(bool areCreationEvents, params object[] events)
        {
            return new EmitEventsResult(Request, EventSerializer, areCreationEvents, events);
        }

        protected EmitEventsResult EmitCreationEvents(params object[] events)
        {
            return EmitEvents(areCreationEvents: true, events: events);
        }

        protected EmitEventsResult EmitNoEvents()
        {
            return EmitEvents(areCreationEvents: false, events: new object[0]);
        }
    }
}