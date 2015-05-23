using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServicesConfigurationRepositoryTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void SectionIsNullTest()
        {
            // assemble
            var manager = Substitute.For <ISelkieConfigurationManager>();
            manager.GetSection("services").Returns(( IServicesConfigurationSection ) null);

            // act
            var repository = new ServicesConfigurationRepository(manager);

            // assert
            IEnumerable <ServiceElement> actual = repository.GetAll();

            Assert.True(!actual.Any());
        }
    }
}