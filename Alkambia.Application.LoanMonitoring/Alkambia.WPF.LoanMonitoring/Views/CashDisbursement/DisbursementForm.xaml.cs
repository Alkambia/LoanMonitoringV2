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

namespace Alkambia.WPF.LoanMonitoring.Views.CashDisbursement
{
    /// <summary>
    /// Interaction logic for DisbursementForm.xaml
    /// </summary>
    public partial class DisbursementForm : MetroWindow
    {
        public DisbursementForm()
        {
            InitializeComponent();
        }

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        string data = ((TextBox)sender).Text;
        //        double dummy = double.Parse(data);
        //        double cash_dummy = double.Parse(cash_tb.Text);
        //        cash_tb.Text = string.Format("{0}",dummy+cash_dummy);
        //    }
        //    catch
        //    {

        //    }
        //}
    }
}
