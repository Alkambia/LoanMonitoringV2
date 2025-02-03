using Alkambia.WPF.LoanMonitoring.Views.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.App.LoanMonitoring.Enums;
using System.Windows;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class SearchController
    {
        ApplicantSearch _search { get; set; }
        public SearchController(ApplicantSearch form)
        {
            this._search = form;
            events();
        }

        public void events()
        {
            _search.searchbutton.Click += Searchbutton_Click;
            _search.applicationbutton.Click += Applicationbutton_Click;
            _search.editinfobtn.Click += Editinfobtn_Click;
        }

        private void Editinfobtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_search.SearchDG.SelectedIndex > -1)
            {
                var person = _search.SearchDG.SelectedItem as PersonalData;
                _search.Hide();
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Edit);
                dummy.ShowDialog();
                _search.Show();
                _search.SearchDG.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Please click something on the row.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Applicationbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var person = new PersonalData();
            person.DateOfBirth = DateTime.Now;
            if (_search.SearchDG.SelectedItem != null)
            {
                person = _search.SearchDG.SelectedItem as PersonalData;
            }
            if (LoanManager.WithUnpaidLoan(person.PersonalDataID))
            {
                MessageBox.Show("Sorry, the person cant apply until loan is fully paid.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _search.Hide();
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Add);
                dummy.ShowDialog();
                _search.Show();
            }
            _search.SearchDG.SelectedIndex = -1;
        }

        private void Searchbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var itemsource = PersonalDataManager.Get(_search.searchTB.Text).ToList();
            _search.SearchDG.ItemsSource = itemsource;
        }
    }
}
