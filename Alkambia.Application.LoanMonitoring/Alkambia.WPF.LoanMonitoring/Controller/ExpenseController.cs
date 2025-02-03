using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views = Alkambia.WPF.LoanMonitoring.Views;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ExpenseController
    {
        Views.Expense.Expense ExpenseMain { get; set; }
        Views.Expense.ExpenseForm ExpenseForm { get; set; }

        Model.Expense ExpenseClass { get; set; }
        List<Model.Expense> Expenses { get; set; }
        public ExpenseController(Views.Expense.Expense expensemain)
        {
            ExpenseMain = expensemain;
            events();
        }
        public void events()
        {
            ExpenseMain.search_buton.Click += Search_buton_Click;
            ExpenseMain.expense_button.Click += Expense_button_Click;
            
            if (LoginController.Instance.CurrentLogin.AccountType.Equals(0))
            {
                ExpenseMain.edit_button.Click += Edit_button_Click;
                ExpenseMain.delete_button.Click += Delete_button_Click;
            }
        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExpenseMain.ExpenseDG.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to delete the data?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        ExpenseClass = ExpenseMain.ExpenseDG.SelectedItem as Model.Expense;
                        ExpenseManager.Delete(ExpenseClass.ExpenseID);
                        Expenses.Remove(ExpenseClass);
                        ExpenseMain.ExpenseDG.Items.Refresh();
                        MessageBox.Show("Data successfully removed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please click something on the row.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("An error occured during deleting the payment, please contact your developer", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            if (ExpenseMain.ExpenseDG.SelectedIndex != -1)
            {
                ExpenseClass = ExpenseMain.ExpenseDG.SelectedItem as Model.Expense;
                ExpenseForm = new Views.Expense.ExpenseForm();
                ExpenseForm.DataContext = ExpenseClass;
                ExpenseForm.cancelbtn.Click += ExpenseFormCancelbtn_Click;
                ExpenseForm.addbtn.Click += ExpenseFormAddbtn_Click;
                ExpenseForm.addbtn.Content = "edit";
                ExpenseForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please click something on the row.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Expense_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExpenseClass = new Model.Expense()
            {
                ExpenseID = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                ExpenseDate = DateTime.Now,
                Amount = 0,
                Name = string.Empty,
                DisplayName = string.Empty,
                Description = string.Empty
            };

            ExpenseForm = new Views.Expense.ExpenseForm();
            ExpenseForm.DataContext = ExpenseClass;
            ExpenseForm.cancelbtn.Click += ExpenseFormCancelbtn_Click;
            ExpenseForm.addbtn.Click += ExpenseFormAddbtn_Click;
            ExpenseForm.ShowDialog();
        }

        private void ExpenseFormAddbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try {
                //Add validation
                ExpenseClass.ExpenseDate = ExpenseClass.ExpenseDate.Date;
                if(ExpenseForm.addbtn.Content.Equals("edit"))
                {
                    ExpenseManager.SaveorUpdate(ExpenseClass);
                    ExpenseMain.ExpenseDG.Items.Refresh();
                }
                else
                {
                    ExpenseManager.Add(ExpenseClass);
                }
            
                MessageBox.Show("Expense Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ExpenseForm.Close();
            }
            catch
            {
                MessageBox.Show("An Error occur while saving, please check your entry for unfilled data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
}

        private void ExpenseFormCancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExpenseForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ExpenseMain.ExpenseDateDP.SelectedDate.HasValue)
            {
                Expenses = ExpenseManager.Get(ExpenseMain.ExpenseDateDP.SelectedDate.Value.Date).ToList();
                ExpenseMain.ExpenseDG.ItemsSource = Expenses;
            }
            
        }

    }
}
