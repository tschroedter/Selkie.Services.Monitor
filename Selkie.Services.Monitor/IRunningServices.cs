using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor
{
    public interface IRunningServices
    {
        bool IsServiceRunning([NotNull] string serviceName);

        [NotNull]
        IPingInformation GetServiceInformation([NotNull] string serviceName);

        [NotNull]
        IEnumerable <string> CurrentlyRunning();

        [NotNull]
        string GetStatus();

        void AddOrUpdateStatus([NotNull] string serviceName);
        bool AreGivenServicesAllRunning([NotNull] IEnumerable <string> serviceNames);
    }
}