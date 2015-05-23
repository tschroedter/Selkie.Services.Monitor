using NUnit.Framework;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedAPingResponseMessageForRacetracksServiceStep : BaseStep
    {
        [Then(@"the result should be that I received a ping response message for Racetracks Service")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForRacetracksService" ],
                           DoNothing);

            if ( !( bool ) ScenarioContext.Current [ "IsReceivedPingResponseForRacetracksService" ] )
            {
                Assert.Fail("Didn't receive PingResponseMessage for Racetracks Service!");
            }
        }
    }
}