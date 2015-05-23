using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Monitor.Common.Messages;
using TechTalk.SpecFlow;

// ReSharper disable once CheckNamespace

namespace Selkie.Services.Monitor.SpecFlow.Steps.Common
{
    public partial class ServiceHandlers
    {
        public void SubscribeOther()
        {
            m_Bus.SubscribeHandlerAsync <StatusResponseMessage>(m_Logger,
                                                                GetType().FullName,
                                                                StatusResponseHandler);
        }

        private void StatusResponseHandler([NotNull] StatusResponseMessage message)
        {
            ScenarioContext.Current [ "IsReceivedStatusResponseMessage" ] = true;
        }
    }
}