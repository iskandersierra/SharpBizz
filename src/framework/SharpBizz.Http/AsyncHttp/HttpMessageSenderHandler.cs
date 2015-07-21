using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBizz.Http.AsyncHttp
{
    public class HttpMessageSenderHandler : HttpMessageHandler
    {
        private IMessageSender _messageSender;
        private IHttpMessageSerializer _messageSerializer;

        public HttpMessageSenderHandler(IMessageSender messageSender) 
            : this(messageSender, HttpMessageSerializer.Default)
        {
        }

        public HttpMessageSenderHandler(IMessageSender messageSender, IHttpMessageSerializer messageSerializer)
        {
            if (messageSender == null) throw new ArgumentNullException("messageSender");
            if (messageSerializer == null) throw new ArgumentNullException("messageSerializer");

            _messageSerializer = messageSerializer;
            _messageSender = messageSender;
        }

        protected IHttpMessageSerializer MessageSerializer
        {
            get { return _messageSerializer; }
        }

        protected IMessageSender MessageSender
        {
            get { return _messageSender; }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await MessageSender.SendMessageAsync(
                    async (stream) => await MessageSerializer.WriteRequestAsync(request, stream, cancellationToken),
                    cancellationToken);

                var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}
