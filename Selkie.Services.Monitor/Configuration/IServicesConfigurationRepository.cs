using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IServicesConfigurationRepository
    {
        [NotNull]
        IEnumerable <ServiceElement> GetAll();

        [NotNull]
        ServiceElement GetByServiceName([NotNull] string name);
    }
}