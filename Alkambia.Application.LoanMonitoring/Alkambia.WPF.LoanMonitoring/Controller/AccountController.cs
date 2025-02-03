using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.WPF.LoanMonitoring.Views.Account;
using model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class AccountController
    {
        Accounts accountForm { get; set; }
        AccountForm form { get; set; }
        List<model.Account> Accounts { get; set; }
        model.Account Account { get; set; }
        public AccountController(Accounts accountForm)
        {
            this.accountForm = accountForm;
            events();
        }

        void events()
        {
            accountForm.search_buton.Click += Search_buton_Click;
            accountForm.add_button.Click += Add_button_Click;
            accountForm.edit_button.Click += Edit_button_Click;
            accountForm.del_button.Click += Del_button_Click;
        }

        private void Del_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (accountForm.accountDG.SelectedIndex != -1)
            {
                if(MessageBox.Show("Are you sure you want to delete this?","Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Account = accountForm.accountDG.SelectedItem as model.Account;
                    AccountManager.Delete(Account.AccountID);
                    Accounts.Remove(Account);
                    accountForm.accountDG.Items.Refresh();
                    MessageBox.Show("Account Deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Edit_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(accountForm.accountDG.SelectedIndex != -1)
            {
                Account = accountForm.accountDG.SelectedItem as model.Account;
                form = new AccountForm();
                form.DataContext = Account;
                form.cancelbtn.Click += formCancelbtn_Click;
                form.addbtn.Click += formAddbtn_Click;
                form.addbtn.Content = "edit";
                form.ShowDialog();
            }
        }

        private void Add_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Account = new model.Account()
            {
                AccountID = Guid.NewGuid(),
                Name = string.Empty,
                DisplayName = string.Empty,
                Description = string.Empty,
                CreatedDate = DateTime.Now,
                UserName = string.Empty,
                Password = string.Empty
            };
            form = new AccountForm();
            form.DataContext = Account;
            form.cancelbtn.Click += formCancelbtn_Click;
            form.addbtn.Click += formAddbtn_Click;
            form.ShowDialog();
        }

        private void formAddbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                
                if (form.addbtn.Content.Equals("edit"))
                {
                    AccountManager.SaveorUpdate(Account);
                }
                else
                {
                    AccountManager.Add(Account);
                    Accounts.Add(Account);
                }
                
                MessageBox.Show("Account Saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                form.Close();
            }
            catch
            {
                MessageBox.Show("Error while saving, please check your inputs, if error continue please contact you system adminitrator", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void formCancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            form.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Accounts = AccountManager.Get(accountForm.searchTB.Text).ToList();
            accountForm.accountDG.ItemsSource = Accounts;
        }
    }
}
