using System.Collections.Generic;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Common.Interfaces;
using Selkie.EasyNetQ;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesStatus
        : IServicesStatus,
          IStartable
    {
        public ServicesStatus([NotNull] ISelkieLogger logger,
                              [NotNull] ISelkieBus bus,
                              [NotNull] IDetermineNotRunningServices determineNotRunningServices,
                              [NotNull] ITimer timer)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_DetermineNotRunningServices = determineNotRunningServices;
            m_Timer = timer;
        }

        internal const int FourSeconds = 4000;
        internal const int TenSeconds = 10000;
        private readonly ISelkieBus m_Bus;
        private readonly IDetermineNotRunningServices m_DetermineNotRunningServices;
        private readonly ISelkieLogger m_Logger;
        private readonly ITimer m_Timer;
        private IEnumerable <ServiceElement> m_NotRunningServices = new ServiceElement[0];

        public IEnumerable <ServiceElement> NotRunningServices
        {
            get
            {
                return m_NotRunningServices;
            }
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));

            m_Timer.Initialize(Update,
                               TenSeconds,
                               FourSeconds);
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void Update([NotNull] object state)
        {
            m_NotRunningServices = m_DetermineNotRunningServices.GetNotRunningServices();

            RestartNotRunningServices(m_NotRunningServices);
        }

        private void RestartNotRunningServices([NotNull] IEnumerable <ServiceElement> notRunningServices)
        {
            foreach ( ServiceElement notRunningService in notRunningServices )
            {
                m_Logger.Info(
                              "Service '{0}' is not running! - Trying to restart...".Inject(
                                                                                            notRunningService
                                                                                                .ServiceName));

                var message = new RestartServiceRequestMessage
                              {
                                  ServiceName = notRunningService.ServiceName
                              };

                m_Bus.PublishAsync(message);
            }
        }
    }
}