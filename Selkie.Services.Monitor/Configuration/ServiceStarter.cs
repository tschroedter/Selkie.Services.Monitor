using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class ServiceStarter : IServiceStarter
    {
        private readonly IServiceProcessCreator m_Creator;

        public ServiceStarter([NotNull] IServiceProcessCreator creator)
        {
            m_Creator = creator;
        }

        public ISelkieProcess Start(ServiceElement serviceElement)
        {
            ISelkieProcess process = m_Creator.Create(serviceElement);

            if ( process == null )
            {
                return SelkieProcess.Unknown;
            }

            process.Start();

            return process;
        }
    }
}