using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.WPF.LoanMonitoring.Views.Configuration;
using Alkambia.App.LoanMonitoring.Helper.Runtime;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class MainController
    {
        MainWindow mainWindow { get; set; }
        Model.Account Account { get; set; }
        public MainController(MainWindow form)
        {
            this.mainWindow = form;
            Events();

            AppConfiguration.Instance.LoadConnectionString();

            if (!SystemConfigurationManager.CheckDBConnecton())
            {
                new DatabaseConfig().ShowDialog();
            }else
            {
                if(AccountManager.IsAccountsExist())
                {
                    callLogin();
                }
                else
                {
                    Account = new Model.Account()
                    {
                        AccountID = Guid.NewGuid(),
                        Name = "InitialUser",
                        DisplayName = "Initial User",
                        Description = "Initial User",
                        CreatedDate = DateTime.Now,
                        UserName = "superuser",
                        Password = "superuser"
                    };
                    AccountManager.Add(Account);
                    callLogin();
                }
                
            }
        }

        private void callLogin()
        {
            LoginController.Instance.ShowLoginWindow();
            if (LoginController.Instance.IsCancel)
            {
                this.mainWindow.Close();
            }
            else
            {
                this.mainWindow.welcomeTB.Text = string.Format("Welcome {0}", LoginController.Instance.CurrentLogin.DisplayName);
                setAccess(LoginController.Instance.CurrentLogin.AccountType);
            }
        }

        private void setAccess(int accountType)
        {
            new QuickViewController(this.mainWindow.quickLink_user_ctrl);
            initializedMenu();
            switch (accountType)
            {
                case 1:
                    {
                        this.mainWindow.LoanApplication.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Payment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Expense.Visibility = System.Windows.Visibility.Collapsed;
                        //this.mainWindow.FixedExpense.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                case 2:
                    {
                        this.mainWindow.Miscellaneous.Visibility = System.Windows.Visibility.Collapsed;
                        //this.mainWindow.LoanApproval.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.DeclinedLoan.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Account.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Investment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Status.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.PaymentCharge.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Kind.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Relation.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.CreditTerm.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                case 3:
                    {
                        this.mainWindow.Miscellaneous.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.LoanApplication.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Releasing.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Payment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Expense.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Account.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Investment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Status.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.PaymentCharge.Visibility = System.Windows.Visibility.Collapsed;
                        //this.mainWindow.FixedExpense.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Kind.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Relation.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.CreditTerm.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Import.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.CashDisbursement.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                case 4:
                case 5:
                    {
                        this.mainWindow.Miscellaneous.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.LoanApplication.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.LoanApproval.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.DeclinedLoan.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Releasing.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Payment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Expense.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Account.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Investment.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Status.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.PaymentCharge.Visibility = System.Windows.Visibility.Collapsed;
                        //this.mainWindow.FixedExpense.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Kind.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Relation.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.CreditTerm.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.Import.Visibility = System.Windows.Visibility.Collapsed;
                        this.mainWindow.CashDisbursement.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void initializedMenu()
        {
            this.mainWindow.LoanApplication.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.LoanApproval.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.DeclinedLoan.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Releasing.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.ReleasedLoans.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Payment.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Expense.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Account.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Investment.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Status.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.PaymentCharge.Visibility = System.Windows.Visibility.Visible;
            //this.mainWindow.FixedExpense.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Kind.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Relation.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.CreditTerm.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Report.Visibility = System.Windows.Visibility.Visible;
            this.mainWindow.Miscellaneous.Visibility = System.Windows.Visibility.Visible;
        }

        private void Events()
        {
            this.mainWindow.LoanApplication.MouseLeftButtonUp += LoanApplication_PreviewMouseLeftButtonUp;
            this.mainWindow.LoanApproval.MouseLeftButtonUp += LoanApproval_PreviewMouseLeftButtonUp;
            this.mainWindow.Releasing.MouseLeftButtonUp += Releasing_PreviewMouseLeftButtonUp;
            this.mainWindow.Payment.MouseLeftButtonUp += Payment_PreviewMouseLeftButtonUp;
            this.mainWindow.Expense.MouseLeftButtonUp += Expense_PreviewMouseLeftButtonUp;
            this.mainWindow.Account.MouseLeftButtonUp += Account_PreviewMouseLeftButtonUp;
            this.mainWindow.Investment.MouseLeftButtonUp += Investment_PreviewMouseLeftButtonUp;
            this.mainWindow.Status.MouseLeftButtonUp += Status_PreviewMouseLeftButtonUp;
            this.mainWindow.PaymentCharge.MouseLeftButtonUp += PaymentCharge_PreviewMouseLeftButtonUp;
            //this.mainWindow.FixedExpense.MouseLeftButtonUp += FixedExpense_PreviewMouseLeftButtonUp;
            //this.mainWindow.LoanPercentage.MouseLeftButtonUp += LoanPercentage_PreviewMouseLeftButtonUp;
            this.mainWindow.Kind.MouseLeftButtonUp += Kind_PreviewMouseLeftButtonUp;
            this.mainWindow.Relation.MouseLeftButtonUp += Relation_PreviewMouseLeftButtonUp;
            this.mainWindow.Report.MouseLeftButtonUp += Report_PreviewMouseLeftButtonUp;
            this.mainWindow.CreditTerm.MouseLeftButtonUp += CreditTerm_PreviewMouseLeftButtonUp;
            this.mainWindow.Miscellaneous.MouseLeftButtonUp += Miscellaneous_MouseLeftButtonUp;

            this.mainWindow.DeclinedLoan.MouseLeftButtonUp += DeclinedLoan_MouseLeftButtonUp;
            this.mainWindow.ReleasedLoans.MouseLeftButtonUp += ReleasedLoans_MouseLeftButtonUp;
            this.mainWindow.dbconfigBtn.Click += DbconfigBtn_Click;
            this.mainWindow.changeuserbtn.Click += Changeuserbtn_Click;
            this.mainWindow.Import.MouseLeftButtonUp += Import_MouseLeftButtonUp;
            this.mainWindow.template_btn.Click += Template_btn_Click;
            this.mainWindow.CashDisbursement.MouseLeftButtonUp += CashDisbursement_MouseLeftButtonUp;


            this.mainWindow.Closing += MainWindow_Closing;
        }

        private void CashDisbursement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.CashDisbursement.DisbursementSearch().ShowDialog();
        }

        private void Template_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var current = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start("explorer.exe", string.Format(@"{0}\Documents", current));
        }

        private void Import_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Import.ImportForm().ShowDialog();
        }

        private void Miscellaneous_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Miscellaneous.MiscellaneousForm().ShowDialog();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void Changeuserbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginController.Instance.ShowLoginWindow();
            if (!LoginController.Instance.IsCancel)
            {
                this.mainWindow.welcomeTB.Text = string.Format("Welcome {0}", LoginController.Instance.CurrentLogin.DisplayName);
                setAccess(LoginController.Instance.CurrentLogin.AccountType);
            }
        }

        private void DbconfigBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(LoginController.Instance.CurrentLogin.AccountType == 0 || LoginController.Instance.CurrentLogin.AccountType == 1)
            {
                new DatabaseConfig().ShowDialog();
            }
        }

        private void ReleasedLoans_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Releasing.Released().ShowDialog();
        }

        private void DeclinedLoan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Approval.DeclinedLoan().ShowDialog();
        }

        private void Report_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Report.ReportMain().ShowDialog();
        }
        private void CreditTerm_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.CreditTerm.Terms().ShowDialog();
        }

        private void Relation_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Relation.RelationMain().ShowDialog();
        }

        private void Kind_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Kind.KindMain().ShowDialog();
        }

        private void LoanApplication_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var x = new Views.Search.ApplicantSearch();
            x.ShowDialog();
        }

        private void LoanApproval_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var x = new Views.Approval.ApprovalForm();
            x.ShowDialog();
        }

        private void Releasing_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var x = new Views.Releasing.Releasing();
            x.ShowDialog();
        }

        private void Payment_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Payment.Payment().ShowDialog();
        }

        private void Expense_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Expense.Expense().ShowDialog();
        }

        private void Account_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Account.Accounts().ShowDialog();
        }

        private void LetterTemplate_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Template.Templates().ShowDialog();
        }

        private void Investment_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Investment.Investments().ShowDialog();
        }

        private void Status_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.Status.Status().ShowDialog();
        }

        private void PaymentCharge_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.LatePaymentCharge.LatePaymentCharge().ShowDialog();
        }

        private void FixedExpense_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.FixedExpense.FixExpenses().ShowDialog();
        }

        private void LoanPercentage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new Views.LoanPercentage.Percentage().ShowDialog();
        }
    }
}
