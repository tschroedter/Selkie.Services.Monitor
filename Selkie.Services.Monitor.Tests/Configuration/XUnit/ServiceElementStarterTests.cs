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
    public sealed class ServiceElementStarterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void StartServiceFromServiceElement_LogsError_ForIsUnknownIsTrue(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] [Frozen] IServiceStarter serviceStarter,
            [NotNull] ServiceElement serviceOne,
            [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            selkieProcess.IsUnknown.Returns(true);

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(selkieProcess);

            var sut = new ServiceElementStarter(logger,
                                                serviceStarter);

            // act
            sut.StartService(serviceOne);

            // assert
            logger.Received().Error(Arg.Is <string>(x => x.StartsWith("...unable")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartServiceFromServiceElement_LogsStatus_WhenCalled(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] [Frozen] IServiceStarter serviceStarter,
            [NotNull] ServiceElement serviceOne,
            [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            selkieProcess.IsUnknown.Returns(false);

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(selkieProcess);

            var sut = new ServiceElementStarter(logger,
                                                serviceStarter);

            // act
            sut.StartService(serviceOne);

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Starting")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartServiceFromServiceElement_LogsSuccess_ForIsUnknownIsFalse(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] [Frozen] IServiceStarter serviceStarter,
            [NotNull] ServiceElement serviceOne,
            [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            selkieProcess.IsUnknown.Returns(false);

            ISelkieProcess process = serviceStarter.Start(Arg.Any <ServiceElement>());
            process.Returns(selkieProcess);

            var sut = new ServiceElementStarter(logger,
                                                serviceStarter);

            // act
            sut.StartService(serviceOne);

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("...service")));
        }
    }
}