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
        public NotRunningServices(
            [NotNull] IDetermineNotRunningServices determineNotRunningServices)
        {
            m_DetermineNotRunningServices = determineNotRunningServices;
        }

        private readonly IDetermineNotRunningServices m_DetermineNotRunningServices;

        public bool AreAllServicesRunning()
        {
            IEnumerable <ServiceElement> notRunning = CurrentlyNotRunning();

            return !notRunning.Any();
        }

        public IEnumerable <ServiceElement> CurrentlyNotRunning()
        {
            return m_DetermineNotRunningServices.GetNotRunningServices();
        }
    }
}