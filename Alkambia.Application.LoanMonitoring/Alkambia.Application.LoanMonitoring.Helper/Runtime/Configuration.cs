using Alkambia.App.LoanMonitoring.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constant = Alkambia.App.LoanMonitoring.Constant.AppConfiguration;

namespace Alkambia.App.LoanMonitoring.Helper.Runtime
{
    public class Configuration
    {
        private static Configuration _Instance = new Configuration();
        private static readonly object _Lock = new object();
        private Configuration() { }
        public static Configuration Instance
        {
            get
            {
                lock (_Lock)
                {
                    return _Instance;
                }
            }
        }

        private static string GetValue(string configname)
        {
            return ConfigXMLReader.Instance.GetInnerText("./config.xml", configname);
        }

        public string Server
        {
            get { return ConfigXMLEncryptor.Decrypt(GetValue(Constant.AppConfiguration.ServerPath)); }
        }

        public string Database
        {
            get { return ConfigXMLEncryptor.Decrypt(GetValue(Constant.AppConfiguration.DatabasePath)); }
        }

        public string Username
        {
            get { return ConfigXMLEncryptor.Decrypt(GetValue(Constant.AppConfiguration.UsernamePath)); }
        }

        public string Password
        {
            get { return ConfigXMLEncryptor.Decrypt(GetValue(Constant.AppConfiguration.PasswordPath)); }
        }
        public string ThemeUri
        {
            get { return GetValue(Constant.AppConfiguration.ThemeUriPath); }
        }
    }
}
