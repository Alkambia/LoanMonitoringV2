using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Release: BaseEntity
    {
        [Key]
        public Guid ReleaseID { get; set; }

        [ForeignKey("LoanApplication")]
        public Guid LoanApplicationID { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }

        [ForeignKey("Loan")]
        public Guid LoanID { get; set; }
        public virtual Loan Loan { get; set; }

        public DateTime DateReleased { get; set; }

    }
}
