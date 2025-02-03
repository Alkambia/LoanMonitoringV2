using Alkambia.WPF.LoanMonitoring.Views.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class PaymentController
    {

        Model.Payment PaymentClass { get; set; }
        Model.PaymentCharge PaymentCharge { get; set; }
        DateTime PaymentScheduleDate { get; set; }
        Model.Loan LoanClass { get; set; }
        Payment PaymentMain { get; set; }
        PaymentForm PaymentFormMain { get; set; }
        double Capital { get; set; }
        double Interest { get; set; }

        List<Model.Payment> Payments { get; set; }

        List<Model.Account> Collectors { get; set; }
        
        public PaymentController(Payment paymentMain)
        {
            this.PaymentMain = paymentMain;
            PaymentCharge = PaymentChargeManager.Get();
            events();
            init();
        }

        private void init()
        {
            Collectors = AccountManager.Get(5).ToList();
        }

        private void events()
        {
            PaymentMain.search_buton.Click += Search_buton_Click;
            PaymentMain.payment_button.Click += Payment_button_Click;
            if(LoginController.Instance.CurrentLogin.AccountType.Equals(0))
            {
                PaymentMain.edit_button.Click += Edit_button_Click;
                PaymentMain.del_button.Click += Del_button_Click;
            }
            
        }

        private void Del_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PaymentMain.PaymentsDG.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to delete the payment data?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        var entry = PaymentMain.PaymentsDG.SelectedItem as Model.Payment;
                        PaymentManager.Delete(entry.PaymentID);
                        Payments.Remove(entry);
                        PaymentMain.PaymentsDG.Items.Refresh();
                        MessageBox.Show("Payment successfully removed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (PaymentMain.PaymentsDG.SelectedIndex != -1)
            {
                PaymentClass = PaymentMain.PaymentsDG.SelectedItem as Model.Payment;
                LoanClass = PaymentClass.Loan;
                Capital = LoanClass.Principal;
                Interest = LoanClass.Interest;

                double amortization = LoanClass.Amortization;

                double paymentSum = LoanClass.Payments != null ? LoanClass.Payments.Sum(x => x.Amount) : 0;

                var selectedCollector = Collectors.First(x => x.AccountID == PaymentClass.CollertorID);
                PaymentFormMain = new PaymentForm();
                PaymentFormMain.DataContext = PaymentClass;
                PaymentFormMain.CollectorCB.ItemsSource = Collectors;
                PaymentFormMain.CollectorCB.SelectedItem = selectedCollector;
                PaymentFormMain.ammortiztionTB.Text = string.Format("{0}", amortization);
                PaymentFormMain.balanceTB.Text = string.Format("{0}", ((Capital + Interest) - paymentSum));
                PaymentFormMain.paymentTB.KeyUp += PaymentTB_KeyUp;
                PaymentFormMain.chargeTB.KeyUp += PaymentChargeTB_KeyUp;
                PaymentFormMain.paymentdateDP.SelectedDateChanged += PaymentdateDP_SelectedDateChanged;
                PaymentFormMain.cancelBtn.Click += PaymentCancelBtn_Click;
                PaymentFormMain.addBtn.Click += PaymentAddBtn_Click;
                PaymentFormMain.addBtn.Content = "edit";

                if(LoginController.Instance.CurrentLogin.AccountType != 0)
                {
                    PaymentFormMain.paymentdateDP.IsEnabled = false;
                }

                PaymentFormMain.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please click something on the row.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Payment_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PaymentMain.accountCodeTB.Text.Trim()))
            {
                try
                {
                    LoanClass = LoanManager.Get(PaymentMain.accountCodeTB.Text.Trim());
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
                        //var maxDate = LoanClass.Payments.Max(x => x.PaymentScheduleDate);
                        //PaymentScheduleDate = LoanClass.PaymentSchedules.Where(x => x.Schedule > maxDate).Min(x => x.Schedule);
                        PaymentScheduleDate = Alkambia.App.LoanMonitoring.Helper.ScheduleCreator.PaymentSchedule(LoanClass);
                    }

                    if (PaymentScheduleDate.AddMonths(1) < DateTime.Now)
                    {
                        latePaymentCharge = (LoanClass.Principal * (PaymentCharge.Percentage / 100));
                    }
                    //get max date compare with schedule less than paymentmax date

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
                    //PaymentFormMain.paymentSchedDP.SelectedDate = PaymentScheduleDate;
                    PaymentFormMain.ammortiztionTB.Text = string.Format("{0}", amortization);
                    PaymentFormMain.balanceTB.Text = string.Format("{0}", ((Capital + Interest) - paymentSum));
                    PaymentFormMain.paymentTB.KeyUp += PaymentTB_KeyUp;
                    PaymentFormMain.chargeTB.KeyUp += PaymentChargeTB_KeyUp;
                    PaymentFormMain.paymentdateDP.SelectedDateChanged += PaymentdateDP_SelectedDateChanged;
                    PaymentFormMain.cancelBtn.Click += PaymentCancelBtn_Click;
                    PaymentFormMain.addBtn.Click += PaymentAddBtn_Click;
                    PaymentFormMain.ShowDialog();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill the search text box with account code.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PaymentdateDP_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            double latePaymentCharge = 0;
            if (PaymentScheduleDate < PaymentFormMain.paymentdateDP.SelectedDate.Value)
            {
                latePaymentCharge = (LoanClass.Principal * (PaymentCharge.Percentage / 100));
            }
            if(string.IsNullOrEmpty(PaymentFormMain.paymentTB.Text.Trim()))
            {
                PaymentFormMain.paymentTB.Text = string.Format("{0}",0);
            }
            var payment = double.Parse(PaymentFormMain.paymentTB.Text.Trim());
            PaymentFormMain.chargeTB.Text = string.Format("{0}",latePaymentCharge);
            PaymentFormMain.totalPaymentTB.Text = string.Format("{0}", (payment + latePaymentCharge));
        }

        private void PaymentAddBtn_Click(object sender, RoutedEventArgs e)
        {
            //Add validation logic
            try
            {
                var paymentContext = PaymentFormMain.DataContext as Model.Payment;
                paymentContext.CollertorID = (PaymentFormMain.CollectorCB.SelectedItem as Model.Account).AccountID;
                if(PaymentFormMain.addBtn.Content.Equals("edit"))
                {
                    PaymentManager.SaveorUpdate(paymentContext);
                    PaymentMain.PaymentsDG.Items.Refresh();
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

        private void PaymentTB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(PaymentFormMain.paymentTB.Text.Trim()))
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
                //totalPaymentTB
                //Math.Round(paymentNoInteres, 12);
                PaymentFormMain.PrincipalTB.Text = string.Format("{0}", Math.Round(paymentNoInteres, 0));
                PaymentFormMain.InterestTB.Text = string.Format("{0}", Math.Round(interest, 0));
                PaymentFormMain.balanceTB.Text = string.Format("{0}", (((Capital + Interest)- paymentSum) - payment));
                PaymentFormMain.totalPaymentTB.Text = string.Format("{0}", (payment+ charge));

            }
            catch(Exception ee)
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

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Payments = PaymentManager.Get(PaymentMain.accountCodeTB.Text).ToList();
            PaymentMain.PaymentsDG.ItemsSource = Payments;
        }
        
    }
}
