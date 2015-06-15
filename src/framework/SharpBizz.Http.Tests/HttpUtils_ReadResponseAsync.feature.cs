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
    [NUnit.Framework.DescriptionAttribute("HttpUtils_ReadResponseAsync")]
    [NUnit.Framework.CategoryAttribute("httpSerialization")]
    public partial class HttpUtils_ReadResponseAsyncFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "HttpUtils_ReadResponseAsync.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HttpUtils_ReadResponseAsync", "Create a HttpResponseMessage from a binary serialized Http response message", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("Read a simple response message")]
        [NUnit.Framework.TestCaseAttribute("100", "Continue", null)]
        [NUnit.Framework.TestCaseAttribute("200", "OK", null)]
        [NUnit.Framework.TestCaseAttribute("201", "Created", null)]
        [NUnit.Framework.TestCaseAttribute("301", "Moved Permanently", null)]
        [NUnit.Framework.TestCaseAttribute("404", "Not Found", null)]
        [NUnit.Framework.TestCaseAttribute("500", "Internal Server Error", null)]
        public virtual void ReadASimpleResponseMessage(string status, string reason, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple response message", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given(string.Format("A simple HTTP response is recieved with \"{0}\", \"{1}\" and HTTP version 1 and 1", status, reason), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When("The message is parsed as a HttpResponseMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("The response is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.And(string.Format("The response status code is \"{0}\"", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And(string.Format("The response reason phrase is \"{0}\"", reason), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a simple response message with different HTTP versions")]
        [NUnit.Framework.TestCaseAttribute("1", "1", null)]
        [NUnit.Framework.TestCaseAttribute("1", "0", null)]
        [NUnit.Framework.TestCaseAttribute("1", "7", null)]
        [NUnit.Framework.TestCaseAttribute("2", "0", null)]
        [NUnit.Framework.TestCaseAttribute("0", "9", null)]
        public virtual void ReadASimpleResponseMessageWithDifferentHTTPVersions(string major, string minor, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a simple response message with different HTTP versions", exampleTags);
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given(string.Format("A simple HTTP response is recieved with \"200\", \"OK\" and HTTP version {0} and {1}", major, minor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When("The message is parsed as a HttpResponseMessage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("The response is not null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.And(string.Format("The response version is {0} and {1}", major, minor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
