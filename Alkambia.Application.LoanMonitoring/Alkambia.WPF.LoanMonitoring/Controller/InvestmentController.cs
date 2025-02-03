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
    public class InvestmentController
    {
        Views.Investment.Investments InvestmentMain { get; set; }
        Views.Investment.InvestmentForm InvForm { get; set; }
        List<Model.Investment> Investments { get; set; }
        Model.Investment InvestmentClass { get; set; }
        public InvestmentController(Views.Investment.Investments InvMain)
        {
            InvestmentMain = InvMain;
            events();
        }
        public void events()
        {
            InvestmentMain.search_buton.Click += Search_buton_Click;
            InvestmentMain.investment_button.Click += Investment_button_Click;
            InvestmentMain.investment_del.Click += Investment_del_Click;
            InvestmentMain.inv_edit_btn.Click += Investment_edit_Click;
        }

        private void Investment_edit_Click(object sender, RoutedEventArgs e)
        {
            if (InvestmentMain.inverstmentDG.SelectedIndex != -1)
            {
                InvestmentClass = InvestmentMain.inverstmentDG.SelectedItem as Model.Investment;

                InvForm = new Views.Investment.InvestmentForm();
                InvForm.cancelbtn.Click += InvFormCancelbtn_Click;
                InvForm.addbtn.Click += InvFormAddbtn_Click;
                InvForm.addbtn.Content = "edit";
                InvForm.DataContext = InvestmentClass;
                InvForm.ShowDialog();

            }
            else
            {
                MessageBox.Show("Please select one item on row?", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Investment_del_Click(object sender, RoutedEventArgs e)
        {
            if(InvestmentMain.inverstmentDG.SelectedIndex != -1)
            {
                if(MessageBox.Show("Are you sure you want to delete this data?","Warning",MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var investment = InvestmentMain.inverstmentDG.SelectedItem as Model.Investment;
                    InvestmentManager.Delete(investment.InvestmentID);
                    MessageBox.Show("Data deleted!");
                    Investments.Remove(investment);
                    InvestmentMain.inverstmentDG.Items.Refresh();
                }
            }
        }

        private void Investment_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InvForm = new Views.Investment.InvestmentForm();
            InvForm.cancelbtn.Click += InvFormCancelbtn_Click;
            InvForm.addbtn.Click += InvFormAddbtn_Click;
            InvestmentClass = new Model.Investment() {
                InvestmentID = Guid.NewGuid(),
                Name = string.Empty,
                Description = string.Empty,
                DisplayName = string.Empty,
                Capital = 0,
                CreatedDate = DateTime.Now,
                IsApproved = false
            };
            InvForm.DataContext = InvestmentClass;
            InvForm.ShowDialog();
        }

        private void InvFormAddbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (InvForm.addbtn.Content.Equals("edit"))
                {
                    InvestmentManager.SaveorUpdate(InvestmentClass);
                }
                else
                {
                    InvestmentManager.Add(InvestmentClass);
                }
                MessageBox.Show("Investment Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                InvForm.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Please check your input fields if you insert a correct data, if error continue please call your system administrator, Error: {0}",ex.Message));
            }
            
        }

        private void InvFormCancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InvForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Investments = InvestmentManager.Get(InvestmentMain.diplaynameTB.Text).ToList();
            InvestmentMain.inverstmentDG.ItemsSource = Investments;
        }
        
    }
}
