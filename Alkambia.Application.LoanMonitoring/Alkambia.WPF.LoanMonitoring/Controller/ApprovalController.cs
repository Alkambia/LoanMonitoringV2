using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.WPF.LoanMonitoring.Views.Approval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.Enums;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ApprovalController
    {
        ApprovalForm ApprovalForm1 { get; set; }
        Remarks RemarksForm { get; set; }
        List<Model.LoanApplication> Applications { get; set; }

        Model.Status StatusDeclined { get; set; }
        Model.Status StatusApproved { get; set; }
        Model.Status StatusNew { get; set; }
        public ApprovalController(ApprovalForm form)
        {
            ApprovalForm1 = form;
            DataInit();
            Init();
            events();
        }

        private void events()
        {
            ApprovalForm1.search_buton.Click += Search_buton_Click;
            ApprovalForm1.approve_buton.Click += Approve_buton_Click;
            ApprovalForm1.decline_button.Click += Decline_button_Click;
            ApprovalForm1.profile_button.Click += Profile_button_Click;
        }
        private void DataInit()
        {
            StatusNew = StatusManager.GetName("Status.New");
            StatusDeclined = StatusManager.GetName("Status.Declined");
            StatusApproved = StatusManager.GetName("Status.Approved");
            Applications = LoanApplicationManager.GetUnApproveLoans().ToList();
        }
        private void Init()
        {
            ApprovalForm1.approvalDG.ItemsSource = Applications;
        }

        private void Profile_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ApprovalForm1.approvalDG.SelectedItem != null)
            {
                var person = (ApprovalForm1.approvalDG.SelectedItem as Model.LoanApplication).PersonalData;
                ApprovalForm1.Hide();
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Read);
                dummy.ShowDialog();
                ApprovalForm1.Show();
            }
            
        }

        private void Decline_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add Note later for declined applications
            if (ApprovalForm1.approvalDG.SelectedItem != null)
            {
                if(MessageBox.Show("Are you sure you want to declined the applicant?","Notification",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RemarksForm = new Remarks();
                    RemarksForm.savebtn.Click += RemarksSavebtn_Click;
                    RemarksForm.cancelbtn.Click += RemarksCancelbtn_Click;
                    var applicationLoan = ApprovalForm1.approvalDG.SelectedItem as Model.LoanApplication;
                    RemarksForm.DataContext = applicationLoan;
                    RemarksForm.ShowDialog();
                    
                }
            }
        }

        private void RemarksCancelbtn_Click(object sender, RoutedEventArgs e)
        {
            RemarksForm.Close();
        }

        private void RemarksSavebtn_Click(object sender, RoutedEventArgs e)
        {
            var applicationLoan = RemarksForm.DataContext as Model.LoanApplication;
            applicationLoan.StatusID = StatusDeclined.StatusID;
            LoanApplicationManager.SaveorUpdate(applicationLoan);
            CommonQuery(applicationLoan);
            RemarksForm.Close();
        }

        private void Approve_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var applicationLoan = ApprovalForm1.approvalDG.SelectedItem as Model.LoanApplication;
            applicationLoan.StatusID = StatusApproved.StatusID;
            LoanApplicationManager.SaveorUpdate(applicationLoan);
            ApprovalManager.Add(new Model.Approval() {
                ApprovalID = Guid.NewGuid(),
                StatusID = StatusNew.StatusID,
                ApprovalDate = DateTime.Now,
                LoanApplicationID = applicationLoan.LoanApplicationID,
            });
            MessageBox.Show("Loan Approve", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            CommonQuery(applicationLoan);
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonQuery();
        }
        private void CommonQuery()
        {
            Applications = LoanApplicationManager.GetUnApproveLoans(ApprovalForm1.searchTB.Text.Trim()).ToList();
            ApprovalForm1.approvalDG.ItemsSource = Applications;
            ApprovalForm1.approvalDG.Items.Refresh();
        }

        private void CommonQuery(Model.LoanApplication application)
        {
            Applications.Remove(application);
            ApprovalForm1.approvalDG.Items.Refresh();
        }
    }
}
