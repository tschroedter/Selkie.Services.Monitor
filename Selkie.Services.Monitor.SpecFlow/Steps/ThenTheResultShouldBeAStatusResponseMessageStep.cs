using NUnit.Framework;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class ThenTheResultShouldBeAStatusResponseMessageStep : BaseStep
    {
        [Then(@"the result should be a status response message")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedStatusResponseMessage" ],
                           WhenISendAStatusRequestMessage);

            if ( !( bool ) ScenarioContext.Current [ "IsReceivedStatusResponseMessage" ] )
            {
                Assert.Fail("Didn't receive StatusResponseMessage!");
            }
        }

        public void WhenISendAStatusRequestMessage()
        {
            Bus.PublishAsync(new StatusRequestMessage());
        }
    }
}