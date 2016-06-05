using System.Collections.Generic;
using Selkie.Services.Monitor.Configuration;

namespace Selkie.Services.Monitor
{
    public interface IDetermineNotRunningServices
    {
        IEnumerable <ServiceElement> GetNotRunningServices();
    }
}