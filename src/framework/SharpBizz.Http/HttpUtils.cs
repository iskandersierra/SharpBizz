using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpMachine;

namespace SharpBizz.Http
{
    public static class HttpUtils
    {
        private static readonly Encoding Ascii = Encoding.ASCII;
        private static readonly HashSet<string> SpaceSeparatedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "User-Agent",
            "Server",
        };

        public static Task WriteRequestAsync(this HttpRequestMessage request, Stream stream)
        {
            return WriteRequestAsync(request, stream, CancellationToken.None);
        }

        public static async Task WriteRequestAsync(this HttpRequestMessage request, Stream stream, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (stream == null) throw new ArgumentNullException("stream");

            // Write first line
            await WriteAsciiAsync(stream, string.Format(@"{0} {1} HTTP/{2}.{3}
", request.Method.Method, Uri.EscapeUriString(request.RequestUri.ToString()), request.Version.Major, request.Version.Minor), cancellationToken);

            // Write headers
            await WriteHeadersAsync(stream, request.Headers, request.Content, cancellationToken);

            // Write content
            await WriteContentAsync(stream, request.Content, cancellationToken);
        }

        public static Task<byte[]> WriteRequestAsync(this HttpRequestMessage request)
        {
            return WriteRequestAsync(request, CancellationToken.None);
        }

        public static async Task<byte[]> WriteRequestAsync(this HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException("request");
            var memStream = new MemoryStream();
            await WriteRequestAsync(request, memStream, cancellationToken);
            var arr = memStream.ToArray();
            return arr;
        }

        public static Task WriteResponseAsync(this HttpResponseMessage response, Stream stream)
        {
            return WriteResponseAsync(response, stream, CancellationToken.None);
        }

        public static async Task WriteResponseAsync(this HttpResponseMessage response, Stream stream, CancellationToken cancellationToken)
        {
            if (response == null) throw new ArgumentNullException("response");
            if (stream == null) throw new ArgumentNullException("stream");

            // Write first line
            await WriteAsciiAsync(stream, string.Format(@"HTTP/{2}.{3} {0} {1}
", (int)response.StatusCode, response.ReasonPhrase, response.Version.Major, response.Version.Minor), cancellationToken);

            // Write headers
            await WriteHeadersAsync(stream, response.Headers, response.Content, cancellationToken);

            // Write content
            await WriteContentAsync(stream, response.Content, cancellationToken);
        }

        public static Task<byte[]> WriteResponseAsync(this HttpResponseMessage response)
        {
            return WriteResponseAsync(response, CancellationToken.None);
        }

        public static async Task<byte[]> WriteResponseAsync(this HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response == null) throw new ArgumentNullException("response");
            var memStream = new MemoryStream();
            await WriteResponseAsync(response, memStream, cancellationToken);
            var arr = memStream.ToArray();
            return arr;
        }


        public static Task<HttpRequestMessage> ReadRequestAsync(Stream stream)
        {
            return ReadRequestAsync(stream, CancellationToken.None);
        }

        public static async Task<HttpRequestMessage> ReadRequestAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            var handler = new HttpRequestParserDelegate();
            var parser = new HttpParser(handler);

            const int bufferSize = 8 * 1024;
            var buffer = new byte[bufferSize];
            int readCount;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                readCount = await stream.ReadAsync(buffer, 0, bufferSize);
                if (readCount <= 0)
                    break;
                var segment = new ArraySegment<byte>(buffer, 0, readCount);
                parser.Execute(segment);
            } while (readCount > 0 && !handler.HasErrors);

            if (handler.HasErrors)
            {
                var exceptions = handler.Errors.Select(e => new FormatException(e)).ToArray<Exception>();
                if (exceptions.Length == 1)
                    throw exceptions[0];
                throw new AggregateException(exceptions);
            }

            return handler.Request;
        }

        public static Task<HttpRequestMessage> ReadRequestAsync(byte[] data)
        {
            return ReadRequestAsync(data, CancellationToken.None);
        }

        public static Task<HttpRequestMessage> ReadRequestAsync(byte[] data, CancellationToken cancellationToken)
        {
            if (data == null) throw new ArgumentNullException("data");
            return ReadRequestAsync(new MemoryStream(data), cancellationToken);
        }

        public static Task<HttpResponseMessage> ReadResponseAsync(Stream stream)
        {
            return ReadResponseAsync(stream, CancellationToken.None);
        }

        public static async Task<HttpResponseMessage> ReadResponseAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            var handler = new HttpResponseParserDelegate();
            var parser = new HttpParser(handler);

            const int bufferSize = 8 * 1024;
            var buffer = new byte[bufferSize];
            int readCount;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                readCount = await stream.ReadAsync(buffer, 0, bufferSize);
                if (readCount <= 0)
                    break;
                var segment = new ArraySegment<byte>(buffer, 0, readCount);
                parser.Execute(segment);
            } while (readCount > 0 && !handler.HasErrors);

            if (handler.HasErrors)
            {
                var exceptions = handler.Errors.Select(e => new FormatException(e)).ToArray<Exception>();
                if (exceptions.Length == 1)
                    throw exceptions[0];
                throw new AggregateException(exceptions);
            }

            return handler.Response;
        }

        public static Task<HttpResponseMessage> ReadResponseAsync(byte[] data)
        {
            return ReadResponseAsync(data, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> ReadResponseAsync(byte[] data, CancellationToken cancellationToken)
        {
            if (data == null) throw new ArgumentNullException("data");
            return ReadResponseAsync(new MemoryStream(data), cancellationToken);
        }

        public static DateTimeOffset? ParseHttpDate(string httpDate)
        {
            if (httpDate == null) throw new ArgumentNullException("httpDate");
            return DateTimeOffset.Parse(httpDate); // "r"
        }

        private static string GetHeaderValue(string header, IEnumerable<string> values)
        {
            var sb = new StringBuilder();
            var first = true;
            var separator = ", ";
            if (SpaceSeparatedHeaders.Contains(header))
                separator = " ";
            foreach (var value in values)
            {
                if (!first) sb.Append(separator);
                sb.Append(value);
                first = false;
            }
            return sb.ToString();
        }

        private static async Task WriteAsciiAsync(Stream stream, string text, CancellationToken cancellationToken)
        {
            // This can be optimized
            var buffer = Ascii.GetBytes(text);
            await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }

        private static async Task WriteHeadersAsync(Stream stream, HttpHeaders messageHeaders, HttpContent content, CancellationToken cancellationToken)
        {
            var headers = messageHeaders.AsEnumerable();
            if (content != null)
                headers = headers.Concat(content.Headers);

            foreach (var header in headers.OrderBy(h => h.Key))
            {
                await WriteAsciiAsync(stream, string.Format(@"{0}: {1}
", header.Key, GetHeaderValue(header.Key, header.Value)), cancellationToken);
            }

            await WriteAsciiAsync(stream, @"
", cancellationToken);
        }

        private static async Task WriteContentAsync(Stream stream, HttpContent content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (content != null)
            {
                await content.CopyToAsync(stream);
            }
        }

        private abstract class HttpParserDelegate : IHttpParserDelegate
        {
            private List<string> _errors;
            private IReadOnlyCollection<string> _roerrors;
            private static readonly HashSet<string> KnownContentHeaderKeys;
            private static readonly List<string> KnownContentHeaderPrefixes;
            private string _currentHeaderName;
            private Stream _tempStream;
            private StreamContent _tempContent;

            protected HttpParserDelegate()
            {
                _tempStream = new MemoryStream();
                _tempContent = new StreamContent(_tempStream);
            }

            public bool HasErrors
            {
                get { return _errors != null && _errors.Any(); }
            }

            public IReadOnlyCollection<string> Errors
            {
                get
                {
                    if (_errors == null) _errors = new List<string>();
                    if (_roerrors == null) _roerrors = new ReadOnlyCollection<string>(_errors);
                    return _roerrors;
                }
            }

            protected void AddError(string error)
            {
                if (error == null) throw new ArgumentNullException("error");
                if (_errors == null) _errors = new List<string>();
                _errors.Add(error);
            }

            public void OnMessageBegin(HttpParser parser)
            {
            }

            public void OnHeaderName(HttpParser parser, string name)
            {
                _currentHeaderName = name;
            }

            public void OnHeaderValue(HttpParser parser, string value)
            {
                ProcessHeader(parser, _currentHeaderName, value);
                _currentHeaderName = null;
            }

            public void OnHeadersEnd(HttpParser parser)
            {
            }

            public void OnBody(HttpParser parser, ArraySegment<byte> data)
            {
                if (Content == null)
                    Content = _tempContent;

                _tempStream.Write(data.Array, data.Offset, data.Count);
            }

            public void OnMessageEnd(HttpParser parser)
            {
                Version = new Version(parser.MajorVersion, parser.MinorVersion);

                if (Content == null && _tempContent.Headers.Any())
                    Content = _tempContent;
            }

            protected virtual void ProcessHeader(HttpParser parser, string name, string value)
            {
                if (KnownContentHeaderKeys.Contains(name))
                {
                    ProcessHeader(parser, ContentHeaders, name, value);
                }
                else if (KnownContentHeaderPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    ProcessHeader(parser, ContentHeaders, name, value);
                }
                else
                {
                    ProcessHeader(parser, SpecificHeaders, name, value);
                }
            }

            private void ProcessHeader(HttpParser parser, HttpHeaders headers, string name, string value)
            {
                IEnumerable<string> values;
                var newValues = new[] { value };
                if (headers.TryGetValues(name, out values))
                    values = values.Concat(newValues);
                else
                    values = newValues;
                headers.Add(name, values);
            }

            public HttpContentHeaders ContentHeaders
            {
                get
                {
                    if (Content != null)
                        return Content.Headers;
                    return _tempContent.Headers;
                }
            }

            public abstract HttpHeaders SpecificHeaders { get; }

            public abstract HttpContent Content { get; set; }
            public abstract Version Version { get; set; }

            static HttpParserDelegate()
            {
                KnownContentHeaderKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Allow", 
                    "Content-Disposition", 
                    "Content-Encoding", 
                    "Content-Language", 
                    "Content-Length", 
                    "Content-Location", 
                    "Content-MD5", 
                    "Content-Range", 
                    "Content-Type", 
                    "Expires", 
                    "Last-Modified", 
                };
                KnownContentHeaderPrefixes = new List<string>()
                {
                    "Content-",
                    "X-Content-",
                };
            }
        }

        private class HttpRequestParserDelegate : HttpParserDelegate, IHttpRequestParserDelegate
        {
            private readonly HttpRequestMessage _request;

            public HttpRequestParserDelegate() : this(new HttpRequestMessage())
            {
            }

            public HttpRequestParserDelegate(HttpRequestMessage request)
            {
                if (request == null) throw new ArgumentNullException("request");
                _request = request;
            }

            public HttpRequestMessage Request
            {
                get { return _request; }
            }

            public void OnMethod(HttpParser parser, string method)
            {
                _request.Method = GetHttpMethod(method);
            }

            public void OnRequestUri(HttpParser parser, string requestUri)
            {
                _request.RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute);
            }

            public void OnPath(HttpParser parser, string path)
            {
            }

            public void OnFragment(HttpParser parser, string fragment)
            {
            }

            public void OnQueryString(HttpParser parser, string queryString)
            {
            }

            public override HttpHeaders SpecificHeaders
            {
                get { return _request.Headers; }
            }

            public override HttpContent Content
            {
                get { return _request.Content; }
                set { _request.Content = value; }
            }

            public override Version Version
            {
                get { return _request.Version; }
                set { _request.Version = value; }
            }

            static HttpRequestParserDelegate()
            {
                HttpMethodCache = new ConcurrentDictionary<string, HttpMethod>(
                    new Dictionary<string, HttpMethod>
                    {
                        {"GET", HttpMethod.Get},
                        {"DELETE", HttpMethod.Delete},
                        {"HEAD", HttpMethod.Head},
                        {"OPTIONS", HttpMethod.Options},
                        {"POST", HttpMethod.Post},
                        {"PUT", HttpMethod.Put},
                        {"TRACE", HttpMethod.Trace},
                    });
            }

            private static readonly ConcurrentDictionary<string, HttpMethod> HttpMethodCache = new ConcurrentDictionary<string, HttpMethod>();
            private static HttpMethod GetHttpMethod(string method)
            {
                return HttpMethodCache.GetOrAdd(method, m => new HttpMethod(m));
            }
        }

        private class HttpResponseParserDelegate : HttpParserDelegate, IHttpResponseParserDelegate
        {
            private readonly HttpResponseMessage _response;
            private static readonly Dictionary<string, Action<string, string, HttpResponseHeaders>> KnownResponseHeaders;

            public HttpResponseParserDelegate() : this(new HttpResponseMessage())
            {
            }

            public HttpResponseParserDelegate(HttpResponseMessage response)
            {
                if (response == null) throw new ArgumentNullException("response");
                _response = response;
            }

            public HttpResponseMessage Response
            {
                get { return _response; }
            }

            public void OnResponseCode(HttpParser parser, int statusCode, string statusReason)
            {
                _response.StatusCode = (HttpStatusCode) statusCode;
                _response.ReasonPhrase = statusReason.Trim();
            }

            public override HttpHeaders SpecificHeaders
            {
                get { return _response.Headers; }
            }

            public override HttpContent Content
            {
                get { return _response.Content; }
                set { _response.Content = value; }
            }

            public override Version Version
            {
                get { return _response.Version; }
                set { _response.Version = value; }
            }
        }
    }
}
