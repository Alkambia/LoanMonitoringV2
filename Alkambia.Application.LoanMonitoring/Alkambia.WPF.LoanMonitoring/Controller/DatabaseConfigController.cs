using Alkambia.App.LoanMonitoring.Constant;
using Alkambia.App.LoanMonitoring.Helper.Runtime;
using Alkambia.WPF.LoanMonitoring.ModelHelper;
using System;
using System.Linq;
using System.Xml.Linq;
using Alkambia.WPF.LoanMonitoring.Views.Configuration;
using System.Windows;
using Constant = Alkambia.App.LoanMonitoring.Constant.AppConfiguration;


namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class DatabaseConfigController
    {
        DatabaseConfig dbConfigForm { get; set; }
        DatabaseHelperModel helperModel { get; set; }
        public DatabaseConfigController(DatabaseConfig dbConfigForm)
        {
            this.dbConfigForm = dbConfigForm;
            events();
            init();
        }
        void init()
        {
            helperModel = GetConfig();
            dbConfigForm.DataContext = helperModel;
        }
        void events()
        {
            dbConfigForm.CancelBtn.Click += CancelBtn_Click;
            dbConfigForm.SaveBtn.Click += SaveBtn_Click;
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                SaveConfig(helperModel);
                MessageBox.Show("Database Configuration Save, please close the application for the changes to take place", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dbConfigForm.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong when saving the configuration, please contact the software administrator", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dbConfigForm.Close();
        }

        public static DatabaseHelperModel GetConfig()
        {
            try
            {
                var conf = new DatabaseHelperModel()
                {
                    Server = Configuration.Instance.Server,
                    Database = Configuration.Instance.Database,
                    Username = Configuration.Instance.Username,
                    Password = Configuration.Instance.Password,

                };
                return conf;
            }
            catch
            {
                return new DatabaseHelperModel();
            }
        }

        public static void SaveConfig(DatabaseHelperModel conf)
        {
            try
            {
                XDocument xmlFile = XDocument.Load(Constant.configXmlFile);
                var infoxml = xmlFile.Elements(Constant.information).Single();
                infoxml.Element(Constant.Server).Value = ConfigXMLEncryptor.Encrypt(conf.Server);
                infoxml.Element(Constant.Database).Value = ConfigXMLEncryptor.Encrypt(conf.Database);
                infoxml.Element(Constant.Username).Value = ConfigXMLEncryptor.Encrypt(conf.Username);
                infoxml.Element(Constant.Password).Value = ConfigXMLEncryptor.Encrypt(conf.Password);
                xmlFile.Save(Constant.configXmlFile);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }
    }
}
