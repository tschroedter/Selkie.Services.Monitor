using NUnit.Framework;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenDidReceivePingResponseMessageForLinesServiceStep : BaseStep
    {
        [Given(@"Did receive ping response message for Lines Service")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForLinesService" ],
                           DoNothing);

            if ( !( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForLinesService" ] )
            {
                Assert.Fail("Didn't receive PingResponseMessage for Lines Service!");
            }
        }
    }
}