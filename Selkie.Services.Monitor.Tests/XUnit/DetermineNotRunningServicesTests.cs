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
    public sealed class DetermineNotRunningServicesTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void UpdateAddsNotRunningServiceTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                    [NotNull] [Frozen] IRunningServices runningServices,
                                                    [NotNull] DetermineNotRunningServices sut,
                                                    [NotNull] ServiceElement serviceElement)
        {
            // assemble
            repository.GetAll().Returns(new[]
                                        {
                                            serviceElement
                                        });
            runningServices.CurrentlyRunning().Returns(new string[0]);

            // act
            IEnumerable <ServiceElement> actual = sut.GetNotRunningServices();

            // assert
            Assert.True(actual.Contains(serviceElement));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateDoesNotAddRunningServiceTest([NotNull] [Frozen] IServicesConfigurationRepository repository,
                                                       [NotNull] [Frozen] IRunningServices runningServices,
                                                       [NotNull] DetermineNotRunningServices sut,
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
            IEnumerable <ServiceElement> actual = sut.GetNotRunningServices();

            // assert
            Assert.False(actual.Contains(serviceElement));
        }
    }
}