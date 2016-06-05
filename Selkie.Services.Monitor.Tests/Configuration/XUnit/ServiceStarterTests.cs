using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    // ReSharper disable MaximumChainedReferences
    public sealed class ServiceStarterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void StartCallsStartOnProcessTest([NotNull] ServiceElement serviceElement,
                                                 [NotNull] ISelkieProcess selkieProcess)
        {
            // assemble
            var creator = Substitute.For <IServiceProcessCreator>();
            creator.Create(serviceElement).Returns(selkieProcess);

            var serviceStarter = new ServiceStarter(creator);

            // act
            serviceStarter.Start(serviceElement);

            // assert
            selkieProcess.Received().Start();
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCreatesNewProcessTest([NotNull] [Frozen] IServiceProcessCreator creator,
                                               [NotNull] ServiceElement serviceElement,
                                               [NotNull] ServiceStarter serviceStarter)
        {
            // assemble

            // act
            serviceStarter.Start(serviceElement);

            // assert
            creator.Received().Create(serviceElement);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartReturnsProcessTest([NotNull] [Frozen] IServiceProcessCreator creator,
                                            [NotNull] ServiceElement serviceElement,
                                            [NotNull] ServiceStarter serviceStarter)
        {
            // assemble

            // act
            ISelkieProcess actual = serviceStarter.Start(serviceElement);

            // assert
            Assert.False(actual.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartReturnsUnknownProcessForErrorTest([NotNull] ServiceElement serviceElement)
        {
            // assemble
            var creator = Substitute.For <IServiceProcessCreator>();
            creator.Create(serviceElement).Returns(( ISelkieProcess ) null);

            var serviceStarter = new ServiceStarter(creator);

            // act
            ISelkieProcess actual = serviceStarter.Start(serviceElement);

            // assert
            Assert.True(actual.IsUnknown);
        }
    }
}