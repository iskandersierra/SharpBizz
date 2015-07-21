using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBizz.Http.AsyncHttp
{
    public interface IMessageSender
    {
        Task SendMessageAsync(Func<Stream, Task> writeContent, CancellationToken cancellationToken);
    }
}
