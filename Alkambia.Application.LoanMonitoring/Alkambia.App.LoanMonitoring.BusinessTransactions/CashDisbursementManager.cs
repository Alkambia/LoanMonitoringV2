using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class CashDisbursementManager
    {
        public static IEnumerable<CashDisbursement> GetDisbursements(string invoiceNumber)
        {
            var disbursementList = new List<CashDisbursement>();
            using (var db = new DBDataContext())
            {
                var expenses = db.Expense.Where(x => x.Name.Contains(invoiceNumber)).ToList();
                var invoiceNumbers = expenses.GroupBy(x => x.Name).Select(t => t.First()).ToList();
                foreach(var inv in invoiceNumbers)
                {
                    var dummies = expenses.Where(x => x.Name == inv.Name).ToList();
                    var disbursement = new CashDisbursement() {
                        InvoiceNumber = inv.Name,
                        DisbursementDate = inv.ExpenseDate,
                        Cash = dummies.Sum(x => x.Amount),
                        Particular = inv.Description,
                        Expenses = dummies
                    };
                    disbursementList.Add(disbursement);
                }

            }
            return disbursementList;
        }

        public static void Add(CashDisbursement disbursement)
        {
            if(disbursement.Expenses != null && disbursement.Expenses.Count() > 0)
            {
                foreach(var data in disbursement.Expenses)
                {
                    data.Name = disbursement.InvoiceNumber;
                    data.Description = disbursement.Particular;
                    data.ExpenseDate = disbursement.DisbursementDate;
                }
                ExpenseManager.AddRange(disbursement.Expenses);
            }
        }

        public static void Edit(CashDisbursement cashDisbursement, List<Guid> added, List<Guid> deleted)
        {
            foreach (var cb in cashDisbursement.Expenses)
            {
                cb.Name = cashDisbursement.InvoiceNumber;
                cb.ExpenseDate = cashDisbursement.DisbursementDate;
                cb.Description = cashDisbursement.Particular;
            }

            var newCBs = cashDisbursement.Expenses.Where(x => added.Contains(x.ExpenseID)).ToList();
            ExpenseManager.AddRange(newCBs);

            var updatedCBs = cashDisbursement.Expenses.Where(x => !added.Contains(x.ExpenseID)).ToList();
            foreach (var exp in updatedCBs)
            {
                ExpenseManager.SaveorUpdate(exp);
            }

            foreach (var del in deleted)
            {
                ExpenseManager.Delete(del);
            }
        }

        public static void Delete(CashDisbursement disbursement)
        {
            foreach (var del in disbursement.Expenses)
            {
                ExpenseManager.Delete(del.ExpenseID);
            }
        }
    }
}
