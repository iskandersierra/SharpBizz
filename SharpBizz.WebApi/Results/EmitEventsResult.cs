using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SharpBizz.WebApi.Serialization;

namespace SharpBizz.WebApi.Results
{
    public class EmitEventsResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly IEmmittedEventsSerializer _eventsSerializer;
        private readonly bool _areCreationEvents;
        private readonly object[] _events;

        public EmitEventsResult(HttpRequestMessage request, IEmmittedEventsSerializer eventsSerializer, bool areCreationEvents, object[] events)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (eventsSerializer == null) throw new ArgumentNullException("eventsSerializer");
            if (events == null) throw new ArgumentNullException("events");
            if (events.Any(e => e == null)) throw new ArgumentNullException("events");
            _request = request;
            _eventsSerializer = eventsSerializer;
            _areCreationEvents = areCreationEvents;
            _events = events.ToArray();
        }

        public bool AreCreationEvents
        {
            get { return _areCreationEvents; }
        }

        public IEnumerable<object> Events
        {
            get { return _events; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.RequestMessage = _request;
            response.Content = new EmittedEventsContent(_eventsSerializer, _areCreationEvents, _events);
            return response;
        }
    }
}