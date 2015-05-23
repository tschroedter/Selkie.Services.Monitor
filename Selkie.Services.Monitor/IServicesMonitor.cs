using JetBrains.Annotations;

namespace Selkie.Services.Monitor
{
    public interface IServicesMonitor
    {
        void Ping();
        void Start();

        [NotNull]
        string GetServicesStatus();

        bool IsServiceRunning([NotNull] string serviceName);
    }
}