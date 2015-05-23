using System;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor
{
    public interface IPingInformation
    {
        bool IsUnknown { get; }

        [NotNull]
        string ServiceName { get; set; }

        DateTime Received { get; set; }
        bool IsRunning { get; }
        double AgeInMilliseconds { get; }
    }
}