using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SharpBizz.Http.Tests
{
    [Binding]
    public class HttpUtils_ReadResponseAsyncSteps
    {
        [Given(@"A simple HTTP response is recieved with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPResponseIsRecievedWithAndHTTPVersionAnd(string status, string reason, int major, int minor)
        {
            var httpMessage = string.Format(@"HTTP/{2}.{3} {0} {1} 

", status, reason, major, minor);
            var asciiMessage = TestUtils.HttpEncoding.GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Given(@"A simple HTTP response is recieved with ""(.*)"" with value ""(.*)""")]
        public void GivenASimpleHTTPResponseIsRecievedWithWithValue(string header, string value)
        {
            var httpMessage = string.Format(@"HTTP/1.1 200 OK
{0}: {1}

", header, value);
            var asciiMessage = TestUtils.HttpEncoding.GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Given(@"A HTTP response is recieved with single-line plain text ""(.*)""")]
        public void GivenAHTTPResponseIsRecievedWithSingle_LinePlainText(string content)
        {
            var httpMessage = string.Format(@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Content-Length: {1}

{0}", content, content.Length);
            var asciiMessage = TestUtils.HttpEncoding.GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Given(@"A HTTP POST response is recieved with byte values up to (.*) bytes")]
        public void GivenAHTTPPOSTResponseIsRecievedWithByteValuesUpToBytes(int count)
        {
            var memstr = new MemoryStream();
            var httpMessage = string.Format(@"HTTP/1.1 200 OK
Content-Type: application/octet-stream
Content-Length: {0}

", count);
            var buffer = TestUtils.HttpEncoding.GetBytes(httpMessage);
            memstr.Write(buffer, 0, buffer.Length);
            for (byte i = 1; i <= count; i++)
                memstr.WriteByte(i);
            var asciiMessage = memstr.ToArray();
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [When(@"The message is parsed as a HttpResponseMessage")]
        public void WhenTheMessageIsParsedAsAHttpResponseMessage()
        {
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var response = HttpUtils.ReadResponseAsync(asciiMessage).Result;
            ScenarioContext.Current.Set(response);
        }

        [Then(@"The response is not null")]
        public void ThenTheResponseMethodIsNotNull()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response, Is.Not.Null);
        }

        [Then(@"The response version is (.*) and (.*)")]
        public void ThenTheResponseVersionIsAnd(int major, int minor)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.Version, Is.EqualTo(new Version(major, minor)));
        }

        [Then(@"The response status code is ""(.*)""")]
        public void ThenTheResponseStatusCodeIs(int status)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.StatusCode, Is.EqualTo((HttpStatusCode)status));
        }
        [Then(@"The response reason phrase is ""(.*)""")]
        public void ThenTheResponseReasonPhraseIs(string reason)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.ReasonPhrase, Is.EqualTo(reason));
        }

        [Then(@"The response headers count is (.*)")]
        public void ThenTheResponseHeadersCountIs(int headersCount)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.Headers.Count(), Is.EqualTo(headersCount));
        }

        [Then(@"The response content headers count is (.*)")]
        public void ThenTheResponseContentHeadersCountIs(int headersCount)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            if (headersCount == 0)
                Assert.That(response.Content == null || !response.Content.Headers.Any(), Is.True);
            else
                Assert.That(response.Content.Headers.Count(), Is.EqualTo(headersCount));
        }

        [Then(@"The response content is null")]
        public void ThenTheResponseContentIsNull()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.Content, Is.Null);
        }

        [Then(@"The response content header ""(.*)"" has value ""(.*)""")]
        public void ThenTheResponseContentHeaderHasValue(string header, string value)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content.Headers, Is.Not.Null);
            IEnumerable<string> values;
            var found = response.Content.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(1));
            Assert.That(values.Single(), Is.EqualTo(value));
        }

        [Then(@"The response header ""(.*)"" has value ""(.*)""")]
        public void ThenTheResponseHeaderHasValue(string header, string value)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            IEnumerable<string> values;
            var found = response.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(1));
            Assert.That(values.Single(), Is.EqualTo(value));
        }

        [Then(@"The response content header ""(.*)"" has values ""(.*)"" with (.*) elements")]
        public void ThenTheResponseContentHeaderHasValuesWithElements(string header, string value, int valuesCount)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content.Headers, Is.Not.Null);
            IEnumerable<string> values;
            var found = response.Content.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(valuesCount));
            Assert.That(values, Is.EquivalentTo(value.Split(',').Select(s => s.Trim())));
        }

        [Then(@"The response header ""(.*)"" has values ""(.*)"" with (.*) elements of type (.*)")]
        public void ThenTheResponseHeaderHasValuesWithElementsOfType(string header, string value, int valuesCount, ValuesTypeEnum valuesType)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>();
            IEnumerable<string> values;
            var found = response.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(valuesCount));
            var separatedValues = TestUtils.SeparateValues(value, valuesType);
            Assert.That(values, Is.EquivalentTo(separatedValues));
        }

        [Then(@"The response content length is ""(.*)""")]
        public void ThenTheResponseContentLengthIs(int count)
        {
            var request = ScenarioContext.Current.Get<HttpResponseMessage>();
            if (request.Content != null)
            {
                Assert.That(request.Content.Headers.ContentLength, Is.EqualTo(count));
                Assert.That(TestUtils.GetStreamLength(request.Content.ReadAsStreamAsync().Result), Is.EqualTo(count));
            }
            else
                Assert.That(count, Is.EqualTo(0));
        }

        [Then(@"The response content length matches ""(.*)"" length")]
        public void ThenTheResponseContentLengthMatchesLength(string content)
        {
            var request = ScenarioContext.Current.Get<HttpResponseMessage>();
            if (request.Content != null)
            {
                Assert.That(request.Content.Headers.ContentLength, Is.EqualTo(content.Length));
                Assert.That(TestUtils.GetStreamLength(request.Content.ReadAsStreamAsync().Result), Is.EqualTo(content.Length));
            }
            else
                Assert.That(content.Length, Is.EqualTo(0));
        }
    }
}
