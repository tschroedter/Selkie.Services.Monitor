using System.Diagnostics;
using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class ServiceProcessCreator : IServiceProcessCreator
    {
        private readonly IPathToFullPathConverter m_Converter;
        private readonly ISelkieLogger m_Logger;
        private readonly ISelkieProcessFactory m_Factory;

        public ServiceProcessCreator([NotNull] ISelkieLogger logger,
                                     [NotNull] ISelkieProcessFactory factory,
                                     [NotNull] IPathToFullPathConverter converter)
        {
            m_Logger = logger;
            m_Factory = factory;
            m_Converter = converter;
        }

        [NotNull]
        public ISelkieProcess Create(ServiceElement service)
        {
            string folderName = m_Converter.ToFullPath(service.FolderName);
            string workingFolder = m_Converter.ToFullPath(service.WorkingFolder);

            string arguments = " /C start /B /D \"{0}\" {1}".Inject(folderName,
                                                                    service.FileName);

            m_Logger.Info("Trying to created process with the following parameters:\r\nArguments: '{0}'\r\nFolderName: '{1}'\r\nWorkingFolder: '{2}'",
                          arguments,
                          folderName,
                          workingFolder);

            var processStartInfo = new ProcessStartInfo
                                   {
                                       CreateNoWindow = false,
                                       UseShellExecute = true,
                                       FileName = "cmd.exe",
                                       WindowStyle = ProcessWindowStyle.Minimized,
                                       Arguments = arguments,
                                       WorkingDirectory = workingFolder
                                   };

            ISelkieProcess process = m_Factory.Create(processStartInfo);

            return process;
        }
    }
}