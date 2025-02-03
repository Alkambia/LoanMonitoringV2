using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constant = Alkambia.App.LoanMonitoring.Constant.AppConfiguration;

namespace Alkambia.App.LoanMonitoring.Helper.Runtime
{
    public class AppConfiguration
    {
        private static AppConfiguration _Instance = new AppConfiguration();
        private static readonly object _Lock = new object();
        private AppConfiguration() { }
        public static AppConfiguration Instance
        {
            get
            {
                lock (_Lock)
                {
                    return _Instance;
                }
            }
        }
        public void LoadConnectionString()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection(Constant.AppConfiguration.connectionStrings);
            connectionStringsSection.ConnectionStrings[Constant.AppConfiguration.DBDataContext].ConnectionString = Connection.Instance.GetConnection;
            config.Save();
            ConfigurationManager.RefreshSection(Constant.AppConfiguration.connectionStrings);
        }
    }
}
