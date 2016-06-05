using System.Collections.Generic;
using Castle.Core;
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
        public ServicesStarter([NotNull] ISelkieLogger logger,
                               [NotNull] IServicesConfigurationRepository repository,
                               [NotNull] IServiceElementStarter starter)
        {
            m_Logger = logger;
            m_Repository = repository;
            m_Starter = starter;
        }

        private readonly ISelkieLogger m_Logger;
        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IServiceElementStarter m_Starter;

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
                m_Starter.StartService(serviceElement);
            }
        }
    }
}