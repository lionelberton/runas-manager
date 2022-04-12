using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runas_manager
{
    public class MainWindowViewModel
    {
        public ObservableCollection<RunAsConfigViewModel> RunAsElements { get; } = new ObservableCollection<RunAsConfigViewModel>();

        public RunAsConfigViewModel? SelectedRunAs { get; set; }

        public CommandHandler AddCommand { get; }
        public CommandHandler RemoveCommand { get; }
        public CommandHandler ExecuteRunCommand { get; }
        public CommandHandler SaveCommand { get; }
        public CommandHandler LoadCommand { get; }

        private readonly RunAsManager _manager;

        public MainWindowViewModel(RunAsManager manager)
        {
            _manager = manager;
            foreach (var rac in _manager.RunAsConfigs)
            {
                RunAsElements.Add(new RunAsConfigViewModel(rac));
            }

            _manager.PropertyChanged += Manager_PropertyChanged;

            AddCommand = new CommandHandler(() =>
              {
                  RunAsElements.Add(new RunAsConfigViewModel(_manager.AddRunAs()));
              }, () => true);

            RemoveCommand = new CommandHandler(() =>
            {
                if (SelectedRunAs != null)
                {
                    _manager.Remove(SelectedRunAs.Config);
                }
            }, () => SelectedRunAs != null);

            ExecuteRunCommand = new CommandHandler(() =>
              {
                  SelectedRunAs?.Execute();
              }, () => SelectedRunAs != null
              && !string.IsNullOrEmpty(SelectedRunAs.ExecutablePath)
              && !string.IsNullOrEmpty(SelectedRunAs.UserName)
              && !string.IsNullOrEmpty(SelectedRunAs.Domain));

            SaveCommand = new CommandHandler(() =>
            {
                if (!string.IsNullOrEmpty(_manager.ConfigPath))
                {
                    _manager.Save();
                }
                else
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "xml|*.xml"
                    };
                    if (sfd.ShowDialog() == true)
                    {
                        _manager.Save(sfd.FileName);
                    }
                }
            }, () => _manager.RunAsConfigs.Count > 0);
            LoadCommand = new CommandHandler(() =>
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "xml|*.xml"
                };
                if (ofd.ShowDialog() == true)
                {
                    _manager.Load(ofd.FileName);
                }
            }, () => true);

        }

        private void Manager_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_manager.RunAsConfigs))
            {
                RunAsElements.Clear();
                foreach (var rac in _manager.RunAsConfigs)
                {
                    RunAsElements.Add(new RunAsConfigViewModel(rac));
                }
            }
        }
    }
}
