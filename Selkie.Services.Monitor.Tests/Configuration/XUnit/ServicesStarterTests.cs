using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Common;
using Selkie.Services.Monitor.Configuration;
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
        public void StartCallsGetAllTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
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
        public void StartStartsProcessesTest([NotNull] [Frozen] ILogger logger,
                                             [NotNull] [Frozen] IServicesConfigurationRepository repository,
                                             [NotNull] [Frozen] IServiceStarter serviceStarter,
                                             [NotNull] ServicesStarter servicesStarter,
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
            servicesStarter.Start();

            // assert
            serviceStarter.Start(serviceOne);
            serviceStarter.Start(serviceTwo);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartLogStartTest([NotNull] [Frozen] ILogger logger,
                                      [NotNull] [Frozen] IServicesConfigurationRepository repository,
                                      [NotNull] ServiceElement serviceOne,
                                      [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceOne
                                        });

            selkieProcess.IsUnknown.Returns(false);

            var serviceStarter = Substitute.For <IServiceStarter>();

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(selkieProcess);

            var servicesStarter = new ServicesStarter(logger,
                                                      repository,
                                                      serviceStarter);

            // act
            servicesStarter.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("...service")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartLogSuccesfullStartTest([NotNull] [Frozen] ILogger logger,
                                                [NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                [NotNull] ServiceElement serviceOne,
                                                [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceOne
                                        });

            selkieProcess.IsUnknown.Returns(false);

            var serviceStarter = Substitute.For <IServiceStarter>();

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(selkieProcess);

            var servicesStarter = new ServicesStarter(logger,
                                                      repository,
                                                      serviceStarter);

            // act
            servicesStarter.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Starting")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartLogUnsuccesfullStartTest([NotNull] [Frozen] ILogger logger,
                                                  [NotNull] [Frozen] ISelkieManagementClient client,
                                                  [NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                  [NotNull] ServiceElement serviceOne,
                                                  [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceOne
                                        });

            var serviceStarter = Substitute.For <IServiceStarter>();

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(SelkieProcess.Unknown);

            var servicesStarter = new ServicesStarter(logger,
                                                      repository,
                                                      serviceStarter);

            // act
            servicesStarter.Start();

            // assert
            logger.Received().Error(Arg.Is <string>(x => x.StartsWith("...unable")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ILogger logger,
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
        public void StopCallsLoggerTest([NotNull] [Frozen] ILogger logger,
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