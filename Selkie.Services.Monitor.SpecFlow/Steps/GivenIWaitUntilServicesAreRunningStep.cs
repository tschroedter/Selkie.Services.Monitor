using System.Threading;
using Selkie.Services.Monitor.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps
{
    public class GivenIWaitUntilServicesAreRunningStep : BaseStep
    {
        [Given(@"I wait until services are running")]
        public override void Do()
        {
            Thread.Sleep(30000);
        }
    }
}