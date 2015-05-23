using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IServicesConfigurationRepository
    {
        [NotNull]
        ServiceElement GetByServiceName([NotNull] string name);

        [NotNull]
        IEnumerable <ServiceElement> GetAll();
    }
}