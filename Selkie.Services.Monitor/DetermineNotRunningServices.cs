using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Transient)]
    public class DetermineNotRunningServices : IDetermineNotRunningServices
    {
        public DetermineNotRunningServices(
            [NotNull] IServicesConfigurationRepository repository,
            [NotNull] IRunningServices runningServices)
        {
            m_Repository = repository;
            m_RunningServices = runningServices;
        }

        private readonly object m_Padlock = new object();

        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IRunningServices m_RunningServices;

        [NotNull]
        public IEnumerable <ServiceElement> GetNotRunningServices()
        {
            lock ( m_Padlock )
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
}