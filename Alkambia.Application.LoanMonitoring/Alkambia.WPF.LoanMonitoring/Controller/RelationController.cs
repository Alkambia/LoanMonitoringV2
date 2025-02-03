using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.WPF.LoanMonitoring.Views.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class RelationController
    {
        RelationMain RelationMain { get; set; }
        RelationForm RelationForm { get; set; }
        List<Model.Relation> Relations { get; set; }
        Model.Relation Relation { get; set; }

        public RelationController(RelationMain RelationMain)
        {
            this.RelationMain = RelationMain;
            events();
        }
        public void events()
        {
            RelationMain.addbtn.Click += Addbtn_Click;
            RelationMain.search_buton.Click += Search_buton_Click;
            RelationMain.editbtn.Click += Editbtn_Click;
        }

        private void Editbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RelationMain.relationDG.SelectedIndex > -1)
            {
                Relation = RelationMain.relationDG.SelectedItem as Model.Relation;
                RelationForm = new RelationForm();
                RelationForm.DataContext = Relation;
                RelationForm.savebtn.Content = "edit";
                RelationForm.savebtn.Click += FormSavebtn_Click;
                RelationForm.canceltbtn.Click += FormCanceltbtn_Click;
                RelationForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select data from grid", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Relations = RelationManager.Get(RelationMain.displayNameTB.Text).ToList();
            RelationMain.relationDG.ItemsSource = Relations;
        }

        private void Addbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Relation = new Model.Relation()
            {
                RelationID = Guid.NewGuid(),
                Name = string.Empty,
                DisplayName = string.Empty,
                Description = String.Empty,
                CreatedDate = DateTime.Now
            };
            RelationForm = new RelationForm();
            RelationForm.DataContext = Relation;
            RelationForm.savebtn.Click += FormSavebtn_Click;
            RelationForm.canceltbtn.Click += FormCanceltbtn_Click;
            RelationForm.ShowDialog();

        }

        private void FormCanceltbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RelationForm.Close();
        }

        private void FormSavebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add Validation logic
            if(!RelationForm.savebtn.Content.Equals("edit"))
            {
                RelationManager.Add(Relation); 
            }
            else
            {
                RelationManager.SaveorUpdate(Relation);
            }
            MessageBox.Show("Relation Entry Save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            RelationForm.Close();
        }

        
    }
}
