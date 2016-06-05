using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesConfigurationRepository : IServicesConfigurationRepository
    {
        public ServicesConfigurationRepository([NotNull] ISelkieConfigurationManager manager)
        {
            m_Manager = manager;

            m_Services = Load();
        }

        private readonly ISelkieConfigurationManager m_Manager;
        private readonly Dictionary <string, ServiceElement> m_Services;

        public ServiceElement GetByServiceName(string name)
        {
            ServiceElement serviceElement;

            return m_Services.TryGetValue(name,
                                          out serviceElement)
                       ? serviceElement
                       : ServiceElement.Unknown;
        }

        public IEnumerable <ServiceElement> GetAll()
        {
            return m_Services.Values.ToArray();
        }

        [NotNull]
        private Dictionary <string, ServiceElement> Load()
        {
            var services = new Dictionary <string, ServiceElement>();

            IServicesConfigurationSection config = m_Manager.GetSection("services");

            if ( config == null )
            {
                return services;
            }

            ServicesCollection test = config.Instances;
            foreach ( object instance in test )
            {
                var element = instance as ServiceElement;

                if ( element != null )
                {
                    services.Add(element.ServiceName,
                                 element);
                }
            }

            return services;
        }
    }
}