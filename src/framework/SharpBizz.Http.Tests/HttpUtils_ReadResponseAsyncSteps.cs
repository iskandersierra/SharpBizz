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
    public class HttpUtils_ReadResponseAsyncSteps
    {
        [Given(@"A simple HTTP response is recieved with ""(.*)"", ""(.*)"" and HTTP version (.*) and (.*)")]
        public void GivenASimpleHTTPResponseIsRecievedWithAndHTTPVersionAnd(string status, string reason, int major, int minor)
        {
            var httpMessage = string.Format(@"HTTP/{2}.{3} {0} {1} 

", status, reason, major, minor);
            var asciiMessage = Encoding.GetEncoding("ISO-8859-1").GetBytes(httpMessage);
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

    }
}
