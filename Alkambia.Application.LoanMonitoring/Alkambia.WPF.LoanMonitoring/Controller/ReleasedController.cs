using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.WPF.LoanMonitoring.Views.Releasing;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using System.Windows.Controls;
using Alkambia.WPF.LoanMonitoring.Controller.TemplateExtensions;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ReleasedController
    {
        static Model.Status StatusNew { get; set; }
        Released ReleasefForm { get; set; }
        Documents docuForm { get; set; }
        List<Model.Loan> Loans { get; set; }
        Model.Loan Loan { get; set; }

        public ReleasedController(Released ReleasefForm)
        {
            this.ReleasefForm = ReleasefForm;
            init();
            events();
        }
        void init()
        {
            StatusNew = StatusManager.GetName("Status.New");
        }
        void events()
        {
            ReleasefForm.search_buton.Click += Search_buton_Click;
            //ReleasefForm.ledger_buton.Click += v;
            ReleasefForm.docu_button.Click += Docu_button_Click;
            ReleasefForm.unrelease_button.Click += Unrelease_button_Click;
        }

        private void Unrelease_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ReleasefForm.ItemsDG.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to unreleased the loan?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var loan = ReleasefForm.ItemsDG.SelectedItem as Model.Loan;
                    if(loan.Payments.Count() > 0)
                    {
                        MessageBox.Show("Sorry I cant proceed on removing your request, this account has started its payment.", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {

                        //var approval = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval);
                        //approval.StatusID = StatusReleased.StatusID;
                        //ApprovalManager.SaveorUpdate(approval);
                        var approval = ApprovalManager.Get(loan.LoanApplication.Approvals.FirstOrDefault().ApprovalID);
                        approval.StatusID = StatusNew.StatusID;
                        ApprovalManager.SaveorUpdate(approval);
                        PaymentScheduleManager.Delete(loan.PaymentSchedules.ToList());
                        ScheduleTypeManager.Delete(loan.ScheduleTypes.ToList());
                        ReleaseManager.Delete(loan.Releases.ToList());
                        LoanManager.Delete(loan.LoanID);
                        MessageBox.Show("Successfuly remove from released.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        CommonQuery(loan);
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Please click in the table row before clicking the button.", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Docu_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ReleasefForm.ItemsDG.SelectedItem != null)
            {
                Loan = ReleasefForm.ItemsDG.SelectedItem as Model.Loan;
                //TemplateController.LoadContract(Loan, "promissorynote.doc");
                docuForm = new Documents();
                docuForm.close_buton.Click += DFormClose_buton_Click;
                docuForm.view_buton.Click += DFormView_buton_Click;
                docuForm.ShowDialog();
            }
        }

        private void DFormView_buton_Click(object sender, RoutedEventArgs e)
        {
            var val = (docuForm.DocumentsLB.SelectedItem as TextBlock).Text.Replace(" ","");

            switch(val)
            {
                case "PromissoryNote":
                    TemplateController.LoadContract(Loan, "promissorynote.doc");
                    break;
                case "Ledger":
                    TemplateController.LoadLedger(Loan);
                    break;
                case "AmortizationSchedule":
                    TemplateController.LoadAmortizationSchedule(Loan);
                    break;
                case "CashVoucher":
                    TemplateController.LoadCashVoucher(Loan, "cashvoucher.docx");
                    break;
                case "CheckVoucher":
                    TemplateController.LoadCashVoucher(Loan, "checkvoucher.docx");
                    break;
                case "Passbook":
                    TemplateController.LoadPassbook(Loan, "passbook.docx");
                    break;
                case "Disclosure":
                    TemplateController.LoadDisclosure(Loan, "disclosure.docx");
                    break;
                case "Reminder":
                    TemplateController.LoadReminder(Loan, "reminders.doc");
                    break;
                case "InformationSheet":
                    var p = new PersonalSheet(Loan);
                    p.Open();
                    break;
            }
        }

        private void DFormClose_buton_Click(object sender, RoutedEventArgs e)
        {
            docuForm.Close();
        }

        private void Ledger_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ReleasefForm.ItemsDG.SelectedItem != null)
            {
                var loan = ReleasefForm.ItemsDG.SelectedItem as Model.Loan;
                TemplateController.LoadLedger(loan);
            }
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CommonQuery();
        }
        private void CommonQuery()
        {
            Loans = LoanManager.GetByDisplayName(ReleasefForm.searchTB.Text).ToList();
            ReleasefForm.ItemsDG.ItemsSource = Loans;
            ReleasefForm.ItemsDG.Items.Refresh();
        }

        private void CommonQuery(Model.Loan loan)
        {
            Loans.Remove(loan);
            ReleasefForm.ItemsDG.Items.Refresh();
        }
    }
}
