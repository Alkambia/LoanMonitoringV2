using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alkambia.App.LoanMonitoring.Helper.Runtime
{
    public class ConfigXMLReader
    {
        private static ConfigXMLReader _Instance = new ConfigXMLReader();
        private static readonly object _Lock = new object();
        private ConfigXMLReader() { }
        public static ConfigXMLReader Instance
        {
            get
            {
                lock (_Lock)
                {
                    return _Instance;
                }
            }
        }

        XmlDocument _xml { get; set; }

        public string GetInnerText(string path, string xpath)
        {
            if(_xml == null)
            {
                _xml = new XmlDocument();
                _xml.Load(path);
            }
            
            System.Xml.XmlNode node = _xml.SelectSingleNode(xpath);
            return node != null ? node.InnerText : string.Empty;
        }
    }
}
