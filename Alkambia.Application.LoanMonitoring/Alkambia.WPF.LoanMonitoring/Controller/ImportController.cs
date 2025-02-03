using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.WPF.LoanMonitoring.Views.Import;
using Alkambia.WPF.LoanMonitoring.HelperClient;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ImportController
    {
        ImportForm importF { get; set; } 
        public ImportController(ImportForm importF)
        {
            this.importF = importF;
            events();
        }
        public void events()
        {
            importF.browse_btn.Click += Browse_btn_Click;
            importF.import_btn.Click += Import_btn_Click;
        }

        private void Import_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataImporter.ImportExcelData(importF.excelUri_tb.Text);
        }

        private void Browse_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            Nullable<bool> result = dialog.ShowDialog();
            if(result == true)
            {
                importF.excelUri_tb.Text = dialog.FileName;
            }
        }
    }
}
