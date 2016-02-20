using System;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    // todo move class to Selkie.Common, change usage to Selkie.Common
    [ProjectComponent(Lifestyle.Singleton)]
    public class SelkieEnvironment : ISelkieEnvironment
    {
        public string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }
    }
}