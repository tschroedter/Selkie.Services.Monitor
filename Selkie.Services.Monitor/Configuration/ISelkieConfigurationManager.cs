using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface ISelkieConfigurationManager
    {
        [CanBeNull]
        IServicesConfigurationSection GetSection([NotNull] string services);
    }
}