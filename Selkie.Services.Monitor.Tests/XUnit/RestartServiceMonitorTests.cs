using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    public sealed class RestartServiceMonitorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void HandlerIgnoresUnknownServiceElementAndLogsMessageTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(ServiceElement.Unknown);

            // act
            sut.RestartServiceRequestHandler(message);

            // assert
            logger.Received().Error(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerIgnoresUnknownServiceElementTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(ServiceElement.Unknown);

            starter.ClearReceivedCalls();

            // act
            sut.RestartServiceRequestHandler(message);

            // assert
            starter.DidNotReceive().StartService(Arg.Any <ServiceElement>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerRestartsServiceAndLogsMessageTest(
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] RestartServiceMonitor sut,
            [NotNull] ServiceElement serviceElement)
        {
            // assemble
            repository.GetByServiceName(Arg.Any <string>()).Returns(serviceElement);

            // act
            sut.RestartServiceRequestHandler(new RestartServiceRequestMessage());

            // assert
            starter.Received().StartService(serviceElement);
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerRestartsServiceTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] RestartServiceMonitor sut,
            [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(serviceElement);

            // act
            sut.RestartServiceRequestHandler(message);

            // assert
            starter.Received().StartService(serviceElement);
        }

        [Theory]
        [AutoNSubstituteData]
        public void RestartGivenServiceCallsStarterWhenCalled(
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] ServiceElement serviceElement,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            // act
            sut.RestartGivenService(serviceElement);

            // assert
            starter.Received().StartService(serviceElement);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            // act
            sut.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            // act
            sut.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToTestLineRequestMessageTest(
            [NotNull] [Frozen] ISelkieBus bus,
            [NotNull] RestartServiceMonitor sut)
        {
            // assemble
            string subscriptionId = sut.GetType().ToString();

            // act
            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <RestartServiceRequestMessage>>());
        }
    }
}