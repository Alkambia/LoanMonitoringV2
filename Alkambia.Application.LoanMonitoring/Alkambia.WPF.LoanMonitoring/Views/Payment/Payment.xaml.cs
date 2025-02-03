using Alkambia.WPF.LoanMonitoring.Controller;
using Alkambia.WPF.LoanMonitoring.Views.Approval;
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

namespace Alkambia.WPF.LoanMonitoring.Views.Payment
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : MetroWindow
    {
        public Payment()
        {
            InitializeComponent();
            new PaymentController(this);
        }
    }
}
