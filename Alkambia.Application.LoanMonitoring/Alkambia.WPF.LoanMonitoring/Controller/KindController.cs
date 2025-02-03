using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.WPF.LoanMonitoring.Views.Kind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class KindController
    {
        KindMain KindMain { get; set; }
        KindForm KindForm { get; set; }
        Model.Kind KindClass { get; set; }
        List<Model.Kind> Kinds { get; set; }

        public KindController(KindMain KindMain)
        {
            this.KindMain = KindMain;
            events();
        }
        public void events()
        {
            KindMain.search_buton.Click += Search_buton_Click;
            KindMain.addbtn.Click += Addbtn_Click;
            KindMain.editbtn.Click += Editbtn_Click;
        }

        private void Editbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(KindMain.KindDG.SelectedIndex > -1)
            {
                KindClass = KindMain.KindDG.SelectedItem as Model.Kind;
                KindForm = new KindForm();
                KindForm.DataContext = KindClass;
                KindForm.canceltbtn.Click += FormCanceltbtn_Click;
                KindForm.savebtn.Click += FormSavebtn_Click;
                KindForm.savebtn.Content = "edit";
                KindForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please click data from grid", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Addbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            KindClass = new Model.Kind() {
                KindID = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = String.Empty,
                DisplayName = String.Empty,
                Description = string.Empty
            };

            KindForm = new KindForm();
            KindForm.DataContext = KindClass;
            KindForm.canceltbtn.Click += FormCanceltbtn_Click;
            KindForm.savebtn.Click += FormSavebtn_Click;
            KindForm.ShowDialog();
        }

        private void FormSavebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add validation logic
            if(!KindForm.savebtn.Content.Equals("edit"))
            {
                KindManager.Add(KindClass);
            }else
            {
                KindManager.SaveorUpdate(KindClass);
            }
            MessageBox.Show("Kind Entry Save", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            KindForm.Close();
        }

        private void FormCanceltbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            KindForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Kinds = KindManager.Get(KindMain.displayNameTB.Text).ToList();
            KindMain.KindDG.ItemsSource = Kinds;
        }
    }
}
