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

namespace Alkambia.WPF.LoanMonitoring.Views.Relation
{
    /// <summary>
    /// Interaction logic for RelationMain.xaml
    /// </summary>
    public partial class RelationMain : MetroWindow
    {
        public RelationMain()
        {
            InitializeComponent();
            new RelationController(this);
        }
        
    }
}
