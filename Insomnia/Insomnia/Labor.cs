using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insomnia
{
    public class Prole
    {
        /// <summary>
        /// Processes to look for, case insensitive
        /// </summary>
        public string[] TargetProcesses;

        /// <summary>
        /// Used to trigger a shutdown
        /// </summary>
        public ManualResetEvent ShutdownEvent;

        public Prole()
        {
            ShutdownEvent = new ManualResetEvent(false);
        }

        public void Labor()
        {
            while (!ShutdownEvent.WaitOne(0)) // Go until ShutdownEvent says stop
            {
                foreach (string process in TargetProcesses)
                {
                    if (ProcessChecker.IsOpen(process))
                    {
                        StopSleep();
                    }
                }
            }
        }

        private void StopSleep()
        {
            
        }
    }

    public static class ProcessChecker
    {
        /// <summary>
        /// Checks if there exists a currently-running process with some name, case insensitive
        /// </summary>
        /// <param name="name">Name of process to look for</param>
        public static bool IsOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.ToLower().Contains(name.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
