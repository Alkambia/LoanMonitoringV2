using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views = Alkambia.WPF.LoanMonitoring.Views;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using System.Windows.Controls;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class StatusController
    {
        Views.Status.Status StatusMain { get; set; }
        Views.Status.StatusForm StatusForm { get; set; }
        List<Model.Status> Statuses { get; set; }
        Model.Status Status { get; set; }
        public StatusController(Views.Status.Status StatusMain)
        {
            this.StatusMain = StatusMain;
            events();
        }
        private void events()
        {
            StatusMain.search_buton.Click += Search_buton_Click;
            StatusMain.status_button.Click += Status_button_Click;
            StatusMain.edit_button.Click += Edit_button_Click;
            StatusMain.del_button.Click += Delete_button_Click;
        }

        private void Delete_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StatusMain.StatusDG.SelectedIndex != -1)
                {
                    if (MessageBox.Show("Are you sure you want to delete this status?","Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Status = StatusMain.StatusDG.SelectedItem as Model.Status;
                        StatusManager.Delete(Status.StatusID);
                        MessageBox.Show("Status succesfully deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }  
                }
                else
                {
                    MessageBox.Show("Plese select status from grid to edit!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Status Cannot be deleted, because probably some entities is referencing on it!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
        private void Edit_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(StatusMain.StatusDG.SelectedIndex != -1)
            {
                Status = StatusMain.StatusDG.SelectedItem as Model.Status;
                StatusForm = new Views.Status.StatusForm();
                StatusForm.DataContext = Status;
                StatusForm.canceltbtn.Click += StatusFormCanceltbtn_Click;
                StatusForm.savebtn.Click += StatusFormSavebtn_Click;
                StatusForm.savebtn.Content = "Edit";
                StatusForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Plese select status from grid to edit!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Status_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Status = new Model.Status() {
                StatusID = Guid.NewGuid(),
                Name = string.Empty,
                Description = string.Empty,
                StatusEntity = string.Empty,
                CreatedDate = DateTime.Now
            };

            StatusForm = new Views.Status.StatusForm();
            StatusForm.DataContext = Status;
            StatusForm.canceltbtn.Click += StatusFormCanceltbtn_Click;
            StatusForm.savebtn.Click += StatusFormSavebtn_Click;
            StatusForm.ShowDialog();
        }

        private void StatusFormSavebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender as Button).Content.Equals("Edit"))
            {
                StatusManager.Add(Status);
            }
            else
            {
                StatusManager.SaveorUpdate(Status);
            }
            MessageBox.Show("Status Entry Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            StatusForm.Close();
        }

        private void StatusFormCanceltbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StatusForm.Close();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Statuses = StatusManager.GetDisplayName(StatusMain.displayNameTB.Text).ToList();
            StatusMain.StatusDG.ItemsSource = Statuses;
        }
    }
}
