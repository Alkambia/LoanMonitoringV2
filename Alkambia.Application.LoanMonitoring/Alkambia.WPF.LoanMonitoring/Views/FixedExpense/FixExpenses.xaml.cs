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

namespace Alkambia.WPF.LoanMonitoring.Views.FixedExpense
{
    /// <summary>
    /// Interaction logic for FixExpenses.xaml
    /// </summary>
    public partial class FixExpenses : MetroWindow
    {
        public FixExpenses()
        {
            InitializeComponent();
            new FixedExpenseController(this);
        }

        //private void expense_button_Click(object sender, RoutedEventArgs e)
        //{
        //    new FixExpenseForm().ShowDialog();
        //}
    }
}
