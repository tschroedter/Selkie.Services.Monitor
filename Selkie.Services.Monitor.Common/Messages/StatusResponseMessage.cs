using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Common.Messages
{
    public class StatusResponseMessage
    {
        public bool AreAllServicesRunning;

        [NotNull]
        public string[] NotRunningServices = new string[0];

        [NotNull]
        public string[] RunningServices = new string[0];
    }
}