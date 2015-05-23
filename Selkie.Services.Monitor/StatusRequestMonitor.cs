using System.Linq;
using Castle.Core;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class StatusRequestMonitor
        : IStatusRequestMonitor,
          IStartable
    {
        private readonly IBus m_Bus;
        private readonly ILogger m_Logger;
        private readonly INotRunningServices m_NotRunningServices;
        private readonly IRunningServices m_RunningServices;

        public StatusRequestMonitor([NotNull] IBus bus,
                                    [NotNull] ILogger logger,
                                    [NotNull] IRunningServices runningServices,
                                    [NotNull] INotRunningServices notRunningServices)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_RunningServices = runningServices;
            m_NotRunningServices = notRunningServices;

            m_Bus.SubscribeHandlerAsync <StatusRequestMessage>(logger,
                                                               GetType().FullName,
                                                               StatusRequestHandler);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void StatusRequestHandler([NotNull] StatusRequestMessage message)
        {
            bool allRunning = m_NotRunningServices.AreAllServicesRunning();
            string[] running = m_RunningServices.CurrentlyRunning().ToArray();
            var notRunning = new string[0];

            var response = new StatusResponseMessage
                           {
                               AreAllServicesRunning = allRunning,
                               RunningServices = running,
                               NotRunningServices = notRunning
                           };

            m_Bus.PublishAsync(response);
        }
    }
}