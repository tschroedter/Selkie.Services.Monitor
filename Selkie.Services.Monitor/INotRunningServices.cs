using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Services.Monitor.Configuration;

namespace Selkie.Services.Monitor
{
    public interface INotRunningServices
    {
        [NotNull]
        IEnumerable <ServiceElement> CurrentlyNotRunning();

        bool AreAllServicesRunning();
    }
}