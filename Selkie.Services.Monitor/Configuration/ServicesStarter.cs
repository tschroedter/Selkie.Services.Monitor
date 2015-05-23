using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Logging;
using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesStarter
        : IServicesStarter,
          IStartable
    {
        private readonly ILogger m_Logger;
        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IServiceStarter m_Starter;

        public ServicesStarter([NotNull] ILogger logger,
                               [NotNull] IServicesConfigurationRepository repository,
                               [NotNull] IServiceStarter starter)
        {
            m_Logger = logger;
            m_Repository = repository;
            m_Starter = starter;
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));

            StartAllServices();
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        private void StartAllServices()
        {
            IEnumerable <ServiceElement> allServices = m_Repository.GetAll();

            foreach ( ServiceElement serviceElement in allServices )
            {
                StartService(serviceElement);
            }
        }

        private void StartService([NotNull] ServiceElement serviceElement)
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