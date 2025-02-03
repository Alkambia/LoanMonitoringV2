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
    public class PaymentChargeController
    {
        Views.LatePaymentCharge.LatePaymentCharge PaymentChargeMain { get; set; }
        Model.PaymentCharge PaymentChargeClass { get; set; }
        bool IsNew { get; set; }
        public PaymentChargeController(Views.LatePaymentCharge.LatePaymentCharge PaymentChargeMain)
        {
            this.PaymentChargeMain = PaymentChargeMain;
            events();
            init();
        }
        private void events()
        {
            PaymentChargeMain.cancelbtn.Click += Cancelbtn_Click;
            PaymentChargeMain.savebtn.Click += Savebtn_Click;
        }

        private void Savebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add validatio later
            if(IsNew)
            {
                PaymentChargeManager.Add(PaymentChargeClass);
            }
            else
            {
                PaymentChargeManager.SaveorUpdate(PaymentChargeClass);
            }
            MessageBox.Show("Late Payment Charge Setup Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            PaymentChargeMain.Close();
        }

        private void Cancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PaymentChargeMain.Close();
        }

        private void init()
        {
            IsNew = false;
            PaymentChargeClass = PaymentChargeManager.Get();
            if(PaymentChargeClass == null)
            {
                IsNew = true;
                PaymentChargeClass = new Model.PaymentCharge() {
                    PaymentChargeID = Guid.NewGuid(),
                    Percentage = 0,
                    Name = "LatePaymentCharge",
                    DisplayName = "Late Payment Charge"
                };

            }
            PaymentChargeMain.DataContext = PaymentChargeClass;
        }
    }
}
