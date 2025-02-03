using Alkambia.WPF.LoanMonitoring.Views.CashDisbursement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using System.Collections.ObjectModel;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class CashDisbursementController
    {
        DisbursementSearch Disbursement_Search { get; set; }
        DisbursementForm Disbursement_Form { get; set; }
        List<Model.CashDisbursement> CashDisbursements { get; set; }
        Model.CashDisbursement New_CashDisbursement { get; set; }
        Model.CashDisbursement Untouched_CashDisbursement { get; set; }

        List<Guid> AddedCDs { get; set; }
        List<Guid> DeletedCds { get; set; }

        public CashDisbursementController(DisbursementSearch Disbursement_Search)
        {
            this.Disbursement_Search = Disbursement_Search;
            Events();
            Init();
        }

        public void Init()
        {
            AddedCDs = new List<Guid>();
            DeletedCds = new List<Guid>();
            CashDisbursements = new List<Model.CashDisbursement>();
        }
        public void Events()
        {
            Disbursement_Search.search_buton.Click += Search_buton_Click;
            Disbursement_Search.new_button.Click += New_button_Click;
            Disbursement_Search.edit_button.Click += Edit_button_Click;
            Disbursement_Search.delete_button.Click += Delete_button_Click;
        }

        private void Edit_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Disbursement_Search.Data_DG.SelectedIndex > -1)
            {
                try
                {
                    New_CashDisbursement = Disbursement_Search.Data_DG.SelectedItem as Model.CashDisbursement;
                    Untouched_CashDisbursement = new Model.CashDisbursement()
                    {
                        InvoiceNumber = New_CashDisbursement.InvoiceNumber,
                        DisbursementDate = New_CashDisbursement.DisbursementDate,
                        Cash = New_CashDisbursement.Cash,
                        Particular = New_CashDisbursement.Particular,
                        Expenses = New_CashDisbursement.Expenses
                    };

                    Disbursement_Form = new DisbursementForm();
                    Disbursement_Form.DataContext = New_CashDisbursement;
                    Disbursement_Form.ExpenseDG.ItemsSource = New_CashDisbursement.Expenses;
                    //add,remove,save,cancel
                    Disbursement_Form.savedisbursement_btn.Content = "Edit";
                    Disbursement_Form.addExp_btn.Click += Form_AddExp_btn_Click;
                    Disbursement_Form.removeExp_btn.Click += Form_RemoveExp_btn_Click;
                    Disbursement_Form.savedisbursement_btn.Click += Form_Savedisbursement_btn_Click;
                    Disbursement_Form.cancel_btn.Click += Form_Cancel_btn_Click;
                    Disbursement_Form.ShowDialog();
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }else
            {
                MessageBox.Show("Please click something on the grid rows", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Delete_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Disbursement_Search.Data_DG.SelectedIndex > -1)
            {
                if(MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    
                    try
                    {
                        var item = Disbursement_Search.Data_DG.SelectedItem as Model.CashDisbursement;
                        CashDisbursementManager.Delete(item);
                        Disbursement_Search.Data_DG.Items.Refresh();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please click something on the grid rows", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void New_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            New_CashDisbursement = new Model.CashDisbursement() {
                InvoiceNumber = string.Empty,
                DisbursementDate = DateTime.Now,
                Cash = 0,
                Particular = string.Empty,
                Expenses = new List<Model.Expense>()
            };

            Disbursement_Form = new DisbursementForm();
            Disbursement_Form.DataContext = New_CashDisbursement;
            Disbursement_Form.ExpenseDG.ItemsSource = New_CashDisbursement.Expenses;
            //add,remove,save,cancel
            Disbursement_Form.addExp_btn.Click += Form_AddExp_btn_Click;
            Disbursement_Form.removeExp_btn.Click += Form_RemoveExp_btn_Click;
            Disbursement_Form.savedisbursement_btn.Click += Form_Savedisbursement_btn_Click;
            Disbursement_Form.cancel_btn.Click += Form_Cancel_btn_Click;
            Disbursement_Form.ShowDialog();
        }

        private void Form_Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Disbursement_Form.Close();
        }

        private void Form_Savedisbursement_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //New_CashDisbursement.DisbursementDate = New_CashDisbursement.DisbursementDate.Date;
                if (Disbursement_Form.savedisbursement_btn.Content.Equals("Edit"))
                {
                    CashDisbursementManager.Edit(New_CashDisbursement, AddedCDs, DeletedCds);
                    AddedCDs.Clear();
                    DeletedCds.Clear();
                }
                else
                {
                    CashDisbursementManager.Add(New_CashDisbursement);
                }
                MessageBox.Show("Data Save", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Disbursement_Form.Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Form_RemoveExp_btn_Click(object sender, RoutedEventArgs e)
        {
            if(Disbursement_Form.ExpenseDG.SelectedIndex > -1)
            {
                var item = Disbursement_Form.ExpenseDG.SelectedItem as Model.Expense;
                DeletedCds.Add(item.ExpenseID);
                var amount = item.Amount;
                New_CashDisbursement.Cash -= amount;
                New_CashDisbursement.Expenses.Remove(item);
                Disbursement_Form.ExpenseDG.Items.Refresh();
            }
        }

        private void Form_AddExp_btn_Click(object sender, RoutedEventArgs e)
        {
            
            if (New_CashDisbursement.Expenses == null)
            {
                New_CashDisbursement.Expenses = new List<Model.Expense>();
            }
            var new_cd = new Model.Expense()
            {
                ExpenseID = Guid.NewGuid(),
                ExpenseDate = DateTime.Now,
                Amount = 0
            };
            New_CashDisbursement.Expenses.Add(new_cd);
            AddedCDs.Add(new_cd.ExpenseID);
            Disbursement_Form.ExpenseDG.Items.Refresh();
        }

        private void Search_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CashDisbursements = CashDisbursementManager.GetDisbursements(Disbursement_Search.inv_number_tb.Text).ToList();
            Disbursement_Search.Data_DG.ItemsSource = CashDisbursements;
        }
    }
}
