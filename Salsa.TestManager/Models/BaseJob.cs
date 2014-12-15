using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.TestManager.Controllers;
using System.Windows.Forms;
using System.ComponentModel;

namespace Salsa.TestManager.Models
{
    public abstract class BaseJob : INotifyPropertyChanged
    {
        public event EventHandler NameChanged;
        public event EventHandler ExecutableFilePathChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name = string.Empty;
        private string _executableFilePath = string.Empty;

        protected BaseJob()
        {
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnNameChanged();
                }

            }
        }

        public string ExecutableFilePath
        {
            get
            {
                return _executableFilePath;
            }
            set
            {
                if (_executableFilePath != value)
                {
                    _executableFilePath = value;
                    OnExecutableFilePathChanged();
                }
            }
        }

        protected virtual void OnNameChanged()
        {
            if (NameChanged != null)
            {
                NameChanged(this, EventArgs.Empty);
            }

            OnPropertyChanged("Name");
        }

        protected virtual void OnExecutableFilePathChanged()
        {
            if (ExecutableFilePathChanged != null)
            {
                ExecutableFilePathChanged(this, EventArgs.Empty);
            }

            OnPropertyChanged("ExecutableFilePath");
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
