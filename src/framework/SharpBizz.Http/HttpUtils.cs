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
using System.Text;
using System.Threading.Tasks;
using HttpMachine;

namespace SharpBizz.Http
{
    public static class HttpUtils
    {
        public static Task WriteRequestAsync(this HttpRequestMessage request, Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {

            });
        }
        public static async Task<byte[]> WriteRequestAsync(this HttpRequestMessage request)
        {
            await WriteRequestAsync(request, Stream.Null);
            return null;
        }

        public static Task WriteResponseAsync(this HttpResponseMessage response, Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {

            });
        }
        public static async Task<byte[]> WriteResponseAsync(this HttpResponseMessage response)
        {
            await WriteResponseAsync(response, Stream.Null);
            return null;
        }

        public static async Task<HttpRequestMessage> ReadRequestAsync(Stream stream)
        {
            var handler = new HttpRequestParserDelegate();
            var parser = new HttpParser(handler);

            const int bufferSize = 8 * 1024;
            var buffer = new byte[bufferSize];
            int readCount;

            do
            {
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
            if (data == null) throw new ArgumentNullException("data");
            return ReadRequestAsync(new MemoryStream(data));
        }

        public static async Task<HttpResponseMessage> ReadResponseAsync(Stream stream)
        {
            var handler = new HttpResponseParserDelegate();
            var parser = new HttpParser(handler);

            const int bufferSize = 8 * 1024;
            var buffer = new byte[bufferSize];
            int readCount;

            do
            {
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
            if (data == null) throw new ArgumentNullException("data");
            return ReadResponseAsync(new MemoryStream(data));
        }

        public static DateTimeOffset? ParseHttpDate(string httpDate)
        {
            return DateTimeOffset.Parse(httpDate); // "r"
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
