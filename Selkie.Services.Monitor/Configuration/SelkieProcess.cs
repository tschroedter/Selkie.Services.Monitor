using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    [ProjectComponent(Lifestyle.Transient)]
    public class SelkieProcess : ISelkieProcess
    {
        private SelkieProcess(bool isUnknown)
        {
            m_IsUnknown = isUnknown;
        }

        public SelkieProcess([NotNull] ProcessStartInfo processStartInfo)
        {
            m_ProcessStartInfo = processStartInfo;
        }

        public static readonly ISelkieProcess Unknown = new SelkieProcess(true);
        private readonly bool m_IsUnknown;
        private readonly ProcessStartInfo m_ProcessStartInfo;

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        public void Start()
        {
            Process.Start(m_ProcessStartInfo);
        }
    }
}