using NUnit.Framework;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedAPingResponseMessageForLinesServiceStep : BaseStep
    {
        [Then(@"the result should be that I received a ping response message for Lines Service")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForLinesService" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForLinesService" ],
                        "Didn't receive PingResponseMessage for Lines Service!");
        }
    }
}