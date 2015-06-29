using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpBizz.Http.Tests
{
    [Binding]
    public class HttpUtils_WriteRequestAsyncSteps
    {
        [Given(@"A simple HTTP request is created with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPRequestIsCreatedWithAndHTTPVersionAnd(string method, string url, int major, int minor)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), new Uri(url, UriKind.RelativeOrAbsolute))
            {
                Version = new Version(major, minor)
            };
            ScenarioContext.Current.Set(request);
        }

        [Given(@"A simple request is created with request ""(.*)"" with value ""(.*)""")]
        public void GivenASimpleRequestIsCreatedWithRequestWithValue(string header, string value)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://www.example.com/", UriKind.RelativeOrAbsolute));
            request.Headers.Add(header, value);
            ScenarioContext.Current.Set(request);
        }

        [Given(@"A simple request is created with content ""(.*)"" with value ""(.*)"" and empty string content")]
        public void GivenASimpleRequestIsCreatedWithContentWithValueAndEmptyStringContent(string header, string value)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://www.example.com/", UriKind.RelativeOrAbsolute))
            {
                Content = new StringContent("")
            };
            request.Content.Headers.Add(header, value);
            ScenarioContext.Current.Set(request);
        }

        [Given(@"A HTTP request is created with byte array content of (.*) bytes")]
        public void GivenAHTTPRequestIsCreatedWithByteArrayContentOfBytes(int count)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://www.example.com/", UriKind.RelativeOrAbsolute));
            byte[] bytes = Enumerable.Range(1, count).Select(i => (byte)i).ToArray();
            var content = new ByteArrayContent(bytes);
            request.Content = content;
            ScenarioContext.Current.Set(request);
        }

        [When(@"The request message is written to a stream")]
        public void WhenTheRequestMessageIsWrittenToAStream()
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            var asciiMessage = request.WriteRequestAsync().Result;
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Then(@"The stream is not empty")]
        public void ThenTheStreamIsNotEmpty()
        {
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            Assert.That(asciiMessage, Is.Not.Null);
            Assert.That(asciiMessage, Is.Not.Empty);
        }

        [Then(@"The string representation of the stream is a simple request message with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleRequestMessageWithAndHTTPVersionAnd(string method, string url, int major, int minor)
        {
            var httpMessage = string.Format(@"{0} {1} HTTP/{2}.{3}

", method, Uri.EscapeUriString(url), major, minor);
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var textMessage = TestUtils.HttpEncoding.GetString(asciiMessage);

            Assert.That(textMessage, Is.EqualTo(httpMessage));
        }

        [Then(@"The string representation of the stream is a simple request with ""(.*)"" with value ""(.*)""")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleRequestWithWithValue(string header, string value)
        {
            var httpMessage = string.Format(@"GET http://www.example.com/ HTTP/1.1
{0}: {1}

", header, value);
            //var binaryMessage = TestUtils.HttpEncoding.GetBytes(httpMessage);
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var textMessage = TestUtils.HttpEncoding.GetString(asciiMessage);

            Assert.That(textMessage, Is.EqualTo(httpMessage));
        }

        [Then(@"The string representation of the stream is a simple request with ""(.*)"" with value ""(.*)"" and plain text content type")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleRequestWithWithValueAndPlainTextContentType(string header, string value)
        {
            var sb = new StringBuilder();
            sb.Append(@"GET http://www.example.com/ HTTP/1.1").AppendLine();
            if (StringComparer.OrdinalIgnoreCase.Compare("Content-Type", header) < 0)
                sb
                    .AppendLine("Content-Type: text/plain; charset=utf-8")
                    .AppendFormat("{0}: {1}", header, value).AppendLine();
            else
                sb
                    .AppendFormat("{0}: {1}", header, value).AppendLine()
                    .AppendLine("Content-Type: text/plain; charset=utf-8");
            sb.AppendLine();
            var httpMessage = sb.ToString();
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var textMessage = TestUtils.HttpEncoding.GetString(asciiMessage);

            Assert.That(textMessage, Is.EqualTo(httpMessage));
        }

        [Then(@"The string representation of the stream is a request with a (.*) bytes content")]
        public void ThenTheStringRepresentationOfTheStreamIsARequestWithABytesContent(int count)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"GET http://www.example.com/ HTTP/1.1");
            sb.AppendLine();
            var httpMessage = sb.ToString();
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var memstr = new MemoryStream();
            byte[] buffer = TestUtils.HttpEncoding.GetBytes(httpMessage);
            memstr.Write(buffer, 0, buffer.Length);
            buffer = Enumerable.Range(1, count).Select(i => (byte)i).ToArray();
            memstr.Write(buffer, 0, buffer.Length);
            memstr.Seek(0, SeekOrigin.Begin);

            //var expectedMessage = memstr.ToArray();
            var asciiMessageBin = TestUtils.AsString(asciiMessage); // string.Join("", Array.ConvertAll(asciiMessage, b => b.ToString("X2")));
            var expectedMessageBin = TestUtils.AsString(memstr); // string.Join("", Array.ConvertAll(expectedMessage, b => b.ToString("X2")));
            Assert.That(asciiMessageBin, Is.EqualTo(expectedMessageBin));
        }

    }
}
