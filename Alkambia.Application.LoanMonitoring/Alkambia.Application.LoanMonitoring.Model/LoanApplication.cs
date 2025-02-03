using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class LoanApplication : BaseEntity
    {
        [Key]
        public Guid LoanApplicationID { get; set; }
        public DateTime ApplicationDate { get; set; }

        public Double LoanAmount { get; set; }
        //public int LoanTerm { get; set; } //Months
        public string LoanPurpose { get; set; }

        //one to one or many
        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual ICollection<Release> Releases { get; set; }

        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }

        //Loan
        [ForeignKey("CreditTerm")]
        public Guid CreditTermID { get; set; }
        public virtual CreditTerm CreditTerm { get; set; }


        [ForeignKey("Status")]
        public Guid StatusID { get; set; }
        public virtual Status Status { get; set; }


    }
}
