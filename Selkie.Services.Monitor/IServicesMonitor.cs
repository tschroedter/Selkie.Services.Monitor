using JetBrains.Annotations;

namespace Selkie.Services.Monitor
{
    public interface IServicesMonitor
    {
        [NotNull]
        string GetServicesStatus();

        bool IsServiceRunning([NotNull] string serviceName);
        void Ping();
        void Start();
    }
}