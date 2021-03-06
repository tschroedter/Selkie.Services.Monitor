using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Common;
using Selkie.Services.Common.Messages;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Monitor
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class Service
        : BaseService,
          IService
    {
        public Service([NotNull] ISelkieBus bus,
                       [NotNull] ISelkieLogger logger,
                       [NotNull] ISelkieManagementClient client,
                       [NotNull] IServicesStopper stopper)
            : base(bus,
                   logger,
                   client,
                   ServiceName)
        {
            m_Stopper = stopper;
        }

        public const string ServiceName = "Monitor Service";
        private readonly IServicesStopper m_Stopper;

        protected override void ServiceInitialize()
        {
            Logger.Debug("Service Initialized...");

            ManagementClient.CheckOrConfigureRabbitMq();
            // Todo: Not perfect to call CheckOrConfigureRabbitMq here because the services are already started!
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
            m_Stopper.StopAllServices();

            Bus.Publish(new ServiceStoppedResponseMessage
                        {
                            ServiceName = ServiceName
                        });

            Logger.Debug("...Service stopped!");
        }
    }
}