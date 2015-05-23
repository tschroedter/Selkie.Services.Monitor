using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServiceProcessCreatorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void CreateReturnsProcessTest([NotNull] ISelkieProcess selkieProcess,
                                             [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var factory = Substitute.For <ISelkieProcessFactory>();
            ISelkieProcess process = factory.Create(Arg.Any <ProcessStartInfo>());
            process.Returns(selkieProcess);

            var creator = new ServiceProcessCreator(factory);

            // act
            ISelkieProcess actual = creator.Create(serviceElement);

            // assert
            Assert.Equal(selkieProcess,
                         actual);
        }
    }
}