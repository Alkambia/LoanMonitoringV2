using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Status: BaseEntity
    {
        [Key]
        public Guid StatusID { get; set; }
        public string StatusEntity { get; set; } //CreditReference, Employer, Approval

        //public virtual Employer Employer { get; set; }
        //public virtual CreditReference CreditReference { get; set; }


    }
}
