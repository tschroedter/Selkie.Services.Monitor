using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Monitor
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class RunningServices : IRunningServices
    {
        private readonly Dictionary <string, PingInformation> m_Services = new Dictionary <string, PingInformation>();

        public bool IsServiceRunning(string serviceName)
        {
            lock ( this )
            {
                if ( m_Services.ContainsKey(serviceName) )
                {
                    return m_Services [ serviceName ].IsRunning;
                }
            }

            return false;
        }

        public IPingInformation GetServiceInformation(string serviceName)
        {
            lock ( this )
            {
                if ( m_Services.ContainsKey(serviceName) )
                {
                    return m_Services [ serviceName ];
                }
            }

            return PingInformation.Unknown;
        }

        public IEnumerable <string> CurrentlyRunning()
        {
            lock ( this )
            {
                var list = new List <string>();

                foreach ( string key in m_Services.Keys )
                {
                    bool isRunning = m_Services [ key ].IsRunning;

                    if ( isRunning )
                    {
                        list.Add(key);
                    }
                }

                return list.ToArray();
            }
        }

        public void AddOrUpdateStatus(string serviceName)
        {
            lock ( this )
            {
                if ( m_Services.ContainsKey(serviceName) )
                {
                    IPingInformation serviceInformation = m_Services [ serviceName ];
                    serviceInformation.Received = DateTime.Now;
                }
                else
                {
                    m_Services.Add(serviceName,
                                   new PingInformation
                                   {
                                       ServiceName = serviceName,
                                       Received = DateTime.Now
                                   });
                }
            }
        }

        public string GetStatus()
        {
            lock ( this )
            {
                return !m_Services.Any()
                           ? "No service registered!"
                           : GetStatusForRunningServices();
            }
        }

        public bool AreGivenServicesAllRunning(IEnumerable <string> serviceNames)
        {
            lock ( this )
            {
                var areAllRunning = true;

                foreach ( string serviceName in serviceNames )
                {
                    IPingInformation pingInformation = m_Services [ serviceName ];

                    if ( pingInformation.IsRunning )
                    {
                        continue;
                    }

                    areAllRunning = false;
                    break;
                }
                return areAllRunning;
            }
        }

        [NotNull]
        private string GetStatusForRunningServices()
        {
            var sb = new StringBuilder();
            string lastKey = m_Services.Keys.Last();

            sb.Append("Running services:" + Environment.NewLine);

            foreach ( KeyValuePair <string, PingInformation> service in m_Services )
            {
                PingInformation information = service.Value;

                string statusText = "Service Name: {0} Running: {1} Age: {2:0}ms".Inject(service.Key,
                                                                                         information.IsRunning,
                                                                                         information.AgeInMilliseconds);
                sb.Append(statusText);

                if ( lastKey != service.Key )
                {
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}