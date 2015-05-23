using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenDidNotReceivePingResponseMessageForRacetracksServiceStep : BaseStep
    {
        [Given(@"Did not receive ping response message for Racetracks Service")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedPingResponseForRacetracksService" ] = false;
        }
    }
}