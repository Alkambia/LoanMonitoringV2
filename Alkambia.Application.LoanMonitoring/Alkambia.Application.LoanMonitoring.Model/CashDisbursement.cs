using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class CashDisbursement
    {
        public string InvoiceNumber { get; set; }
        public DateTime DisbursementDate { get; set; }
        public double Cash { get; set; }
        public string Particular { get; set; }
        public List<Expense> Expenses { get; set; }
    }
}
