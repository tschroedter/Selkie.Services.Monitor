using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Common;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    // ReSharper disable TooManyArguments
    public sealed class ServicesStatusTests
    {
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
        public void NotRunningServicesDefaultValueTest([NotNull] ServicesStatus servicesStatus)
        {
            Assert.NotNull(servicesStatus.NotRunningServices);
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateAddsNotRunningServiceTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                    [NotNull] [Frozen] IRunningServices runningServices,
                                                    [NotNull] ServicesStatus servicesStatus,
                                                    [NotNull] ServiceElement serviceElement)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceElement
                                        });
            runningServices.CurrentlyRunning().Returns(new string[0]);

            // act
            servicesStatus.Update(this);

            // assert
            Assert.True(servicesStatus.NotRunningServices.Contains(serviceElement));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateDoesNotAddRunningServiceTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                       [NotNull] [Frozen] IRunningServices runningServices,
                                                       [NotNull] ServicesStatus servicesStatus,
                                                       [NotNull] ServiceElement serviceElement)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceElement
                                        });
            runningServices.CurrentlyRunning().Returns(new[]
                                                       {
                                                           serviceElement.ServiceName
                                                       });

            // act
            servicesStatus.Update(this);

            // assert
            Assert.False(servicesStatus.NotRunningServices.Contains(serviceElement));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSendsRestartMessageForEachStoppedServiceTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] [Frozen] IBus bus,
            [NotNull] ServicesStatus servicesStatus,
            [NotNull] ServiceElement one,
            [NotNull] ServiceElement two)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            one,
                                            two
                                        });
            runningServices.CurrentlyRunning().Returns(new string[0]);

            // act
            servicesStatus.Update(this);

            // assert
            bus.Received().PublishAsync(Arg.Is <RestartServiceRequestMessage>(x => x.ServiceName == one.ServiceName));
            bus.Received().PublishAsync(Arg.Is <RestartServiceRequestMessage>(x => x.ServiceName == two.ServiceName));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ILogger logger,
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
        public void StopCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                        [NotNull] ServicesStatus servicesStatus)
        {
            // assemble
            // act
            servicesStatus.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }
    }
}