using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Common.Interfaces;
using Selkie.EasyNetQ;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    public sealed class ServicesStatusTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void NotRunningServicesDefaultValueTest([NotNull] ServicesStatus servicesStatus)
        {
            Assert.NotNull(servicesStatus.NotRunningServices);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] ServicesStatus servicesStatus)
        {
            // assemble
            // act
            servicesStatus.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartInitializesTimerTest([NotNull] [Frozen] ITimer timer,
                                              [NotNull] ServicesStatus servicesStatus)
        {
            // assemble
            // act
            servicesStatus.Start();

            // assert
            timer.Received().Initialize(servicesStatus.Update,
                                        ServicesStatus.TenSeconds,
                                        ServicesStatus.FourSeconds);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] ServicesStatus servicesStatus)
        {
            // assemble
            // act
            servicesStatus.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateAddsNotRunningServiceTest(
            [NotNull] [Frozen] IDetermineNotRunningServices determineNotRunningServices,
            [NotNull] ServicesStatus servicesStatus,
            [NotNull] ServiceElement serviceElement)
        {
            // assemble
            determineNotRunningServices.GetNotRunningServices()
                                       .Returns(new[]
                                                {
                                                    serviceElement
                                                });

            // act
            servicesStatus.Update(this);

            // assert
            Assert.True(servicesStatus.NotRunningServices.Contains(serviceElement));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSendsRestartMessageForEachStoppedServiceTest(
            [NotNull] [Frozen] IDetermineNotRunningServices determineNotRunningServices,
            [NotNull] [Frozen] ISelkieBus bus,
            [NotNull] ServicesStatus servicesStatus,
            [NotNull] ServiceElement one,
            [NotNull] ServiceElement two)
        {
            // assemble
            determineNotRunningServices.GetNotRunningServices()
                                       .Returns(new[]
                                                {
                                                    one,
                                                    two
                                                });

            // act
            servicesStatus.Update(this);

            // assert
            bus.Received().PublishAsync(Arg.Is <RestartServiceRequestMessage>(x => x.ServiceName == one.ServiceName));
            bus.Received().PublishAsync(Arg.Is <RestartServiceRequestMessage>(x => x.ServiceName == two.ServiceName));
        }
    }
}