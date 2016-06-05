using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Common.Interfaces;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class ServicesMonitor
        : IServicesMonitor,
          IStartable
    {
        // ReSharper disable once TooManyDependencies
        public ServicesMonitor([NotNull] ISelkieBus bus,
                               [NotNull] ISelkieLogger logger,
                               [NotNull] IRunningServices runningServices,
                               [NotNull] INotRunningServices notRunningServices,
                               [NotNull] ITimer timer)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_NotRunningServices = notRunningServices;
            m_Timer = timer;
            m_RunningServices = runningServices;
        }

        internal const int TwoSeconds = 2000;
        internal const int FiveSeconds = 5000;
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private readonly INotRunningServices m_NotRunningServices;
        private readonly IRunningServices m_RunningServices;
        private readonly ITimer m_Timer;

        public bool IsServiceRunning(string serviceName)
        {
            return m_RunningServices.IsServiceRunning(serviceName);
        }

        public string GetServicesStatus()
        {
            return m_RunningServices.GetStatus();
        }

        public void Ping()
        {
            m_Logger.Debug("Sending <Ping>...");

            var request = new PingRequestMessage();

            m_Bus.PublishAsync(request);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));

            m_Timer.Initialize(OnTimer,
                               FiveSeconds,
                               TwoSeconds);
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void LogStatus()
        {
            m_Logger.Info(m_RunningServices.GetStatus());
        }

        internal void OnTimer([NotNull] object state)
        {
            LogStatus();
            SendStatusMessage();
            Ping();
        }

        internal void SendStatusMessage()
        {
            var message = new ServicesStatusResponseMessage
                          {
                              IsAllServicesRunning = m_NotRunningServices.AreAllServicesRunning()
                          };

            m_Bus.PublishAsync(message);
        }
    }
}