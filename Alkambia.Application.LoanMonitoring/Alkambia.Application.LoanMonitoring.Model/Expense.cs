using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Expense: BaseEntity
    {
        [Key]
        public Guid ExpenseID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double Amount { get; set; }

    }
}
