using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class FixedExpense: BaseEntity
    {
        [Key]
        public Guid FixedExpenseID { get; set; }
        public double Amount { get; set; }

        public DateTime BillingDate { get; set; }
    }
}
