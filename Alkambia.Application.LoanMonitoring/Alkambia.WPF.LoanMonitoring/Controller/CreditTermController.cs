using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.WPF.LoanMonitoring.Views.CreditTerm;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class CreditTermController
    {
        Terms Term { get; set; }
        CreditTermMain CreditTermMain { get; set; }
        Model.CreditTerm CreditTerm { get; set; }
        List<Model.CreditTerm> CreditTerms { get; set; }
        bool IsNew { get; set; }
        public CreditTermController(Terms Term)
        {
            this.Term = Term;
            events();
        }
        public void events()
        {
            Term.search_buton.Click += Search_buton_Click;
            Term.add_button.Click += Add_button_Click;
            Term.edit_button.Click += Edit_button_Click;
            Term.del_button.Click += Del_button_Click;
        }

        private void Del_button_Click(object sender, RoutedEventArgs e)
        {
            if (Term.termsDG.SelectedIndex > -1)
            {
                if(MessageBox.Show("Are you sure you want to delete this?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CreditTerm = Term.termsDG.SelectedItem as Model.CreditTerm;
                    CreditTermManager.Delete(CreditTerm.CreditTermID);
                    MessageBox.Show("Credit Term Deleted!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select data from grid", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            if(Term.termsDG.SelectedIndex > -1)
            {
                CreditTerm = Term.termsDG.SelectedItem as Model.CreditTerm;
                CreditTermMain = new CreditTermMain();
                CreditTermMain.DataContext = CreditTerm;
                CreditTermMain.SaveBtn.Content = "edit";
                CreditTermMain.SaveBtn.Click += FormSaveBtn_Click;
                CreditTermMain.CancelBtn.Click += CancelBtn_Click;
                CreditTermMain.monthlyInterestTB.TextChanged += MonthlyInterestTB_TextChanged;
                CreditTermMain.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select data from grid", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            CreditTerm = new Model.CreditTerm()
            {
                CreditTermID = Guid.NewGuid(),
                Term = 0,
                Name = string.Empty,
                DisplayName = String.Empty,
                Description = String.Empty,
                CreatedDate = DateTime.Now
            };
            CreditTermMain = new CreditTermMain();
            CreditTermMain.DataContext = CreditTerm;
            CreditTermMain.SaveBtn.Click += FormSaveBtn_Click;
            CreditTermMain.CancelBtn.Click += CancelBtn_Click;
            CreditTermMain.monthlyInterestTB.TextChanged += MonthlyInterestTB_TextChanged;
            //CreditTermMain.totalInterestTB.TextChanged += TotalInterestTB_TextChanged;
            CreditTermMain.ShowDialog();
        }

        private void TotalInterestTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CreditTermMain.totalInterestTB.Text.Trim()))
                {
                    CreditTermMain.totalInterestTB.Text = "0";
                }
                
                CreditTermMain.monthlyInterestTB.Text = string.Format("{0}", (int.Parse(CreditTermMain.totalInterestTB.Text) / CreditTerm.Term));
            }
            catch
            {

            }
        }

        private void MonthlyInterestTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CreditTermMain.monthlyInterestTB.Text.Trim()))
                {
                    CreditTermMain.monthlyInterestTB.Text = "0";
                }
                CreditTermMain.totalInterestTB.Text = string.Format("{0}", (int.Parse(CreditTermMain.monthlyInterestTB.Text) * CreditTerm.Term));
            }
            catch
            {

            }
            
        }

        private void Search_buton_Click(object sender, RoutedEventArgs e)
        {
            CreditTerms = CreditTermManager.Get().ToList();
            Term.termsDG.ItemsSource = CreditTerms;
        }

        private void FormSaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Add Validation Logic
            CreditTerm.TotalInterest = (int.Parse(CreditTermMain.monthlyInterestTB.Text) * CreditTerm.Term);
            if (!CreditTermMain.SaveBtn.Content.Equals("edit"))
            {
                CreditTermManager.Add(CreditTerm);
            }
            else
            {
                CreditTermManager.SaveorUpdate(CreditTerm);
            }
            MessageBox.Show("Credit Term Save!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            CreditTermMain.Close();
        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreditTermMain.Close();
        }


    }
}
