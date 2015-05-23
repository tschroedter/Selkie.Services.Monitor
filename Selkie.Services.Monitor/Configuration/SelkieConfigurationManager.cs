using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ExcludeFromCodeCoverage]
    [ProjectComponent(Lifestyle.Singleton)]
    public class SelkieConfigurationManager : ISelkieConfigurationManager
    {
        public IServicesConfigurationSection GetSection(string services)
        {
            var config = ConfigurationManager.GetSection("services") as ServicesConfigurationSection;

            return config;
        }
    }
}