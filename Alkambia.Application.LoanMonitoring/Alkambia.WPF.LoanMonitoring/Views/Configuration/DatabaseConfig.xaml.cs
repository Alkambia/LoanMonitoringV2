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
using MahApps.Metro.Controls;
using Alkambia.WPF.LoanMonitoring.Controller;

namespace Alkambia.WPF.LoanMonitoring.Views.Configuration
{
    /// <summary>
    /// Interaction logic for DatabaseConfig.xaml
    /// </summary>
    public partial class DatabaseConfig : MetroWindow
    {
        public DatabaseConfig()
        {
            InitializeComponent();
            new DatabaseConfigController(this);
        }
    }
}
