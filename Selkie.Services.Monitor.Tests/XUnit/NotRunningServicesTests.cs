using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class NotRunningServicesTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void CurrentlyNotRunningReturnsEmptyListForAllServicesRunningTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] NotRunningServices notRunningServices)
        {
            // assemble
            ServiceElement[] serviceElements =
            {
                new ServiceElement
                {
                    ServiceName = "One"
                },
                new ServiceElement
                {
                    ServiceName = "Two"
                }
            };

            string[] services =
            {
                "One",
                "Two"
            };

            repository.GetAll().Returns(serviceElements);
            runningServices.CurrentlyRunning().Returns(services);

            // act
            IEnumerable <ServiceElement> actual = notRunningServices.CurrentlyNotRunning();

            // assert
            Assert.Equal(0,
                         actual.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void CurrentlyNotRunningReturnsEmptyListForNotAllServicesRunningTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] NotRunningServices notRunningServices)
        {
            // assemble
            ServiceElement[] serviceElements =
            {
                new ServiceElement
                {
                    ServiceName = "One"
                },
                new ServiceElement
                {
                    ServiceName = "Two"
                }
            };

            string[] services =
            {
                "One"
            };

            repository.GetAll().Returns(serviceElements);
            runningServices.CurrentlyRunning().Returns(services);

            // act
            ServiceElement[] actual = notRunningServices.CurrentlyNotRunning().ToArray();

            // assert
            Assert.Equal(1,
                         actual.Length);
            Assert.Equal("Two",
                         actual [ 0 ].ServiceName);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AreAllServicesRunningTrueForAllServicesRunningTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] NotRunningServices notRunningServices)
        {
            // assemble
            ServiceElement[] serviceElements =
            {
                new ServiceElement
                {
                    ServiceName = "One"
                },
                new ServiceElement
                {
                    ServiceName = "Two"
                }
            };

            string[] services =
            {
                "One",
                "Two"
            };

            repository.GetAll().Returns(serviceElements);
            runningServices.CurrentlyRunning().Returns(services);

            // act
            bool actual = notRunningServices.AreAllServicesRunning();

            // assert
            Assert.True(actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AreAllServicesRunningTrueForNotAllServicesRunningTest(
            [NotNull] [Frozen] IServicesConfigurationRepository repository,
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] NotRunningServices notRunningServices)
        {
            // assemble
            ServiceElement[] serviceElements =
            {
                new ServiceElement
                {
                    ServiceName = "One"
                },
                new ServiceElement
                {
                    ServiceName = "Two"
                }
            };

            string[] services =
            {
                "One"
            };

            repository.GetAll().Returns(serviceElements);
            runningServices.CurrentlyRunning().Returns(services);

            // act
            bool actual = notRunningServices.AreAllServicesRunning();

            // assert
            Assert.False(actual);
        }
    }
}