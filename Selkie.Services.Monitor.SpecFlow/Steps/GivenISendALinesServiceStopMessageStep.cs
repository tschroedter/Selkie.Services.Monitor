using Selkie.Services.Common.Messages;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenISendALinesServiceStopMessageStep : BaseStep
    {
        [Given(@"I send a Lines Service stop message")]
        public override void Do()
        {
            var message = new StopServiceRequestMessage
                          {
                              IsStopAllServices = false,
                              ServiceName = "Lines Service"
                          };

            Bus.Publish(message);
        }
    }
}