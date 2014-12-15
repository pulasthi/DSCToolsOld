using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager.Models
{
    public abstract class BaseTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _processesPerNode = 1;
        private int _threadsPerProcess = 1;
        private BindingList<ISchedulerNode> _requiredNodes = new BindingList<ISchedulerNode>();
        
        protected BaseTask()
        {
        }

        public string Name
        {
            get
            {
                return string.Format("{0} x {1} x {2}", ThreadsPerProcess, ProcessesPerNode, _requiredNodes.Count);
            }
        }

        public int ProcessesPerNode
        {
            get
            {
                return _processesPerNode;
            }
            set
            {
                if (_processesPerNode != value)
                {
                    _processesPerNode = value;
                    OnPropertyChanged("ProcessesPerNode");
                }
            }
        }

        public int ThreadsPerProcess
        {
            get
            {
                return _threadsPerProcess;
            }
            set
            {
                if (_threadsPerProcess != value)
                {
                    _threadsPerProcess = value;
                    OnPropertyChanged("ThreadsPerProcess");
                }
            }
        }

        public int Parallelism
        {
            get
            {
                return _requiredNodes.Count * ProcessesPerNode * ThreadsPerProcess;
            }
        }

        public int MaxProcessesRequired
        {
            get
            {
                return ProcessesPerNode * RequiredNodes.Count;
            }
        }
        
        public BindingList<ISchedulerNode> RequiredNodes
        {
            get
            {
                return _requiredNodes;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
