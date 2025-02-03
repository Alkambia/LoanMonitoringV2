using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.WPF.LoanMonitoring.Controller;
using Alkambia.App.LoanMonitoring.Enums;

namespace Alkambia.WPF.LoanMonitoring.Views.LoanApplication
{
    /// <summary>
    /// Interaction logic for LoanApplicationMain.xaml
    /// </summary>
    public partial class LoanApplicationMain : MetroWindow
    {
        int page = 0;
        public LoanApplicationMain(Model.PersonalData person, CrudEnums crud)
        {
            InitializeComponent();
            var controller = new LoanApplicationController(this, person, crud);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            
                page++;
                string current = string.Format("PersonalData{0}", page);
                string previous = string.Format("PersonalData{0}", page - 1);
                this.FindChild<UserControl>(previous).Visibility = Visibility.Collapsed;
                this.FindChild<UserControl>(current).Visibility = Visibility.Visible;
                CheckPage();
            
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            page--;
            string current = string.Format("PersonalData{0}", page);
            string previous = string.Format("PersonalData{0}", page + 1);
            this.FindChild<UserControl>(previous).Visibility = Visibility.Collapsed;
            this.FindChild<UserControl>(current).Visibility = Visibility.Visible;
            CheckPage();
        }
        private void CheckPage()
        {
            if(page < 1)
            {
                previous_buton.Visibility = Visibility.Hidden;
            }
            else if(page > 4)
            {
                next_button.Visibility = Visibility.Collapsed;
                submit_button.Visibility = Visibility.Visible;
            }
            else
            {
                next_button.Visibility = Visibility.Visible;
                previous_buton.Visibility = Visibility.Visible;
                submit_button.Visibility = Visibility.Collapsed;
            }
        }
    }
}
