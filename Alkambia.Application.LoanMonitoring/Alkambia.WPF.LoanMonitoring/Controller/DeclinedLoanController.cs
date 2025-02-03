using System;
using System.Collections.Generic;
using System.Linq;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.WPF.LoanMonitoring.Views.Approval;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using Alkambia.App.LoanMonitoring.Enums;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class DeclinedLoanController
    {
        DeclinedLoan DeclinedLoan { get; set; }
        List<Model.LoanApplication> Applications { get; set; }
        Model.Status StatusDeclined { get; set; }
        Model.Status StatusNew { get; set; }
        Remarks RemarksForm { get; set; }

        public DeclinedLoanController(DeclinedLoan DeclinedLoan)
        {
            this.DeclinedLoan = DeclinedLoan;
            init();
            events();
        }

        void init()
        {
            StatusNew = StatusManager.GetName("Status.New");
            StatusDeclined = StatusManager.GetName("Status.Declined");
        }

        void events()
        {
            DeclinedLoan.search_buton.Click += Search_buton_Click;
            DeclinedLoan.remarks_buton.Click += Remarks_buton_Click;
            DeclinedLoan.profile_button.Click += Profile_button_Click;
            DeclinedLoan.undeclined_button.Click += Undeclined_button_Click;
        }

        private void Undeclined_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DeclinedLoan.ItemsDG.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to undeclined the applicant?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var applicationLoan = DeclinedLoan.ItemsDG.SelectedItem as Model.LoanApplication;
                    applicationLoan.StatusID = StatusNew.StatusID;
                    LoanApplicationManager.SaveorUpdate(applicationLoan);
                    CommonQuery(applicationLoan);
                }
            }
            else
            {
                MessageBox.Show("Please click in the table row before clicking the button.", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
                
        }

        private void Profile_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DeclinedLoan.ItemsDG.SelectedItem != null)
            {
                var person = (DeclinedLoan.ItemsDG.SelectedItem as Model.LoanApplication).PersonalData;
                DeclinedLoan.Hide();
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Read);
                dummy.ShowDialog();
                DeclinedLoan.Show();
            }
            else
            {
                MessageBox.Show("Please click in the table row before clicking the button.", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            
        }

        private void Remarks_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DeclinedLoan.ItemsDG.SelectedItem != null)
            {
                RemarksForm = new Remarks();
                RemarksForm.DataContext = DeclinedLoan.ItemsDG.SelectedItem as Model.LoanApplication;
                RemarksForm.cancelbtn.Visibility = System.Windows.Visibility.Hidden;
                RemarksForm.savebtn.Content = "Close";
                RemarksForm.savebtn.Click += RemarksSavebtn_Click;
                RemarksForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please click in the table row before clicking the button.", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
        }

        private void RemarksSavebtn_Click(object sender, RoutedEventArgs e)
        {
            RemarksForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonQuery();
        }
        private void CommonQuery()
        {
            Applications = LoanApplicationManager.GetDeclinedLoans(DeclinedLoan.searchTB.Text.Trim()).ToList();
            DeclinedLoan.ItemsDG.ItemsSource = Applications;
            DeclinedLoan.ItemsDG.Items.Refresh();
        }

        private void CommonQuery(Model.LoanApplication application)
        {
            Applications.Remove(application);
            DeclinedLoan.ItemsDG.Items.Refresh();
        }
    }
}
