using System.Diagnostics;
using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class ServiceProcessCreator : IServiceProcessCreator
    {
        private readonly ISelkieProcessFactory m_Factory;

        public ServiceProcessCreator([NotNull] ISelkieProcessFactory factory)
        {
            m_Factory = factory;
        }

        [NotNull]
        public ISelkieProcess Create(ServiceElement service)
        {
            var processStartInfo = new ProcessStartInfo
                                   {
                                       CreateNoWindow = false,
                                       UseShellExecute = true,
                                       FileName = "cmd.exe",
                                       WindowStyle = ProcessWindowStyle.Minimized,
                                       Arguments = " /C start /B /D {0} {1}".Inject(service.FolderName,
                                                                                    service.FileName),
                                       WorkingDirectory = service.WorkingFolder
                                   };

            ISelkieProcess process = m_Factory.Create(processStartInfo);

            return process;
        }
    }
}