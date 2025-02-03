using Alkambia.App.LoanMonitoring.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constant = Alkambia.App.LoanMonitoring.Constant.AppConfiguration;

namespace Alkambia.App.LoanMonitoring.Helper.Runtime
{
    public class Connection
    {

        private static Connection _Instance = new Connection();
        private static readonly object _Lock = new object();
        private Connection() { }
        public static Connection Instance
        {
            get
            {
                lock (_Lock)
                {
                    return _Instance;
                }
            }
        }
        public string GetConnection
        {
            get
            {
                return string.Format(Constant.AppConfiguration.connStringConfig, Configuration.Instance.Server, Configuration.Instance.Database, Configuration.Instance.Username, Configuration.Instance.Password);
            }
        }
    }
}
