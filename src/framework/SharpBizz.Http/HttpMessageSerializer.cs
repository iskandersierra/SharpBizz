using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBizz.Http
{
    public class HttpMessageSerializer : IHttpMessageSerializer
    {
        public static readonly HttpMessageSerializer Default = new HttpMessageSerializer();

        public Task WriteRequestAsync(HttpRequestMessage request, Stream stream, CancellationToken cancellationToken)
        {
            return HttpUtils.WriteRequestAsync(request, stream, cancellationToken);
        }

        public Task<byte[]> WriteRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return HttpUtils.WriteRequestAsync(request, cancellationToken);
        }

        public Task WriteResponseAsync(HttpResponseMessage response, Stream stream, CancellationToken cancellationToken)
        {
            return HttpUtils.WriteResponseAsync(response, stream, cancellationToken);
        }

        public Task<byte[]> WriteResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return HttpUtils.WriteResponseAsync(response, cancellationToken);
        }

        public Task<HttpRequestMessage> ReadRequestAsync(Stream stream, CancellationToken cancellationToken)
        {
            return HttpUtils.ReadRequestAsync(stream, cancellationToken);
        }

        public Task<HttpRequestMessage> ReadRequestAsync(byte[] data, CancellationToken cancellationToken)
        {
            return HttpUtils.ReadRequestAsync(data, cancellationToken);
        }

        public Task<HttpResponseMessage> ReadResponseAsync(Stream stream, CancellationToken cancellationToken)
        {
            return HttpUtils.ReadResponseAsync(stream, cancellationToken);
        }

        public Task<HttpResponseMessage> ReadResponseAsync(byte[] data, CancellationToken cancellationToken)
        {
            return HttpUtils.ReadResponseAsync(data, cancellationToken);
        }
    }
}