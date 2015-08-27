using System;
using System.Collections.Generic;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class PingResponseMonitor
        : IPingResponseMonitor,
          IStartable
    {
        internal readonly string ServiceMonitorName;
        internal readonly Dictionary <string, PingInformation> Services = new Dictionary <string, PingInformation>();
        private readonly ISelkieLogger m_Logger;
        private readonly IRunningServices m_RunningServices;

        public PingResponseMonitor([NotNull] ISelkieBus bus,
                                   [NotNull] ISelkieLogger logger,
                                   [NotNull] IRunningServices runningServices)
        {
            m_Logger = logger;
            m_RunningServices = runningServices;
            ServiceMonitorName = Service.ServiceName;

            bus.SubscribeAsync <PingResponseMessage>(GetType().FullName,
                                                     PingResponseHandler);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void PingResponseHandler([NotNull] PingResponseMessage message)
        {
            if ( IsMessageIgnored(message) )
            {
                return;
            }

            lock ( this )
            {
                LogPingResponseTime(message);

                m_RunningServices.AddOrUpdateStatus(message.ServiceName);
            }
        }

        private void LogPingResponseTime([NotNull] PingResponseMessage message)
        {
            TimeSpan duration = message.Response - message.Request;

            string text = "[{0}] Ping Reply: {1:d}ms".Inject(message.ServiceName,
                                                             duration.Milliseconds);

            m_Logger.Debug(text);
        }

        internal bool IsMessageIgnored([NotNull] PingResponseMessage message)
        {
            return message.ServiceName == ServiceMonitorName;
        }
    }
}