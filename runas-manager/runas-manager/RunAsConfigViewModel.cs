using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runas_manager
{
    public class RunAsConfigViewModel : NotifierBase
    {
        public string? Name
        {
            get
            {
                return Config.Name;
            }
            set
            {
                if (value != Config.Name)
                {
                    Config.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string? ExecutablePath
        {
            get
            {
                return Config.ExecutablePath;
            }
            set
            {
                if (value != Config.ExecutablePath)
                {
                    Config.ExecutablePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? UserName
        {
            get
            {
                return Config.UserName;
            }
            set
            {
                if (value != Config.UserName)
                {
                    Config.UserName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string? Domain
        {
            get { return Config.Domain; }
            set
            {
                if (value != Config.Domain)
                {
                    Config.Domain = value;
                    OnPropertyChanged();
                }
            }
        }

        public RunAsConfig Config { get; }
        public RunAsConfigViewModel(RunAsConfig rac)
        {
            Config = rac;
        }

        public void Execute()
        {
            var password = Password.Show();
            Config.Execute(password);
        }
    }
}
