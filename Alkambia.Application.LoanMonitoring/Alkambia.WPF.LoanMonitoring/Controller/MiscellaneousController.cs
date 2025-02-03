using Alkambia.WPF.LoanMonitoring.Views.Miscellaneous;
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
    public class MiscellaneousController
    {
        MiscellaneousForm mForm { get; set; }
        bool isNew { get; set; }
        Model.Miscellaneous miscClass { get; set; }

        public MiscellaneousController(MiscellaneousForm mForm)
        {
            this.mForm = mForm;
            events();
            inits();
        }

        void inits()
        {
            isNew = false;
            miscClass = MiscellaneousManager.Get();
            if(miscClass == null)
            {
                isNew = true;
                miscClass = new Model.Miscellaneous() {
                    MiscellaneousID = Guid.NewGuid(),
                    Name = string.Empty,
                    DisplayName = string.Empty,
                    CreatedDate = DateTime.Now,
                    Percentage = 0,
                    AdditionalCharge = 0
                };
            }
            mForm.DataContext = miscClass;
        }
        void events()
        {
            mForm.cancelbtn.Click += Cancelbtn_Click;
            mForm.savebtn.Click += Savebtn_Click;
        }

        private void Savebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (isNew)
                {
                    MiscellaneousManager.Add(miscClass);
                }
                else
                {
                    MiscellaneousManager.SaveorUpdate(miscClass);
                }
                MessageBox.Show("Data Save.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                mForm.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong while saving the data, please check you inputs, if error still occur please contact you system administrator.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Cancelbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mForm.Close();
        }
    }
}
