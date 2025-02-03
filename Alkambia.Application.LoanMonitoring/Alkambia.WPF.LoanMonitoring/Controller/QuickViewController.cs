using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using Alkambia.App.LoanMonitoring.Enums;
using Alkambia.WPF.LoanMonitoring.Views.Payment;
using System.Windows.Input;
using Alkambia.WPF.LoanMonitoring.Controller.ReportExtensions;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class QuickViewController
    {
        Views.QuickLinks.QuickView quickView { get; set; }
        List<Model.Loan> loans { get; set; }

        double Capital { get; set; }
        double Interest { get; set; }
        DateTime PaymentScheduleDate { get; set; }
        Model.PaymentCharge PaymentCharge { get; set; }
        Model.Payment PaymentClass { get; set; }
        Model.Loan LoanClass { get; set; }
        PaymentForm PaymentFormMain { get; set; }
        List<Model.Account> Collectors { get; set; }

        public QuickViewController(Views.QuickLinks.QuickView quickView)
        {
            this.quickView = quickView;
            events();
            init();
        }
        private void init()
        {
            PaymentCharge = PaymentChargeManager.Get();
            Collectors = AccountManager.Get(5).ToList();
        }
        private void events()
        {
            quickView.search_btn.Click += Search_btn_Click;
            quickView.application_btn.Click += Application_btn_Click;
            quickView.payment_btn.Click += Payment_btn_Click;
            quickView.payment_sum_btn.Click += Payment_sum_btn_Click;
        }

        private void Payment_sum_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (quickView.SearchDG.SelectedIndex > -1)
            {
                LoanClass = quickView.SearchDG.SelectedItem as Model.Loan;
                new PaymentSummaryReport(LoanClass);
            }
            else
            {
                MessageBox.Show("Please click a borrower on the grid to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            quickView.SearchDG.SelectedIndex = -1;
        }

        private void Payment_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (quickView.SearchDG.SelectedIndex > -1)
            {
                LoanClass = quickView.SearchDG.SelectedItem as Model.Loan;
                try
                {
                    Capital = LoanClass.Principal;
                    Interest = LoanClass.Interest;

                    double latePaymentCharge = 0;

                    double amortization = LoanClass.Amortization;
                    if (LoanClass.Payments == null || LoanClass.Payments.Count().Equals(0))
                    {
                        PaymentScheduleDate = LoanClass.PaymentSchedules.Min(x => x.Schedule);

                    }
                    else
                    {
                        PaymentScheduleDate = Alkambia.App.LoanMonitoring.Helper.ScheduleCreator.PaymentSchedule(LoanClass);
                    }

                    if (PaymentScheduleDate.AddMonths(1) < DateTime.Now)
                    {
                        latePaymentCharge = (LoanClass.Principal * (PaymentCharge.Percentage / 100));
                    }

                    PaymentClass = new Model.Payment()
                    {
                        PaymentID = Guid.NewGuid(),
                        Date = DateTime.Now,
                        ORNumber = string.Empty,
                        Principal = 0,
                        Interest = 0,
                        Charge = latePaymentCharge,
                        Amount = 0,
                        LoanID = LoanClass.LoanID,
                        Loan = LoanClass,
                        PaymentScheduleDate = PaymentScheduleDate
                    };

                    double paymentSum = LoanClass.Payments != null ? LoanClass.Payments.Sum(x => x.Amount) : 0;

                    PaymentFormMain = new PaymentForm();
                    PaymentFormMain.DataContext = PaymentClass;
                    PaymentFormMain.CollectorCB.ItemsSource = Collectors;
                    PaymentFormMain.ammortiztionTB.Text = string.Format("{0}", amortization);
                    PaymentFormMain.balanceTB.Text = string.Format("{0}", ((Capital + Interest) - paymentSum));
                    PaymentFormMain.paymentTB.KeyUp += PaymentTB_KeyUp;
                    PaymentFormMain.chargeTB.KeyUp += PaymentChargeTB_KeyUp;
                    PaymentFormMain.paymentdateDP.SelectedDateChanged += PaymentdateDP_SelectedDateChanged;
                    PaymentFormMain.cancelBtn.Click += PaymentCancelBtn_Click;
                    PaymentFormMain.addBtn.Click += PaymentAddBtn_Click;

                    if (LoginController.Instance.CurrentLogin.AccountType != 0)
                    {
                        PaymentFormMain.paymentdateDP.IsEnabled = false;
                    }

                    PaymentFormMain.ShowDialog();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please click a borrower on the grid to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            quickView.SearchDG.SelectedIndex = -1;
        }

        private void PaymentAddBtn_Click(object sender, RoutedEventArgs e)
        {
            //Add validation logic
            try
            {
                var paymentContext = PaymentFormMain.DataContext as Model.Payment;
                paymentContext.CollertorID = (PaymentFormMain.CollectorCB.SelectedItem as Model.Account).AccountID;
                if (PaymentFormMain.addBtn.Content.Equals("edit"))
                {
                    PaymentManager.SaveorUpdate(paymentContext);
                }
                else
                {
                    PaymentManager.Add(paymentContext);
                }

                MessageBox.Show("Payment Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                PaymentFormMain.Close();
            }
            catch
            {
                MessageBox.Show("An Error occur while saving, please check your entry for unfilled data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PaymentCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            PaymentFormMain.Close();
        }

        private void PaymentdateDP_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            double latePaymentCharge = 0;
            if (PaymentScheduleDate < PaymentFormMain.paymentdateDP.SelectedDate.Value)
            {
                latePaymentCharge = (LoanClass.Principal * (PaymentCharge.Percentage / 100));
            }
            if (string.IsNullOrEmpty(PaymentFormMain.paymentTB.Text.Trim()))
            {
                PaymentFormMain.paymentTB.Text = string.Format("{0}", 0);
            }
            var payment = double.Parse(PaymentFormMain.paymentTB.Text.Trim());
            PaymentFormMain.chargeTB.Text = string.Format("{0}", latePaymentCharge);
            PaymentFormMain.totalPaymentTB.Text = string.Format("{0}", (payment + latePaymentCharge));
        }
        private void PaymentTB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PaymentFormMain.paymentTB.Text.Trim()))
                {
                    PaymentFormMain.paymentTB.Text = "0";
                }
                double paymentSum = LoanClass.Payments != null ? LoanClass.Payments.Sum(x => x.Amount) : 0;
                var payment = double.Parse(PaymentFormMain.paymentTB.Text.Trim());
                var paymentNoInteres = (Capital / (Capital + Interest)) * payment;
                var interest = payment - paymentNoInteres;
                var charge = double.Parse(PaymentFormMain.chargeTB.Text);
                PaymentClass.Principal = Math.Round(paymentNoInteres, 0);
                PaymentClass.Interest = Math.Round(interest, 0);
                PaymentFormMain.PrincipalTB.Text = string.Format("{0}", Math.Round(paymentNoInteres, 0));
                PaymentFormMain.InterestTB.Text = string.Format("{0}", Math.Round(interest, 0));
                PaymentFormMain.balanceTB.Text = string.Format("{0}", (((Capital + Interest) - paymentSum) - payment));
                PaymentFormMain.totalPaymentTB.Text = string.Format("{0}", (payment + charge));

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PaymentChargeTB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PaymentFormMain.chargeTB.Text.Trim()))
                {
                    PaymentFormMain.chargeTB.Text = "0";
                }
                var payment = double.Parse(PaymentFormMain.paymentTB.Text.Trim());
                var charge = double.Parse(PaymentFormMain.chargeTB.Text);
                PaymentFormMain.totalPaymentTB.Text = string.Format("{0}", (payment + charge));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Application_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var person = new Model.PersonalData();
            person.DateOfBirth = DateTime.Now;
            if (quickView.SearchDG.SelectedIndex > -1)
            {
                var loan = quickView.SearchDG.SelectedItem as Model.Loan;
                person = loan.LoanApplication.PersonalData;
            }
            if (LoanManager.WithUnpaidLoan(person.PersonalDataID))
            {
                MessageBox.Show("Sorry, the person cant apply until loan is fully paid.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Add);
                dummy.ShowDialog();
            }
            quickView.SearchDG.SelectedIndex = -1;
        }

        private void Search_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            loans = LoanManager.GetByDisplayNameOrAccountCode(quickView.search_tb.Text).ToList();
            quickView.SearchDG.ItemsSource = loans;
        }
    }
}
