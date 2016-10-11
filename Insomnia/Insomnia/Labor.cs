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
            m_previousExecutionState = NativeMethods.SetThreadExecutionState(
                NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED);

            if (0 == m_previousExecutionState)
            {
                throw new Exception("NativeMethods.SetThreadExecutionState call failed");
            }
        }

        public void AllowSleep()
        {
            NativeMethods.SetThreadExecutionState(m_previousExecutionState);
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

    internal static class NativeMethods
    {
        // Import SetThreadExecutionState Win32 API and necessary flags
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
    }
}
