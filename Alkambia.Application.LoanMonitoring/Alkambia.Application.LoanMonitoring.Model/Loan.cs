using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Loan: BaseEntity
    {
        [Key]
        public Guid LoanID { get; set; }
        public string AccountCode { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime FirstDueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public double Amortization { get; set; }
        
        public double InterestRate { get; set; } //permonth
        public double Principal { get; set; }
        public double Interest { get; set; }

        public double CashProceeds { get; set; }
        public double Miscellaneous { get; set; }

        [ForeignKey("LoanApplication")]
        public Guid LoanApplicationID { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }

        //[ForeignKey("CreditTerm")]
        //public Guid CreditTermID { get; set; }
        //public virtual CreditTerm CreditTerm { get; set; }
        public int LoanTerm { get; set; }

        [ForeignKey("Status")]
        public Guid StatusID { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<ScheduleType> ScheduleTypes { get; set; }
        public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<CoMaker> CoMakers { get; set; }
        public virtual ICollection<Release> Releases { get; set; }

    }
}
