using System;
using System.ServiceProcess;

namespace Insomnia
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
        [STAThread]
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
