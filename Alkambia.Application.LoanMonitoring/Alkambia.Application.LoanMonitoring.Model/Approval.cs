using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Approval: BaseEntity
    {
        [Key]
        public Guid ApprovalID { get; set; }
        public Guid StatusID { get; set; }
        public DateTime ApprovalDate { get; set; }

        [ForeignKey("LoanApplication")]
        public Guid LoanApplicationID { get; set; }
        public virtual LoanApplication LoanApplication { get; set; }
    }
}
