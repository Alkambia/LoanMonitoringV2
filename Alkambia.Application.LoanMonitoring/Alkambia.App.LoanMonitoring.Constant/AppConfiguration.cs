using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Constant
{
    public class AppConfiguration
    {
        public static string connectionStrings = "connectionStrings";
        public static string DBDataContext = "DbContext";
        public static string connStringConfig = "Server={0};Database={1};User Id={2};Password={3};";

        public static string ServerPath = "information/Server";
        public static string DatabasePath = "information/Database";
        public static string UsernamePath = "information/Username";
        public static string PasswordPath = "information/Password";
        public static string ThemeUriPath = "information/ThemeUri";

        public static string information = "information";
        public static string Server = "Server";
        public static string Database = "Database";
        public static string Username = "Username";
        public static string Password = "Password";

        public static string configXmlFile = "config.xml";
    }
}
