﻿using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Console
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class Installer
        : BasicConsoleInstaller,
          IWindsorInstaller
    {
        public override string GetPrefixOfDllsToInstall()
        {
            return "Selkie.";
        }
    }
}