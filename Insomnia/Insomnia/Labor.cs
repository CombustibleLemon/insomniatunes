using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia
{
    public class Prole
    {
        
    }

    public static class ProcessChecker
    {
        /// <summary>
        /// Checks if there exists a currently-running process with some name
        /// </summary>
        /// <param name="name">Name of process to look for</param>
        public static bool IsOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
