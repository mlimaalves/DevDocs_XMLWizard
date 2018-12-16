using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLWizard
{
    class XML
    {
        private static readonly string Assemblyfolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";
        private static readonly string ConfigFileName  = "configurations.xml";
        public readonly string ConfigFilePath = Assemblyfolder + ConfigFileName;
        public readonly XmlDocument ConfigFile = new XmlDocument();

        public XML()
        {
            if(File.Exists(ConfigFilePath)) ConfigFile.Load(ConfigFilePath);
        }

        public string GetXmlElements(string strnode) => (ConfigFile.DocumentElement == null || ConfigFile.DocumentElement.SelectSingleNode(strnode) == null) ? "" : ConfigFile.DocumentElement.SelectSingleNode(strnode).InnerText;
    }
}
