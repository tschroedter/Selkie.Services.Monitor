using System.Collections.Generic;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [Interceptor(typeof ( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesStopper : IServicesStopper
    {
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private readonly IServicesConfigurationRepository m_Repository;

        public ServicesStopper([NotNull] ISelkieLogger logger,
                               [NotNull] ISelkieBus bus,
                               [NotNull] IServicesConfigurationRepository repository)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_Repository = repository;
        }

        public void StopAllServices()
        {
            IEnumerable <ServiceElement> allServices = m_Repository.GetAll();

            foreach ( ServiceElement serviceElement in allServices )
            {
                string serviceName = serviceElement.ServiceName;

                m_Logger.Debug("Sending message to stop service '{0}'...".Inject(serviceName));

                var message = new StopServiceRequestMessage
                              {
                                  ServiceName = serviceName
                              };

                m_Bus.Publish(message);
            }
        }
    }
}