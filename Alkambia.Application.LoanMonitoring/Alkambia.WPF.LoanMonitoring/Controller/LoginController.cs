using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class LoginController
    {
        private Views.Login.LoginForm LoginForm { get; set; }
        private Model.Account AccountClass { get; set; }
        private bool Cancel { get; set; }

        private static LoginController _Instance = new LoginController();
        private static readonly object _Lock = new object();
        private LoginController() { }
        public static LoginController Instance
        {
            get
            {
                lock (_Lock)
                {
                    return _Instance;
                }
            }
        }

        public void ShowLoginWindow()
        {
            Cancel = true;
            LoginForm = new Views.Login.LoginForm();
            LoginForm = new Views.Login.LoginForm();
            LoginForm.cancelbtn.Click += Cancelbtn_Click;
            LoginForm.loginbtn.Click += Loginbtn_Click;
            LoginForm.ShowDialog();
        }

        public Model.Account CurrentLogin
        {
            get
            {
                return AccountClass;
            }
            
        }

        public bool IsCancel {
            get
            {
                return Cancel;
            }
        }
        private void Loginbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AccountClass = AccountManager.Get(LoginForm.usernametb.Text, LoginForm.passwordtb.Password);
            if(AccountClass == null)
            {
                MessageBox.Show("Username and Password didn't match, please re-enter your login credential", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Cancel = false;
                MessageBox.Show(string.Format("Welcome {0}", AccountClass.DisplayName), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginForm.Close();
            }

        }
        private void Cancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Cancel = true;
            LoginForm.Close();
        }
    }
}
