using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenDidNotReceiveStatusResponseMessageStep : BaseStep
    {
        [Given(@"Did not receive status response message")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedStatusResponseMessage" ] = false;
        }
    }
}