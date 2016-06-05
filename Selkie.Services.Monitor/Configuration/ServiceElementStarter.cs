using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class ServiceElementStarter : IServiceElementStarter
    {
        public ServiceElementStarter(
            [NotNull] ISelkieLogger logger,
            [NotNull] IServiceStarter starter)
        {
            m_Logger = logger;
            m_Starter = starter;
        }

        private readonly ISelkieLogger m_Logger;
        private readonly IServiceStarter m_Starter;

        public void StartService(
            ServiceElement serviceElement)
        {
            m_Logger.Info("Starting service '{0}'...".Inject(serviceElement.ServiceName));

            ISelkieProcess process = m_Starter.Start(serviceElement);

            if ( process.IsUnknown )
            {
                m_Logger.Error("...unable to start service '{0}'!".Inject(serviceElement.ServiceName));
            }
            else
            {
                m_Logger.Info("...service '{0}' started!".Inject(serviceElement.ServiceName));
            }
        }
    }
}