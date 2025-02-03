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
    public class LoanPercentageController
    {
        Views.LoanPercentage.Percentage PercentageMain { get; set; }
        Model.LoanPercentage LoanPercentageClass { get; set; }
        bool IsNew { get; set; }
        public LoanPercentageController(Views.LoanPercentage.Percentage PercentageMain)
        {
            this.PercentageMain = PercentageMain;
            events();
            init();
        }
        private void events()
        {
            PercentageMain.savebtn.Click += Savebtn_Click;
            PercentageMain.cancelbtn.Click += Cancelbtn_Click;
        }
        private void init()
        {
            IsNew = false;
            LoanPercentageClass = LoanPercentageManager.Get();
            if(LoanPercentageClass == null)
            {
                IsNew = true;
                LoanPercentageClass = new Model.LoanPercentage()
                {
                    LoanPercentageID = Guid.NewGuid(),
                    Name = "LoanPercentage",
                    DisplayName = "Loan Percentage",
                    Percentage = 30,
                    Description = "Total percentage of loan",
                    CreatedDate = DateTime.Now
                };
            }
            
            //PercentageMain = new Views.LoanPercentage.Percentage();
            PercentageMain.DataContext = LoanPercentageClass;
        }

        private void Cancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PercentageMain.Close();
        }

        private void Savebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add validatio later
            if (IsNew)
            {
                LoanPercentageManager.Add(LoanPercentageClass);
            }
            else
            {
                LoanPercentageManager.SaveorUpdate(LoanPercentageClass);
            }
            MessageBox.Show("Late Payment Charge Setup Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            PercentageMain.Close();

        }
    }
}
