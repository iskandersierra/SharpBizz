using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpBizz.Http.Tests
{
    [Binding]
    public class HttpUtils_WriteResponseAsyncSteps
    {
        [Given(@"A simple HTTP response is created with (.*), ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPResponseIsCreatedWithAndHTTPVersionAnd(int status, string reason, int major, int minor)
        {
            var response = new HttpResponseMessage((HttpStatusCode)status)
            {
                ReasonPhrase = reason,
                Version = new Version(major, minor)
            };
            ScenarioContext.Current.Set(response);
        }

        [Given(@"A simple response is created with content ""(.*)"" with value ""(.*)"" and empty string content")]
        public void GivenASimpleResponseIsCreatedWithContentWithValueAndEmptyStringContent(string header, string value)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            response.Content.Headers.Add(header, value);
            ScenarioContext.Current.Set(response);
        }

        [Given(@"A simple response is created with response ""(.*)"" with value ""(.*)""")]
        public void GivenASimpleResponseIsCreatedWithResponseWithValue(string header, string value)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add(header, value);
            ScenarioContext.Current.Set(response);
        }

        [When(@"The response message is written to a stream")]
        public void WhenTheResponseMessageIsWrittenToAStream()
        {
            var request = ScenarioContext.Current.Get<HttpResponseMessage>();
            var asciiMessage = request.WriteResponseAsync().Result;
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Then(@"The string representation of the stream is a simple response message with (.*), ""(.*)"" and HTTP version (.*) and (.*)")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleResponseMessageWithAndHTTPVersionAnd(int status, string reason, int major, int minor)
        {
            var httpMessage = string.Format(@"HTTP/{0}.{1} {2} {3}

", major, minor, status, reason);
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var textMessage = TestUtils.HttpEncoding.GetString(asciiMessage);

            Assert.That(textMessage, Is.EqualTo(httpMessage));
        }

        [Then(@"The string representation of the stream is a simple response with ""(.*)"" with value ""(.*)"" and plain text content type")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleResponseWithWithValueAndPlainTextContentType(string header, string value)
        {
            var sb = new StringBuilder();
            sb.Append(@"HTTP/1.1 200 OK").AppendLine();
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

        [Then(@"The string representation of the stream is a simple response with ""(.*)"" with value ""(.*)""")]
        public void ThenTheStringRepresentationOfTheStreamIsASimpleResponseWithWithValue(string header, string value)
        {
            var httpMessage = string.Format(@"HTTP/1.1 200 OK
{0}: {1}

", header, value);
            //var binaryMessage = TestUtils.HttpEncoding.GetBytes(httpMessage);
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var textMessage = TestUtils.HttpEncoding.GetString(asciiMessage);

            Assert.That(textMessage, Is.EqualTo(httpMessage));
        }
    }
}
