using runas_manager.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace runas_manager
{
    public class RunAsManager : NotifierBase
    {
        public List<RunAsConfig> RunAsConfigs { get; } = new();

        public string? ConfigPath { get; private set; }
        public RunAsManager()
        {
            var configPath = Settings.Default.LastConfig;
            if (!string.IsNullOrEmpty(configPath))
            {
                Load(configPath);
            }
        }
        public RunAsConfig AddRunAs()
        {
            var r = new RunAsConfig
            {
                Name = "newName"
            };
            RunAsConfigs.Add(r);
            return r;
        }

        public void Remove(RunAsConfig config)
        {
            RunAsConfigs.Remove(config);
            OnPropertyChanged(nameof(RunAsConfigs));
        }

        public void Load(string config)
        {
            ConfigPath = config;
            var xroot = XElement.Load(config);
            RunAsConfigs.Clear();
            foreach (var xChild in xroot.Elements())
            {
                RunAsConfigs.Add(RunAsConfig.Load(xChild));
            }
            OnPropertyChanged(nameof(RunAsConfigs));
        }

        public void Save(string path)
        {
            var xroot = new XElement("Config");
            foreach (var r in RunAsConfigs)
            {
                xroot.Add(r.Save());
            }
            xroot.Save(path);
            Settings.Default.LastConfig = path;
            Settings.Default.Save();
        }

        public void Save()
        {
            if (ConfigPath != null)
            {
                Save(ConfigPath);
            }
        }
    }
}
