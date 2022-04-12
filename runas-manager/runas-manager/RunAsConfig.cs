using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace runas_manager
{
    public class RunAsConfig
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Domain { get; set; }
        public string? ExecutablePath { get; set; }

        public void Execute(string password)
        {
            if (!string.IsNullOrEmpty(ExecutablePath)
                && !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(Domain)
                )
            {
                _ = ImpersonatedProcess.Start(UserName, password, Domain, ExecutablePath);
            }
        }

        public static RunAsConfig Load(XElement xConfig)
        {
            var r = new RunAsConfig
            {
                Name = xConfig.Attribute("Name")?.Value,
                UserName = xConfig.Attribute("UserName")?.Value,
                Domain = xConfig.Attribute("Domain")?.Value,
                ExecutablePath = xConfig.Attribute("ExecutablePath")?.Value
            };
            return r;
        }

        public XElement Save()
        {
            return new XElement("RunAs", new XAttribute("Name", Name ?? ""),
                new XAttribute("UserName", UserName ?? ""),
                new XAttribute("Domain", Domain ?? ""),
                new XAttribute("ExecutablePath", ExecutablePath ?? "")
                );
        }
    }
}
