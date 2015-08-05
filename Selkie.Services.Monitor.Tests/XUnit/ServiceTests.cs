using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServiceTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void InitializeCallsCheckOrConfigureRabbitMqTest([NotNull] [Frozen] ISelkieManagementClient client,
                                                                [NotNull] Service service)
        {
            service.Initialize();

            client.Received().CheckOrConfigureRabbitMq();
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                        [NotNull] Service service)
        {
            service.Stop();

            logger.Received().Debug(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopSendsServiceStoppedResponseMessageTest([NotNull] [Frozen] IBus bus,
                                                               [NotNull] Service service)
        {
            service.Stop();

            bus.Received().Publish(Arg.Is <ServiceStoppedResponseMessage>(x => x.ServiceName == Service.ServiceName));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsStopAllServicesTest([NotNull] [Frozen] IServicesStopper stopper,
                                                 [NotNull] Service service)
        {
            service.Stop();

            stopper.Received().StopAllServices();
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartSendsMessageTest([NotNull] [Frozen] IBus bus,
                                          [NotNull] Service service)
        {
            service.Start();

            bus.Received().Publish(Arg.Is <ServiceStartedResponseMessage>(x => x.ServiceName == Service.ServiceName));
        }

        [Theory]
        [AutoNSubstituteData]
        public void InitializeCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                              [NotNull] Service service)
        {
            service.Initialize();

            logger.Received().Debug(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                         [NotNull] Service service)
        {
            service.Start();

            logger.Received().Debug(Arg.Any <string>());
        }
    }
}