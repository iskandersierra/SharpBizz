using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        public static Task WriteAsync(this HttpRequestMessage request, Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {

            });
        }
        public static async Task<byte[]> WriteAsync(this HttpRequestMessage request)
        {
            await WriteAsync(request, Stream.Null);
            return null;
        }

        public static Task WriteAsync(this HttpResponseMessage response, Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {

            });
        }
        public static async Task<byte[]> WriteAsync(this HttpResponseMessage response)
        {
            await WriteAsync(response, Stream.Null);
            return null;
        }

        public static async Task<HttpRequestMessage> ReadRequestAsync(Stream stream)
        {
            var message = new HttpRequestMessage();
            var handler = new HttpRequestParserDelegate(message);
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

            return message;
        }
        public static Task<HttpRequestMessage> ReadRequestAsync(byte[] data)
        {
            return ReadRequestAsync(new MemoryStream(data));
        }

        public static Task<HttpResponseMessage> ReadResponseAsync(Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {
                return default(HttpResponseMessage);
            });
        }
        public static Task<HttpResponseMessage> ReadResponseAsync(byte[] data)
        {
            return ReadResponseAsync(Stream.Null);
        }

        public static DateTimeOffset? ParseHttpDate(string httpDate)
        {
            return DateTimeOffset.Parse(httpDate); // "r"
        }

        private abstract class HttpParserDelegate : IHttpParserDelegate
        {
            private List<string> _errors;
            private IReadOnlyCollection<string> _roerrors;
            private static readonly Dictionary<string, Action<string, string, HttpContentHeaders>> KnownContentHeaders;
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

            private void ProcessHeader(HttpParser parser, string name, string value)
            {
                if (KnownContentHeaders.ContainsKey(name))
                {
                    var func = KnownContentHeaders[name];
                    func(name, value, ContentHeaders);
                    return;
                }

                OnProcessHeader(parser, name, value);
            }

            protected abstract void OnProcessHeader(HttpParser parser, string name, string value);

            public HttpContentHeaders ContentHeaders
            {
                get
                {
                    if (Content != null)
                        return Content.Headers;
                    return _tempContent.Headers;
                }
            }

            public abstract HttpContent Content { get; set; }
            public abstract Version Version { get; set; }

            static HttpParserDelegate()
            {
                KnownContentHeaders = new Dictionary<string, Action<string, string, HttpContentHeaders>>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    {"Allow", SetupAllowHeader},
                    {"Content-Disposition", SetupContentDispositionHeader},
                    {"Content-Encoding", SetupContentEncodingHeader},
                    {"Content-Language", SetupContentLanguageHeader},
                    {"Content-Length", SetupContentLengthHeader},
                    {"Content-Location", SetupContentLocationHeader},
                    {"Content-MD5", SetupContentMD5Header},
                    {"Content-Range", SetupContentRangeHeader},
                    {"Content-Type", SetupContentTypeHeader},
                    {"Expires", SetupExpiresHeader},
                    {"Last-Modified", SetupLastModifiedHeader},
                };
            }

            private static void SetupAllowHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.Allow.Add(value);
            }

            private static void SetupContentDispositionHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentDisposition = new ContentDispositionHeaderValue(value);
            }

            private static void SetupContentEncodingHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentEncoding.Add(value);
            }

            private static void SetupContentLanguageHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentLanguage.Add(value);
            }

            private static void SetupContentLengthHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.Add("Content-Length", value);
            }

            private static void SetupContentLocationHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentLocation = new Uri(value, UriKind.RelativeOrAbsolute);
            }

            private static void SetupContentMD5Header(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentMD5 = Convert.FromBase64String(value);
            }

            private static void SetupContentRangeHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentRange = ContentRangeHeaderValue.Parse(value);
            }

            private static void SetupContentTypeHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.ContentType = MediaTypeHeaderValue.Parse(value);
            }

            private static void SetupExpiresHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.Expires = ParseHttpDate(value);
            }

            private static void SetupLastModifiedHeader(string name, string value, HttpContentHeaders headers)
            {
                headers.LastModified = ParseHttpDate(value);
            }

        }

        private class HttpRequestParserDelegate : HttpParserDelegate, IHttpRequestParserDelegate
        {
            private readonly HttpRequestMessage _request;
            private static readonly Dictionary<string, Action<string, string, HttpRequestHeaders>> KnownRequestHeaders;

            public HttpRequestParserDelegate(HttpRequestMessage request)
            {
                if (request == null) throw new ArgumentNullException("request");
                _request = request;
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

            protected override void OnProcessHeader(HttpParser parser, string name, string value)
            {
                if (KnownRequestHeaders.ContainsKey(name))
                {
                    var func = KnownRequestHeaders[name];
                    func(name, value, _request.Headers);
                    return;
                }

                if (name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase) ||
                    name.StartsWith("X-Content-", StringComparison.OrdinalIgnoreCase))
                {
                    ContentHeaders.Add(name, value);
                }
                else
                {
                    _request.Headers.Add(name, value);
                }
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

                KnownRequestHeaders = new Dictionary<string, Action<string, string, HttpRequestHeaders>>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    {"Accept", SetupAcceptHeader},
                    {"Accept-Charset", SetupAcceptCharsetHeader},
                    {"Accept-Encoding", SetupAcceptEncodingHeader},
                    {"Accept-Language", SetupAcceptLanguageHeader},
                    {"Authorization", SetupAuthorizationHeader},
                    {"Cache-Control", SetupCacheControlHeader},
                    {"Connection", SetupConnectionHeader},
                    {"Date", SetupDateHeader},
                    {"Expect", SetupExpectHeader},
                    {"From", SetupFromHeader},
                    {"Host", SetupHostHeader},
                    {"If-Match", SetupIfMatchHeader},
                    {"If-Modified-Since", SetupIfModifiedSinceHeader},
                    {"If-None-Match", SetupIfNoneMatchHeader},
                    {"If-Range", SetupIfRangeHeader},
                    {"If-Unmodified-Since", SetupIfUnmodifiedSinceHeader},
                    {"Max-Forwards", SetupMaxForwardsHeader},
                    {"Pragma", SetupPragmaHeader},
                    {"Proxy-Authorization", SetupProxyAuthorizationHeader},
                    {"Range", SetupRangeHeader},
                    {"Referer", SetupReferrerHeader},
                    {"TE", SetupTEHeader},
                    {"Trailer", SetupTrailerHeader},
                    {"Transfer-Encoding", SetupTransferEncodingHeader},
                    {"Upgrade", SetupUpgradeHeader},
                    {"User-Agent", SetupUserAgentHeader},
                    {"Via", SetupViaHeader},
                    {"Warning", SetupWarningHeader},
                };
            }

            private static readonly ConcurrentDictionary<string, HttpMethod> HttpMethodCache = new ConcurrentDictionary<string, HttpMethod>();
            private static HttpMethod GetHttpMethod(string method)
            {
                return HttpMethodCache.GetOrAdd(method, m => new HttpMethod(m));
            }

            private static void SetupAcceptHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Accept.ParseAdd(value);
            }
            private static void SetupAcceptCharsetHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.AcceptCharset.ParseAdd(value);
            }
            private static void SetupAcceptEncodingHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.AcceptEncoding.ParseAdd(value);
            }
            private static void SetupAcceptLanguageHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.AcceptLanguage.ParseAdd(value);
            }
            private static void SetupAuthorizationHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Authorization = AuthenticationHeaderValue.Parse(value);
            }
            private static void SetupCacheControlHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.CacheControl = CacheControlHeaderValue.Parse(value);
            }
            private static void SetupConnectionHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Connection.ParseAdd(value);
            }
            private static void SetupDateHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Date = ParseHttpDate(value);
            }
            private static void SetupExpectHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Expect.ParseAdd(value);
            }
            private static void SetupFromHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.From = value;
            }
            private static void SetupHostHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Host = value;
            }
            private static void SetupIfMatchHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.IfMatch.ParseAdd(value);
            }
            private static void SetupIfModifiedSinceHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.IfModifiedSince = ParseHttpDate(value);
            }
            private static void SetupIfNoneMatchHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.IfNoneMatch.ParseAdd(value);
            }
            private static void SetupIfRangeHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.IfRange = RangeConditionHeaderValue.Parse(value);
            }
            private static void SetupIfUnmodifiedSinceHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.IfUnmodifiedSince = ParseHttpDate(value);
            }
            private static void SetupMaxForwardsHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.MaxForwards = int.Parse(value);
            }
            private static void SetupPragmaHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Pragma.ParseAdd(value);
            }
            private static void SetupProxyAuthorizationHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.ProxyAuthorization = AuthenticationHeaderValue.Parse(value);
            }
            private static void SetupRangeHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Range = RangeHeaderValue.Parse(value);
            }
            private static void SetupReferrerHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Referrer = new Uri(value, UriKind.RelativeOrAbsolute);
            }
            private static void SetupTEHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.TE.ParseAdd(value);
            }
            private static void SetupTrailerHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Trailer.ParseAdd(value);
            }
            private static void SetupTransferEncodingHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.TransferEncoding.ParseAdd(value);
            }
            private static void SetupUpgradeHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Upgrade.ParseAdd(value);
            }
            private static void SetupUserAgentHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.UserAgent.ParseAdd(value);
            }
            private static void SetupViaHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Via.ParseAdd(value);
            }
            private static void SetupWarningHeader(string name, string value, HttpRequestHeaders headers)
            {
                headers.Warning.ParseAdd(value);
            }
        }

        private class HttpResponseParserDelegate : HttpParserDelegate, IHttpResponseParserDelegate
        {
            private readonly HttpResponseMessage _response;
            private static readonly Dictionary<string, Action<string, string, HttpResponseHeaders>> KnownResponseHeaders;

            public HttpResponseParserDelegate(HttpResponseMessage response)
            {
                if (response == null) throw new ArgumentNullException("response");
                _response = response;
            }

            public void OnResponseCode(HttpParser parser, int statusCode, string statusReason)
            {
            }

            protected override void OnProcessHeader(HttpParser parser, string name, string value)
            {
                throw new NotImplementedException();
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

            static HttpResponseParserDelegate()
            {
                KnownResponseHeaders = new Dictionary<string, Action<string, string, HttpResponseHeaders>>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    {"Accept-Ranges", SetupAcceptRangesHeader},
                    {"Age", SetupAgeHeader},
                    {"Cache-Control", SetupCacheControlHeader},
                    {"Connection", SetupConnectionHeader},
                    {"Date", SetupDateHeader},
                    {"Pragma", SetupPragmaHeader},
                    {"Trailer", SetupTrailerHeader},
                    {"Transfer-Encoding", SetupTransferEncodingHeader},
                    {"Upgrade", SetupUpgradeHeader},
                    {"Via", SetupViaHeader},
                    {"Warning", SetupWarningHeader},
                };
            }

            private static void SetupAcceptRangesHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.AcceptRanges.ParseAdd(value);
            }
            private static void SetupAgeHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Age = TimeSpan.FromSeconds(int.Parse(value));
            }
            private static void SetupCacheControlHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.CacheControl = CacheControlHeaderValue.Parse(value);
            }
            private static void SetupConnectionHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Connection.ParseAdd(value);
            }
            private static void SetupDateHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Date = ParseHttpDate(value);
            }
            private static void SetupPragmaHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Pragma.ParseAdd(value);
            }
            private static void SetupTrailerHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Trailer.ParseAdd(value);
            }
            private static void SetupTransferEncodingHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.TransferEncoding.ParseAdd(value);
            }
            private static void SetupUpgradeHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Upgrade.ParseAdd(value);
            }
            private static void SetupViaHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Via.ParseAdd(value);
            }
            private static void SetupWarningHeader(string name, string value, HttpResponseHeaders headers)
            {
                headers.Warning.ParseAdd(value);
            }
        }
    }
}
