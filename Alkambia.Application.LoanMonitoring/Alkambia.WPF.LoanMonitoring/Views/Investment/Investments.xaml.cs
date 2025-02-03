using Alkambia.WPF.LoanMonitoring.Controller;
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

namespace Alkambia.WPF.LoanMonitoring.Views.Investment
{
    /// <summary>
    /// Interaction logic for Investments.xaml
    /// </summary>
    public partial class Investments : MetroWindow
    {
        public Investments()
        {
            InitializeComponent();
            new InvestmentController(this);
        }

        //private void investment_button_Click(object sender, RoutedEventArgs e)
        //{
        //    new InvestmentForm().ShowDialog();
        //}
    }
}
