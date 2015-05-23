using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.Common;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesStatus
        : IServicesStatus,
          IStartable
    {
        internal const int FourSeconds = 4000;
        internal const int TenSeconds = 10000;
        private readonly IBus m_Bus;
        private readonly ILogger m_Logger;
        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IRunningServices m_RunningServices;
        private readonly ITimer m_Timer;
        private IEnumerable <ServiceElement> m_NotRunningServices = new ServiceElement[0];

        public ServicesStatus([NotNull] ILogger logger,
                              [NotNull] IBus bus,
                              [NotNull] IServicesConfigurationRepository repository,
                              [NotNull] IRunningServices runningServices,
                              [NotNull] ITimer timer)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_Repository = repository;
            m_RunningServices = runningServices;
            m_Timer = timer;
        }

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
            m_NotRunningServices = DetermineNotRunningServices();

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

        [NotNull]
        private IEnumerable <ServiceElement> DetermineNotRunningServices()
        {
            IEnumerable <ServiceElement> configured = m_Repository.GetAll();
            string[] running = m_RunningServices.CurrentlyRunning().ToArray();

            var notRunning = new List <ServiceElement>();

            foreach ( ServiceElement serviceElement in configured )
            {
                bool isRunning = running.Any(serviceName => serviceElement.ServiceName.Contains(serviceName));

                if ( !isRunning )
                {
                    notRunning.Add(serviceElement);
                }
            }

            return notRunning;
        }
    }
}