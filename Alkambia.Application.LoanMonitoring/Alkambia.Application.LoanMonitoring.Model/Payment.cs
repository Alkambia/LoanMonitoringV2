using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Payment : BaseEntity
    {
        [Key]
        public Guid PaymentID { get; set; }
        public DateTime Date { get; set; }
        public string ORNumber { get; set; }
        public double Principal { get; set; }
        public double Interest { get; set; }
        public double Charge { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentScheduleDate { get; set; }

        [ForeignKey("Loan")]
        public Guid LoanID { get; set; }
        public virtual Loan Loan { get; set; }

        public Guid CollertorID { get; set; }
    }
}
