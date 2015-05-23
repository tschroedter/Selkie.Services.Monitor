using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenDidNotReceivePingResponseMessageForLinesServiceStep : BaseStep
    {
        [Given(@"Did not receive ping response message for Lines Service")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedPingResponseForLinesService" ] = false;
        }
    }
}