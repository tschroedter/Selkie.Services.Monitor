using System.Diagnostics.CodeAnalysis;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Common.Messages;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServicesStopperTests
    {
        [Theory]
        [AutoNSubstituteData]
        // ReSharper disable once TooManyArguments
        public void StopAllServicesSendsStopMessageForAllKnownServicesTest([NotNull] [Frozen] IBus bus,
                                                                           [NotNull] [Frozen] IServicesConfigurationRepository
                                                                               repository,
                                                                           [NotNull] ServicesStopper servicesStopper,
                                                                           [NotNull] ServiceElement one,
                                                                           [NotNull] ServiceElement two)
        {
            // assemble
            ServiceElement[] elements =
            {
                one,
                two
            };

            repository.GetAll().Returns(elements);

            // act
            servicesStopper.StopAllServices();

            // assert
            bus.Received().Publish(Arg.Is <StopServiceRequestMessage>(x => x.ServiceName == one.ServiceName));
            bus.Received().Publish(Arg.Is <StopServiceRequestMessage>(x => x.ServiceName == two.ServiceName));
        }
    }

    //ncrunch: no coverage start
}