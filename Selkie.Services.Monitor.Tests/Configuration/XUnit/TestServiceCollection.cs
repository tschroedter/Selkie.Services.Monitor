using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Selkie.Services.Monitor.Configuration;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal class TestServiceCollection : IServicesCollection
    {
        private readonly ServiceElement[] m_Data;

        public TestServiceCollection()
        {
            var one = new ServiceElement
                      {
                          ServiceName = "Service"
                      };

            var two = new ServiceElement
                      {
                          ServiceName = "Other Service"
                      };

            m_Data = new[]
                     {
                         one,
                         two
                     };
        }

        public ServiceElement this[int index]
        {
            get
            {
                return m_Data [ index ];
            }

            set
            {
                m_Data [ index ] = value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return m_Data.GetEnumerator();
        }
    }
}