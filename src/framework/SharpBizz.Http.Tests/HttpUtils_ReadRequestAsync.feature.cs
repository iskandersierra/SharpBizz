﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34209
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SharpBizz.Http.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("HttpUtils_ReadRequestAsync")]
    [NUnit.Framework.CategoryAttribute("httpSerialization")]
    public partial class HttpUtils_ReadRequestAsyncFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "HttpUtils_ReadRequestAsync.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HttpUtils_ReadRequestAsync", "Create a HttpRequestMessage from a binary serialized Http request message", ProgrammingLanguage.CSharp, new string[] {
                        "httpSerialization"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message")]
        [NUnit.Framework.TestCaseAttribute("GET", null)]
        [NUnit.Framework.TestCaseAttribute("POST", null)]
        [NUnit.Framework.TestCaseAttribute("PUT", null)]
        [NUnit.Framework.TestCaseAttribute("DELETE", null)]
        [NUnit.Framework.TestCaseAttribute("HEAD", null)]
        [NUnit.Framework.TestCaseAttribute("PATCH", null)]
        [NUnit.Framework.TestCaseAttribute("OPTIONS", null)]
        [NUnit.Framework.TestCaseAttribute("CUSTOM", null)]
        public virtual void ReadASimpleRequestMessage(string method, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given(string.Format("A simple HTTP request is recieved with \"{0}\", \"http://www.example.com/\" and HTTP " +
                        "version 1 and 1", method), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.And(string.Format("The request method is \"{0}\"", method), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with different types of URLs")]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/", null)]
        [NUnit.Framework.TestCaseAttribute("https://www.example.com/", null)]
        [NUnit.Framework.TestCaseAttribute("http://localhost/", null)]
        [NUnit.Framework.TestCaseAttribute("https://localhost/", null)]
        [NUnit.Framework.TestCaseAttribute("http://192.168.17.1/", null)]
        [NUnit.Framework.TestCaseAttribute("https://192.168.17.1/", null)]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/path", null)]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/path.ext", null)]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/?query=value", null)]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/?query=value&other=123", null)]
        [NUnit.Framework.TestCaseAttribute("http://www.example.com/path with escapes/", null)]
        public virtual void ReadASimpleRequestMessageWithDifferentTypesOfURLs(string url, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with different types of URLs", exampleTags);
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
 testRunner.Given(string.Format("A simple HTTP request is recieved with \"GET\", \"{0}\" and HTTP version 1 and 1", url), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 23
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.And(string.Format("The request url is \"{0}\"", url), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with different HTTP versions")]
        [NUnit.Framework.TestCaseAttribute("1", "1", null)]
        [NUnit.Framework.TestCaseAttribute("1", "0", null)]
        [NUnit.Framework.TestCaseAttribute("1", "7", null)]
        [NUnit.Framework.TestCaseAttribute("2", "0", null)]
        [NUnit.Framework.TestCaseAttribute("0", "9", null)]
        public virtual void ReadASimpleRequestMessageWithDifferentHTTPVersions(string major, string minor, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with different HTTP versions", exampleTags);
#line 40
this.ScenarioSetup(scenarioInfo);
#line 41
 testRunner.Given(string.Format("A simple HTTP request is recieved with \"GET\", \"http://www.example.com/\" and HTTP " +
                        "version {0} and {1}", major, minor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 43
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
 testRunner.And(string.Format("The request version is {0} and {1}", major, minor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with no headers and no content")]
        public virtual void ReadASimpleRequestMessageWithNoHeadersAndNoContent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with no headers and no content", ((string[])(null)));
#line 53
this.ScenarioSetup(scenarioInfo);
#line 54
 testRunner.Given("A simple HTTP request is recieved with \"GET\", \"http://www.example.com/\" and HTTP " +
                    "version 1 and 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 56
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 57
 testRunner.And("The request headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.And("The request content is null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with one single-valued content header and no conten" +
            "t")]
        [NUnit.Framework.TestCaseAttribute("Content-Disposition", "attachment; filename=\"fname.ext\"", null)]
        [NUnit.Framework.TestCaseAttribute("Content-Location", "/index.htm", null)]
        [NUnit.Framework.TestCaseAttribute("Content-MD5", "Q2hlY2sgSW50ZWdyaXR5IQ==", null)]
        [NUnit.Framework.TestCaseAttribute("Content-Range", "bytes 21010-47021/47022", null)]
        [NUnit.Framework.TestCaseAttribute("Content-Type", "text/html; charset=utf-8", null)]
        [NUnit.Framework.TestCaseAttribute("Expires", "Thu, 01 Dec 1994 16:00:00 GMT", null)]
        [NUnit.Framework.TestCaseAttribute("Last-Modified", "Tue, 15 Nov 1994 12:45:26 GMT", null)]
        public virtual void ReadASimpleRequestMessageWithOneSingle_ValuedContentHeaderAndNoContent(string header, string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with one single-valued content header and no conten" +
                    "t", exampleTags);
#line 60
this.ScenarioSetup(scenarioInfo);
#line 61
 testRunner.Given(string.Format("A simple HTTP GET request is recieved with \"{0}\" with value \"{1}\"", header, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
 testRunner.And("The request content headers count is 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.And("The request headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.And(string.Format("The request content header \"{0}\" has value \"{1}\"", header, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with one multi-valued content header and no content" +
            "")]
        [NUnit.Framework.TestCaseAttribute("Allow", "GET, HEAD, OPTIONS", "3", null)]
        [NUnit.Framework.TestCaseAttribute("Content-Encoding", "gzip, lzha", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Content-Language", "da, en-US, pt-PT, es-ES", "4", null)]
        public virtual void ReadASimpleRequestMessageWithOneMulti_ValuedContentHeaderAndNoContent(string header, string values, string count, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with one multi-valued content header and no content" +
                    "", exampleTags);
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
 testRunner.Given(string.Format("A simple HTTP GET request is recieved with \"{0}\" with value \"{1}\"", header, values), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 80
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 81
 testRunner.And("The request content headers count is 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
 testRunner.And("The request headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
 testRunner.And(string.Format("The request content header \"{0}\" has values \"{1}\" with {2} elements", header, values, count), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with one single-valued request header and no conten" +
            "t")]
        [NUnit.Framework.TestCaseAttribute("Accept-Datetime", "Thu, 31 May 2007 20:35:00 GMT", null)]
        [NUnit.Framework.TestCaseAttribute("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==", null)]
        [NUnit.Framework.TestCaseAttribute("Cache-Control", "no-cache", null)]
        [NUnit.Framework.TestCaseAttribute("Connection", "keep-alive", null)]
        [NUnit.Framework.TestCaseAttribute("Cookie", "$Version=1; Skin=new;", null)]
        [NUnit.Framework.TestCaseAttribute("Date", "Tue, 15 Nov 1994 08:12:31 GMT", null)]
        [NUnit.Framework.TestCaseAttribute("Expect", "100-continue", null)]
        [NUnit.Framework.TestCaseAttribute("From", "user@example.com", null)]
        [NUnit.Framework.TestCaseAttribute("Host", "www.example.com:80", null)]
        [NUnit.Framework.TestCaseAttribute("If-Match", "\"737060cd8c284d8af7ad3082f209582d\"", null)]
        [NUnit.Framework.TestCaseAttribute("If-Modified-Since", "Tue, 15 Nov 1994 08:12:31 GMT", null)]
        [NUnit.Framework.TestCaseAttribute("If-None-Match", "\"737060cd8c284d8af7ad3082f209582d\"", null)]
        [NUnit.Framework.TestCaseAttribute("If-Range", "\"737060cd8c284d8af7ad3082f209582d\"", null)]
        [NUnit.Framework.TestCaseAttribute("If-Unmodified-Since", "Tue, 15 Nov 1994 08:12:31 GMT", null)]
        [NUnit.Framework.TestCaseAttribute("Max-Forwards", "10", null)]
        [NUnit.Framework.TestCaseAttribute("Origin", "http://www.example-social-network.com", null)]
        [NUnit.Framework.TestCaseAttribute("Pragma", "no-cache", null)]
        [NUnit.Framework.TestCaseAttribute("Proxy-Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==", null)]
        [NUnit.Framework.TestCaseAttribute("Range", "bytes=500-999", null)]
        [NUnit.Framework.TestCaseAttribute("Referer", "http://en.wikipedia.org/wiki/Main_Page", null)]
        public virtual void ReadASimpleRequestMessageWithOneSingle_ValuedRequestHeaderAndNoContent(string header, string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with one single-valued request header and no conten" +
                    "t", exampleTags);
#line 90
this.ScenarioSetup(scenarioInfo);
#line 91
 testRunner.Given(string.Format("A simple HTTP GET request is recieved with \"{0}\" with value \"{1}\"", header, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 92
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 93
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 94
 testRunner.And("The request content headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.And("The request headers count is 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And(string.Format("The request header \"{0}\" has value \"{1}\"", header, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple request message with one multi-valued header and no content")]
        [NUnit.Framework.TestCaseAttribute("Accept", "text/plain, text/html", "2", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("Accept-Charset", "utf-8, utf-16, utf-32", "3", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("Accept-Encoding", "gzip, deflate", "2", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("Accept-Language", "en-US, es-ES, pt-PT", "3", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("TE", "trailers, deflate", "2", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("Upgrade", "HTTP/2.0, SHTTP/1.3, IRC/6.9, RTA/x11", "4", "CommaSeparated", null)]
        [NUnit.Framework.TestCaseAttribute("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0", "4", "ProductComments", null)]
        [NUnit.Framework.TestCaseAttribute("Via", "1.0 fred, 1.1 example.com (Apache/1.1)", "2", "CommaSeparated", null)]
        public virtual void ReadASimpleRequestMessageWithOneMulti_ValuedHeaderAndNoContent(string header, string values, string count, string valuesType, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple request message with one multi-valued header and no content", exampleTags);
#line 121
this.ScenarioSetup(scenarioInfo);
#line 122
 testRunner.Given(string.Format("A simple HTTP GET request is recieved with \"{0}\" with value \"{1}\"", header, values), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 123
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 124
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 125
 testRunner.And("The request content headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.And("The request headers count is 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
 testRunner.And(string.Format("The request header \"{0}\" has values \"{1}\" with {2} elements of type {3}", header, values, count, valuesType), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a request message with single-line plain text content")]
        [NUnit.Framework.TestCaseAttribute("Hello world!!!", null)]
        public virtual void ReadARequestMessageWithSingle_LinePlainTextContent(string content, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a request message with single-line plain text content", exampleTags);
#line 139
this.ScenarioSetup(scenarioInfo);
#line 140
 testRunner.Given(string.Format("A HTTP POST request is recieved with single-line plain text \"{0}\"", content), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 141
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 142
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
 testRunner.And("The request content headers count is 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
 testRunner.And("The request headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
 testRunner.And(string.Format("The request content length matches \"{0}\" length", content), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a request message with binary content")]
        [NUnit.Framework.TestCaseAttribute("0", null)]
        [NUnit.Framework.TestCaseAttribute("1", null)]
        [NUnit.Framework.TestCaseAttribute("10", null)]
        [NUnit.Framework.TestCaseAttribute("100", null)]
        public virtual void ReadARequestMessageWithBinaryContent(string count, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a request message with binary content", exampleTags);
#line 150
this.ScenarioSetup(scenarioInfo);
#line 151
 testRunner.Given(string.Format("A HTTP POST request is recieved with byte values up to {0} bytes", count), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 152
 testRunner.When("The message is parsed as a HttpRequestMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 153
 testRunner.Then("The request is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 154
 testRunner.And("The request content headers count is 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
 testRunner.And("The request headers count is 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
 testRunner.And(string.Format("The request content length is \"{0}\"", count), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
