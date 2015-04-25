using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SharpBizz.WebApi.Serialization
{
    public interface IEmmittedEventsSerializer
    {
        Task<long> SerializeAsync(Stream stream, object[] events);
        MediaTypeHeaderValue GetContentType();
    }
}