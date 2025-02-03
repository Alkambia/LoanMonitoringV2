using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class PaymentSchedule
    {
        [Key]
        public Guid PaymentScheduleID { get; set; }
        public DateTime Schedule { get; set; }
        public Double InstallmentAmount { get; set; }
        public Double Principal { get; set; }
        public Double Interest { get; set; }

        [ForeignKey("Loan")]
        public Guid LoanID { get; set; }
        public virtual Loan Loan { get; set; }
    }
}
