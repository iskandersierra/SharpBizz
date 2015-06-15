using System;
using System.Collections.Generic;
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
    public class HttpUtils_ReadRequestAsyncSteps
    {
        [Given(@"A simple HTTP request is recieved with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPRequestIsRecievedWithAndHTTPVersionAnd(string method, string url, int major, int minor)
        {
            var httpMessage = string.Format(@"{0} {1} HTTP/{2}.{3}

", method, Uri.EscapeUriString(url), major, minor);
            var asciiMessage = Encoding.GetEncoding("ISO-8859-1").GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [Given(@"A simple HTTP GET request is recieved with ""(.*)"" with value ""(.*)""")]
        public void GivenASimpleHTTPGETRequestIsRecievedWithWithValue(string header, string value)
        {
            var httpMessage = string.Format(@"GET http://www.example.com/ HTTP/1.1
{0}: {1}

", header, value);
            var asciiMessage = Encoding.GetEncoding("ISO-8859-1").GetBytes(httpMessage);
            ScenarioContext.Current.Set(asciiMessage, "binary message");
        }

        [When(@"The message is parsed as a HttpRequestMessage")]
        public void WhenTheMessageIsParsedAsAHttpRequestMessage()
        {
            var asciiMessage = ScenarioContext.Current.Get<byte[]>("binary message");
            var request = HttpUtils.ReadRequestAsync(asciiMessage).Result;
            ScenarioContext.Current.Set(request);
        }
        
        [Then(@"The request is not null")]
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
        
        [Then(@"The request content headers count is (.*)")]
        public void ThenTheRequestContentHeadersCountIs(int headersCount)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            if (headersCount == 0)
                Assert.That(request.Content == null || !request.Content.Headers.Any(), Is.True);
            else
                Assert.That(request.Content.Headers.Count(), Is.EqualTo(headersCount));
        }
        
        [Then(@"The request content is null")]
        public void ThenTheRequestContentIsNull()
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Content, Is.Null);
        }
        
        [Then(@"The request content header ""(.*)"" has value ""(.*)""")]
        public void ThenTheRequestContentHeaderHasValue(string header, string value)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Content, Is.Not.Null);
            Assert.That(request.Content.Headers, Is.Not.Null);
            IEnumerable<string> values;
            var found = request.Content.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(1));
            Assert.That(values.Single(), Is.EqualTo(value));
        }

        [Then(@"The request header ""(.*)"" has value ""(.*)""")]
        public void ThenTheRequestHeaderHasValue(string header, string value)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            IEnumerable<string> values;
            var found = request.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(1));
            Assert.That(values.Single(), Is.EqualTo(value));
        }

        [Then(@"The request content header ""(.*)"" has values ""(.*)"" with (.*) elements")]
        public void ThenTheRequestContentHeaderHasValuesWithElements(string header, string value, int valuesCount)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            Assert.That(request.Content, Is.Not.Null);
            Assert.That(request.Content.Headers, Is.Not.Null);
            IEnumerable<string> values;
            var found = request.Content.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(valuesCount));
            Assert.That(values, Is.EquivalentTo(value.Split(',').Select(s => s.Trim())));
        }

        [Then(@"The request header ""(.*)"" has values ""(.*)"" with (.*) elements")]
        public void ThenTheRequestHeaderHasValuesWithElements(string header, string value, int valuesCount)
        {
            var request = ScenarioContext.Current.Get<HttpRequestMessage>();
            IEnumerable<string> values;
            var found = request.Headers.TryGetValues(header, out values);
            Assert.That(found, Is.True);
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count(), Is.EqualTo(valuesCount));
            Assert.That(values, Is.EquivalentTo(value.Split(',').Select(s => s.Trim())));
        }
    }
}
