using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Services.Monitor.Configuration;

namespace Selkie.Services.Monitor
{
    public interface INotRunningServices
    {
        bool AreAllServicesRunning();

        [NotNull]
        IEnumerable <ServiceElement> CurrentlyNotRunning();
    }
}