using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SharpBizz.WebApi.Serialization;

namespace SharpBizz.WebApi.Results
{
    public class EmittedEventsContent : HttpContent
    {
        private readonly IEmmittedEventsSerializer _eventsSerializer;
        private readonly bool _areCreationEvents;
        private readonly object[] _events;
        private long? _precomputedLength;

        public EmittedEventsContent(IEmmittedEventsSerializer eventsSerializer, bool areCreationEvents, object[] events)
        {
            if (eventsSerializer == null) throw new ArgumentNullException("eventsSerializer");
            if (events == null) throw new ArgumentNullException("events");
            _eventsSerializer = eventsSerializer;
            _areCreationEvents = areCreationEvents;
            _events = events;

            Headers.ContentType = eventsSerializer.GetContentType();
        }

        public bool AreCreationEvents
        {
            get { return _areCreationEvents; }
        }

        public IEnumerable<object> Events
        {
            get { return _events.AsEnumerable(); }
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _precomputedLength = await _eventsSerializer.SerializeAsync(stream, _events);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = (_precomputedLength ?? (_precomputedLength = _eventsSerializer.SerializeAsync(Stream.Null, _events).Result).Value);
            return length >= 0;
        }
    }
}
