using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.Services.Common;
using Selkie.Services.Common.Messages;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class Service
        : BaseService,
          IService
    {
        public const string ServiceName = "Monitor Service";
        private readonly IServicesStopper m_Stopper;

        public Service([NotNull] IBus bus,
                       [NotNull] ILogger logger,
                       [NotNull] ISelkieManagementClient client,
                       [NotNull] IServicesStopper stopper)
            : base(bus,
                   logger,
                   client,
                   ServiceName)
        {
            m_Stopper = stopper;
        }

        protected override void ServiceStart()
        {
            Logger.Debug("Service started...");

            var message = new ServiceStartedResponseMessage
                          {
                              ServiceName = ServiceName
                          };

            Bus.Publish(message);

            Bus.PublishAsync(new StatusResponseMessage
                             {
                                 AreAllServicesRunning = true
                             });
        }

        protected override void ServiceStop()
        {
            m_Stopper.StopAllServices(); // todo maybe all this stopping is not required??

            Bus.Publish(new ServiceStoppedResponseMessage
                        {
                            ServiceName = ServiceName
                        });

            Logger.Debug("...Service stopped!");
        }

        protected override void ServiceInitialize()
        {
            Logger.Debug("...Service Initialized...");
        }
    }
}