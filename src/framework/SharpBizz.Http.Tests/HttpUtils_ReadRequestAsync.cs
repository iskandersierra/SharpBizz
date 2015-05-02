using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpBizz.Http.Tests
{
    [Binding]
    public class HttpUtils_ReadRequestAsync
    {
        [Given(@"A simple HTTP request is recieved with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPRequestIsRecievedWithAndHTTPVersionAnd(string method, string url, int major, int minor)
        {
            var httpMessage = string.Format(@"{0} {1} HTTP/{2}.{3}

", method, Uri.EscapeUriString(url), major, minor);
            var asciiMessage = Encoding.GetEncoding("ISO-8859-1").GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }
        
        [Given(@"A GET request is recieved with some standard headers")]
        public void GivenAGETRequestIsRecievedWithSomeStandardHeaders()
        {
            var httpMessage = @"GET /hello.txt HTTP/1.1
User-Agent: curl/7.16.3 libcurl/7.16.3 OpenSSL/0.9.7l zlib/1.2.3
Host: www.example.com
Accept-Language: en, mi

";
            var asciiMessage = Encoding.GetEncoding("ISO-8859-1").GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }


//HTTP/1.1 200 OK
//Date: Mon, 27 Jul 2009 12:28:53 GMT
//Server: Apache
//Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
//ETag: "34aa387-d-1568eb00"
//Accept-Ranges: bytes
//Content-Length: 51
//Vary: Accept-Encoding
//Content-Type: text/plain

//Hello World! My payload includes a trailing CRLF.

        [When(@"The message is parsed as a HttpRequestMessage")]
        public void WhenTheMessageIsParsedAsAHttpRequestMessage()
        {
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var request = HttpUtils.ReadRequestAsync(asciiMessage).Result;
            ScenarioContext.Current.Set(request);
        }

        [Then(@"The request method is not null")]
        public void ThenTheRequestMethodIsNotNull()
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request, Is.Not.Null);
        }

        [Then(@"The request method is ""(.*)""")]
        public void ThenTheRequestMethodIs(string method)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Method.Method, Is.EqualTo(method));
        }

        [Then(@"The request url is ""(.*)""")]
        public void ThenTheRequestUrlIs(string url)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.RequestUri, Is.EqualTo(new Uri(url, UriKind.RelativeOrAbsolute)));
        }

        [Then(@"The request version is (.*) and (.*)")]
        public void ThenTheRequestVersionIsAnd(int major, int minor)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Version, Is.EqualTo(new Version(major, minor)));
        }

        [Then(@"The request headers count is (.*)")]
        public void ThenTheRequestHeadersCountIs(int headersCount)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Headers.Count(), Is.EqualTo(headersCount));
        }

        [Then(@"The request content is null")]
        public void ThenTheRequestContentIsNull()
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Content, Is.Null);
        }
    }
}
