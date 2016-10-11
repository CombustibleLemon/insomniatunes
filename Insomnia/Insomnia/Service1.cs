using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insomnia
{
    public partial class Service1 : ServiceBase
    {
        private Thread _thread;
        private Prole _laborer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Create laborer to perform loop
            _laborer = new Prole
            {
                TargetProcesses = new string[] {"iTunes.exe"}
            };

            // Create seperate thread for execution loop
            _thread = new Thread(_laborer.Labor)
            {
                Name = "Laborer Thread",
                IsBackground = true
            };
            _thread.Start();
        }

        protected override void OnStop()
        {
            _laborer.ShutdownEvent.Set();
            if (!_thread.Join(1000)) // give the thread 1 second to stop
            { 
                _laborer.AllowSleep();
                _thread.Abort();
            }
        }
    }
}
