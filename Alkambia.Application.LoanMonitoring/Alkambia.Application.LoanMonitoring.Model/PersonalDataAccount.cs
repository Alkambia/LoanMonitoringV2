using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class PersonalDataAccount
    {
        [Key]
        public Guid PersonalDataAccountID {get;set;}
        public string AccountNumber { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }
    }
}
