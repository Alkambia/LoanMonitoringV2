using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.WPF.LoanMonitoring.ModelHelper
{
    public class ExpenseModelHelper
    {
        public DateTime Date { get; set; }
        public int CellIndex { get; set; }
        public string UpperHeader { get; set; }
        public string DisplayName { get; set; }
        public double Amount { get; set; }
        public string InvoiceNumber { get; set; } //InvoiceNumber
        public string Particular { get; set; } //Particular
    }
}
