using System;
using System.Diagnostics;
using System.Linq;

namespace Selkie.Services.Monitor.SpecFlow.Steps.Common
{
    public class KillAllSelkieProcesses
    {
        public void Kill()
        {
            Process[] foundProcesses = Process.GetProcesses();
            Process[] selkieProcesses = foundProcesses.Where(IsSelkieProcess).ToArray();

            Console.WriteLine(selkieProcesses.Count() + " Selkie processes found.");

            foreach ( Process process in selkieProcesses )
            {
                KillProcess(process);
            }
        }

        private static void KillProcess(Process process)
        {
            try
            {
                Console.WriteLine("Trying to stop process '{0}'...",
                                  process.ProcessName);

                process.Kill();
            }
            catch ( Exception exception )
            {
                Console.WriteLine("Couldn't stop process '{0}' because of exception: {1}",
                                  process.ProcessName,
                                  exception.Message);
            }
        }

        private static bool IsSelkieProcess(Process x)
        {
            return x.ProcessName.StartsWith("Selkie.") && x.ProcessName.EndsWith(".Console");
        }
    }
}