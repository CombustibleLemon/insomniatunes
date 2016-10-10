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
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private Thread _thread;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(WorkerThreadFunc);
            _thread.Name = "Worker Thread";
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void WorkerThreadFunc()
        {
            while (!_shutdownEvent.WaitOne(0))
            {

            }
        }

        protected override void OnStop()
        {
            _shutdownEvent.Set();
            if (!_thread.Join(3000)) // give the thread 3 seconds to stop
            { 
                _thread.Abort();
            }
        }
    }
}
