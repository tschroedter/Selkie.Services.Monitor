using Castle.Core;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class RestartServiceMonitor
        : IRestartServiceMonitor,
          IStartable
    {
        private readonly ISelkieLogger m_Logger;
        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IServiceStarter m_Starter;

        public RestartServiceMonitor([NotNull] ISelkieLogger logger,
                                     [NotNull] ISelkieBus bus,
                                     [NotNull] IServicesConfigurationRepository repository,
                                     [NotNull] IServiceStarter starter)
        {
            m_Logger = logger;
            m_Repository = repository;
            m_Starter = starter;

            bus.SubscribeAsync <RestartServiceRequestMessage>(GetType().FullName,
                                                              RestartServiceRequestHandler);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void RestartServiceRequestHandler([NotNull] RestartServiceRequestMessage message)
        {
            ServiceElement serviceElement = m_Repository.GetByServiceName(message.ServiceName);

            if ( serviceElement.IsUnknown )
            {
                m_Logger.Error("Could not restart service '{0}'!".Inject(message.ServiceName));

                return;
            }

            RestartGivenService(serviceElement);
        }

        internal void RestartGivenService([NotNull] ServiceElement serviceElement)
        {
            m_Logger.Info("Restarting service '{0}'...".Inject(serviceElement.ServiceName));

            ISelkieProcess process = m_Starter.Start(serviceElement);

            if ( process.IsUnknown )
            {
                m_Logger.Error("...unable to restart service '{0}'!".Inject(serviceElement.ServiceName));
            }
            else
            {
                m_Logger.Info("...service '{0}' restarted!".Inject(serviceElement.ServiceName));
            }
        }
    }
}