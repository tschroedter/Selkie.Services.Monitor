using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor
{
    public interface IRunningServices
    {
        void AddOrUpdateStatus([NotNull] string serviceName);
        bool AreGivenServicesAllRunning([NotNull] IEnumerable <string> serviceNames);

        [NotNull]
        IEnumerable <string> CurrentlyRunning();

        [NotNull]
        IPingInformation GetServiceInformation([NotNull] string serviceName);

        [NotNull]
        string GetStatus();

        bool IsServiceRunning([NotNull] string serviceName);
    }
}