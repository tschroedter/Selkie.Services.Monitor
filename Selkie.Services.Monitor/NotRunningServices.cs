using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class NotRunningServices : INotRunningServices
    {
        private readonly IServicesConfigurationRepository m_Repository;
        private readonly IRunningServices m_RunningServices;

        public NotRunningServices([NotNull] IServicesConfigurationRepository repository,
                                  [NotNull] IRunningServices runningServices)
        {
            m_Repository = repository;
            m_RunningServices = runningServices;
        }

        public bool AreAllServicesRunning()
        {
            IEnumerable <ServiceElement> notRunning = CurrentlyNotRunning();

            return !notRunning.Any();
        }

        public IEnumerable <ServiceElement> CurrentlyNotRunning()
        {
            lock ( this )
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