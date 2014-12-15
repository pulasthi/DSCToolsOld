using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Salsa.TestManager.Models;
using System.ComponentModel;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager.Controllers
{
    public abstract class BaseController
    {
        protected BaseController()
        {
        }

        public abstract Control JobView
        {
            get;
        }

        public abstract Control TaskView
        {
            get;
        }

        public abstract void AddTask();

        public abstract void SubmitJob();
    }
}
