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
    //ncrunch: no coverage start
    public sealed class RestartServiceMonitorTests
    {
        // ReSharper disable MaximumChainedReferences
        // ReSharper disable TooManyArguments
        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToTestLineRequestMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                           [NotNull] RestartServiceMonitor monitor)
        {
            string subscriptionId = monitor.GetType().ToString();

            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <RestartServiceRequestMessage>>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerIgnoresUnknownServiceElementTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IServiceStarter starter,
            [NotNull] RestartServiceMonitor monitor)
        {
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(ServiceElement.Unknown);

            starter.ClearReceivedCalls();

            monitor.RestartServiceRequestHandler(message);

            starter.DidNotReceive().Start(Arg.Any <ServiceElement>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerIgnoresUnknownServiceElementAndLogsMessageTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] RestartServiceMonitor monitor)
        {
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(ServiceElement.Unknown);

            monitor.RestartServiceRequestHandler(message);

            logger.Received().Error(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerRestartsServiceTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
                                               [NotNull] [Frozen] IServiceStarter serviceStarter,
                                               [NotNull] RestartServiceMonitor monitor,
                                               [NotNull] ServiceElement serviceElement)
        {
            var message = new RestartServiceRequestMessage();

            repository.GetByServiceName(message.ServiceName).Returns(serviceElement);

            monitor.RestartServiceRequestHandler(message);

            serviceStarter.Received().Start(serviceElement);
        }

        [Theory]
        [AutoNSubstituteData]
        public void HandlerRestartsServiceAndLogsMessageTest([NotNull] [Frozen] ISelkieLogger logger,
                                                             [NotNull] [Frozen] ISelkieBus bus,
                                                             [NotNull] [Frozen] IServicesConfigurationRepository
                                                                 repository,
                                                             [NotNull] RestartServiceMonitor monitor,
                                                             [NotNull] ServiceElement serviceElement,
                                                             [NotNull] ISelkieProcess selkieProcess)
        {
            var serviceStarter = Substitute.For <IServiceStarter>();

            repository.GetByServiceName(Arg.Any <string>()).Returns(serviceElement);

            serviceStarter.Start(Arg.Any <ServiceElement>()).Returns(selkieProcess);

            monitor.RestartServiceRequestHandler(new RestartServiceRequestMessage());

            logger.Received().Info(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] RestartServiceMonitor monitor)
        {
            // assemble
            // act
            monitor.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] RestartServiceMonitor monitor)
        {
            // assemble
            // act
            monitor.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void RestartGivenServiceCallsLoggerForUnableToRestartTest([NotNull] ISelkieProcess process,
                                                                         [NotNull] ServiceElement serviceElement,
                                                                         [NotNull] ISelkieLogger logger)
        {
            // assemble
            process.IsUnknown.Returns(true);
            serviceElement.ServiceName = "Service";

            var starter = Substitute.For <IServiceStarter>();
            starter.Start(serviceElement).Returns(process);
            RestartServiceMonitor sut = CreateSut(logger,
                                                  starter);

            // act
            sut.RestartGivenService(serviceElement);

            // assert
            logger.Received().Error(Arg.Is <string>(x => x.EndsWith("unable to restart service 'Service'!")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void RestartGivenServiceCallsLoggerForRestartedTest([NotNull] ISelkieProcess process,
                                                                   [NotNull] ServiceElement serviceElement,
                                                                   [NotNull] ISelkieLogger logger)
        {
            // assemble
            process.IsUnknown.Returns(false);
            serviceElement.ServiceName = "Service";

            var starter = Substitute.For <IServiceStarter>();
            starter.Start(serviceElement).Returns(process);
            RestartServiceMonitor sut = CreateSut(logger,
                                                  starter);

            // act
            sut.RestartGivenService(serviceElement);

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.EndsWith("'Service' restarted!")));
        }

        private RestartServiceMonitor CreateSut(ISelkieLogger logger,
                                                IServiceStarter starter)
        {
            var sut = new RestartServiceMonitor(logger,
                                                Substitute.For <ISelkieBus>(),
                                                Substitute.For <IServicesConfigurationRepository>(),
                                                starter);

            return sut;
        }
    }
}