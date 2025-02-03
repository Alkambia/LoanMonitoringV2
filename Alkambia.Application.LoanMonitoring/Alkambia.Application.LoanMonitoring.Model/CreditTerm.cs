using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class CreditTerm :BaseEntity
    {
        [Key]
        public Guid CreditTermID { get; set; }
        public int Term { get; set; } //In Months
        public double MonthlyInterest { get; set; } //Percentage
        public double TotalInterest { get; set; } //Percentage

        //public virtual ICollection<Loan> Loans { get; set; }
    }
}
