using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PInvoke;
using static PInvoke.Kernel32;

namespace Insomnia
{
    public class Prole
    {
        private uint m_previousExecutionState;

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
                        if (ProcessChecker.IsCharging)
                        {
                            StopSleep();
                        }
                    }
                }
            }

            AllowSleep();
        }

        private void StopSleep()
        {
            SetThreadExecutionState(
                EXECUTION_STATE.ES_CONTINUOUS |
                EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        public void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
    }

    public static class ProcessChecker
    {
        /// <summary>
        /// Checks if there exists a currently-running process with some name
        /// </summary>
        /// <param name="name">Name of process to look for, case insensitive</param>
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

        /// <summary>
        /// Tests if computer is plugged into power
        /// </summary>
        public static bool IsCharging => (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online);
    }
}
