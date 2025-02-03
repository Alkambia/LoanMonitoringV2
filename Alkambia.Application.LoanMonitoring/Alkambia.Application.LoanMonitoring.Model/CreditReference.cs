using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class CreditReference
    {
        [Key]
        public Guid CreditReferenceID { get; set; }
        public string Creditor { get; set; }
        public string Address { get; set; }
        public double AmountLoan { get; set; }
        public int Granted { get; set; } //0:false 1: true

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }

        [ForeignKey("Status")]
        public Guid StatusID { get; set; }
        public virtual Status Status { get; set; }

    }
}
