using System;
using System.Diagnostics.CodeAnalysis;
using Castle.Windsor.Installer;
using Selkie.Services.Common;

namespace Selkie.Services.Monitor.Console
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class Program : ServiceMain
    {
        public static void Main()
        {
            StartServiceAndWaitForKey(FromAssembly.This(),
                                      Service.ServiceName);

            Environment.Exit(0);

            // todo add specflow tests and message providing status
        }
    }
}