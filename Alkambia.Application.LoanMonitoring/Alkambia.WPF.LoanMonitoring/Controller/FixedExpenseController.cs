using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class FixedExpenseController
    {
        Views.FixedExpense.FixExpenses FixExpenseMain { get; set; }
        Views.FixedExpense.FixExpenseForm FixExpenseForm { get; set; }
        Model.FixedExpense FixExpenseClass { get; set; }
        List<Model.FixedExpense> FixExpenses { get; set; }

        public FixedExpenseController(Views.FixedExpense.FixExpenses FixExpenseMain)
        {
            this.FixExpenseMain = FixExpenseMain;
            events();
        }
        public void events()
        {
            FixExpenseMain.search_buton.Click += Search_buton_Click;
            FixExpenseMain.add_button.Click += Add_button_Click;
            FixExpenseMain.edit_button.Click += Edit_button_Click;
        }

        private void Edit_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(FixExpenseMain.expenseDG.SelectedIndex > -1)
            {
                FixExpenseClass = FixExpenseMain.expenseDG.SelectedItem as Model.FixedExpense;
                FixExpenseForm = new Views.FixedExpense.FixExpenseForm();
                FixExpenseForm.DataContext = FixExpenseClass;
                FixExpenseForm.addbtn.Content = "save";
                FixExpenseForm.addbtn.Click += FormAddbtn_Click;
                FixExpenseForm.cancelbtn.Click += FormCancelbtn_Click;
                FixExpenseForm.ShowDialog();
            }
        }

        private void Add_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixExpenseClass = new Model.FixedExpense()
            {
                FixedExpenseID = Guid.NewGuid(),
                Amount = 0,
                Name = string.Empty,
                DisplayName = string.Empty,
                CreatedDate = DateTime.Now,
                BillingDate = DateTime.Now
            };
            FixExpenseForm = new Views.FixedExpense.FixExpenseForm();
            FixExpenseForm.DataContext = FixExpenseClass;
            FixExpenseForm.addbtn.Click += FormAddbtn_Click;
            FixExpenseForm.cancelbtn.Click += FormCancelbtn_Click;
            FixExpenseForm.ShowDialog();
        }

        private void FormCancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixExpenseForm.Close();
        }

        private void FormAddbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add validation logic
            if(!FixExpenseForm.addbtn.Content.Equals("save"))
            {
                FixedExpenseManager.Add(FixExpenseClass);
                FixExpenseMain.expenseDG.Items.Refresh();
                FixExpenses.Add(FixExpenseClass);
            }
            else
            {
                FixedExpenseManager.SaveorUpdate(FixExpenseClass);
            }
            MessageBox.Show("Expense Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            FixExpenseForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixExpenses = FixedExpenseManager.Get(FixExpenseMain.searchTB.Text).ToList();
            FixExpenseMain.expenseDG.ItemsSource = FixExpenses;
        }
    }
}
