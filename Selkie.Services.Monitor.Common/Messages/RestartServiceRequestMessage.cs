using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Common.Messages
{
    public class RestartServiceRequestMessage
    {
        [NotNull]
        public string ServiceName = "Unknown Service Name";
    }
}