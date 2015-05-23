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

using TechTalk.SpecFlow;

#pragma warning disable

namespace Selkie.Services.Monitor.SpecFlow
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("MonitorService")]
    public partial class MonitorServiceFeature
    {
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }

        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }

        private static TechTalk.SpecFlow.ITestRunner testRunner;

        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"),
                                                                "MonitorService",
                                                                "",
                                                                ProgrammingLanguage.CSharp,
                                                                ( ( string[] ) ( null ) ));
            testRunner.OnFeatureStart(featureInfo);
        }

        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
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
        [NUnit.Framework.DescriptionAttribute("Monitor Service restarts other Lines Services")]
        public virtual void MonitorServiceRestartsOtherLinesServices()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Monitor Service restarts other Lines Services",
                                                                  ( ( string[] ) ( null ) ));
#line 37
            this.ScenarioSetup(scenarioInfo);
#line 38
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 39
            testRunner.And("I wait until services are running",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 40
            testRunner.And("I send a Lines Service stop message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 41
            testRunner.And("Did not receive ping response message for Lines Service",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 42
            testRunner.Then("the result should be that I received a ping response message for Lines Service",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Monitor Service starts other Services")]
        public virtual void MonitorServiceStartsOtherServices()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Monitor Service starts other Services",
                                                                  ( ( string[] ) ( null ) ));
#line 30
            this.ScenarioSetup(scenarioInfo);
#line 31
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 32
            testRunner.And("Did not receive ping response message for Lines Service",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 33
            testRunner.And("Did not receive ping response message for Racetracks Service",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 34
            testRunner.Then("the result should be that I received a ping response message for Lines Service",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line 35
            testRunner.Then("the result should be that I received a ping response message for Racetracks Service",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Ping Monitor Service")]
        public virtual void PingMonitorService()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ping Monitor Service",
                                                                  ( ( string[] ) ( null ) ));
#line 3
            this.ScenarioSetup(scenarioInfo);
#line 4
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 5
            testRunner.And("Did not receive ping response message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 6
            testRunner.When("I send a ping message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 7
            testRunner.Then("the result should be a ping response message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Started Monitor Service sends message")]
        public virtual void StartedMonitorServiceSendsMessage()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Started Monitor Service sends message",
                                                                  ( ( string[] ) ( null ) ));
#line 20
            this.ScenarioSetup(scenarioInfo);
#line 21
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 22
            testRunner.Then("the result should be that I received a ServiceStartedMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Status Request Monitor Service")]
        public virtual void StatusRequestMonitorService()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Status Request Monitor Service",
                                                                  ( ( string[] ) ( null ) ));
#line 24
            this.ScenarioSetup(scenarioInfo);
#line 25
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 26
            testRunner.And("Did not receive status response message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 27
            testRunner.When("I send a status request message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 28
            testRunner.Then("the result should be a status response message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stop Monitor Service")]
        public virtual void StopMonitorService()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stop Monitor Service",
                                                                  ( ( string[] ) ( null ) ));
#line 9
            this.ScenarioSetup(scenarioInfo);
#line 10
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 11
            testRunner.And("Did not receive ping response message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 12
            testRunner.When("I send a stop message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 13
            testRunner.Then("the result should be service not running",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stopping Monitor Service sends message")]
        public virtual void StoppingMonitorServiceSendsMessage()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stopping Monitor Service sends message",
                                                                  ( ( string[] ) ( null ) ));
#line 15
            this.ScenarioSetup(scenarioInfo);
#line 16
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 17
            testRunner.When("I send a stop message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 18
            testRunner.Then("the result should be that I received a ServiceStoppedMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}

#pragma warning restore

#endregion