using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBizz.Http
{
    public interface IHttpMessageSerializer
    {
        Task WriteRequestAsync(HttpRequestMessage request, Stream stream, CancellationToken cancellationToken);
        Task<byte[]> WriteRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        Task WriteResponseAsync(HttpResponseMessage response, Stream stream, CancellationToken cancellationToken);
        Task<byte[]> WriteResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken);

        Task<HttpRequestMessage> ReadRequestAsync(Stream stream, CancellationToken cancellationToken);
        Task<HttpRequestMessage> ReadRequestAsync(byte[] data, CancellationToken cancellationToken);
        Task<HttpResponseMessage> ReadResponseAsync(Stream stream, CancellationToken cancellationToken);
        Task<HttpResponseMessage> ReadResponseAsync(byte[] data, CancellationToken cancellationToken);

    }
}
