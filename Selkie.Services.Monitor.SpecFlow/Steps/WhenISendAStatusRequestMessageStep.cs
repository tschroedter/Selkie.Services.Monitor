using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class WhenISendAStatusRequestMessageStep : BaseStep
    {
        [When(@"I send a status request message")]
        public override void Do()
        {
            Bus.PublishAsync(new StatusRequestMessage());
        }
    }
}