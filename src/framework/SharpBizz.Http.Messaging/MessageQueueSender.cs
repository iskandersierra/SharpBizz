using System;
using System.IO;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using SharpBizz.Http.AsyncHttp;

namespace SharpBizz.Http.Messaging
{
    public class MessageQueueSender : IMessageSender, IDisposable
    {
        private string _queueName;
        private MessageQueue _queue;
        private bool _isQueueTransactional;
        private readonly bool _ownsQueue;
        private bool _isDisposed;

        public MessageQueueSender(MessageQueue queue, bool isQueueTransactional, bool ownsQueue = true)
        {
            if (queue == null) throw new ArgumentNullException("queue");
            _queueName = _queue.FormatName;
            _queue = queue;
            _isQueueTransactional = isQueueTransactional;
            _ownsQueue = ownsQueue;
        }

        public MessageQueueSender(MessageQueue queue)
            : this(queue, queue.Transactional)
        {
        }

        public async Task SendMessageAsync(Func<Stream, Task> writeContent, CancellationToken cancellationToken)
        {
            if (writeContent == null) throw new ArgumentNullException("writeContent");
            CheckIsDisposed();

            var message = new Message();
            var stream = new MemoryStream();
            await writeContent(stream);
            message.BodyStream = stream;
            message.Recoverable = true;
            ConfigureMessage(message);

            if (!_isQueueTransactional)
                _queue.Send(message);
            else
                _queue.Send(message, MessageQueueTransactionType.Single);
        }

        protected virtual void ConfigureMessage(Message message)
        {
        }

        private void CheckIsDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(_queueName, "Message queue sender is already disposed");
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_ownsQueue)
                    _queue.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
