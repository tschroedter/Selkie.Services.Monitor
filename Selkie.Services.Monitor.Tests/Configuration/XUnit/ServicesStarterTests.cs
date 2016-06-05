using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    // ReSharper disable TooManyArguments
    public sealed class ServicesStarterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void StartCallsGetAllTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] ServicesStarter servicesStarter)
        {
            // assemble

            // act
            servicesStarter.Start();

            // assert
            repository.Received().GetAll();
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] ServicesStarter servicesStarter)
        {
            // assemble
            // act
            servicesStarter.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsStarterTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] ServiceElement serviceOne,
            [NotNull] ServicesStarter sut)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceOne
                                        });

            // act
            sut.Start();

            // assert
            starter.Received().StartService(serviceOne);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartLogMessageTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] ServicesStarter sut)
        {
            // assemble
            // act
            sut.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }


        [Theory]
        [AutoNSubstituteData]
        public void StartStartsProcessesTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IServiceElementStarter starter,
            [NotNull] ServicesStarter sut,
            [NotNull] ServiceElement serviceOne,
            [NotNull] ServiceElement serviceTwo)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceOne,
                                            serviceTwo
                                        });

            // act
            sut.Start();

            // assert
            starter.StartService(serviceOne);
            starter.StartService(serviceTwo);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] ServicesStarter servicesStarter)
        {
            // assemble
            // act
            servicesStarter.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }
    }
}