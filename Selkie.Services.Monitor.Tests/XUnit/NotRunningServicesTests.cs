using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public void AreAllServicesRunningTrueForAllServicesRunningTest(
            [NotNull] [Frozen] IDetermineNotRunningServices determineNotRunningServices,
            [NotNull] NotRunningServices sut)
        {
            // assemble
            determineNotRunningServices.GetNotRunningServices()
                                       .Returns(new ServiceElement[0]);

            // act
            // assert
            Assert.True(sut.AreAllServicesRunning());
        }

        [Theory]
        [AutoNSubstituteData]
        public void AreAllServicesRunningTrueForNotAllServicesRunningTest(
            [NotNull] [Frozen] IDetermineNotRunningServices determineNotRunningServices,
            [NotNull] NotRunningServices sut)
        {
            // assemble
            ServiceElement[] serviceElements =
            {
                new ServiceElement
                {
                    ServiceName = "One"
                }
            };

            determineNotRunningServices.GetNotRunningServices()
                                       .Returns(serviceElements);

            // act
            // assert
            Assert.False(sut.AreAllServicesRunning());
        }

        [Theory]
        [AutoNSubstituteData]
        public void CurrentlyNotRunningReturnsEmptyListForAllServicesRunningTest(
            [NotNull] [Frozen] IDetermineNotRunningServices determineNotRunningServices,
            [NotNull] NotRunningServices sut)
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

            determineNotRunningServices.GetNotRunningServices()
                                       .Returns(serviceElements);

            // act
            IEnumerable <ServiceElement> actual = sut.CurrentlyNotRunning();

            // assert
            Assert.Equal(serviceElements,
                         actual);
        }
    }
}