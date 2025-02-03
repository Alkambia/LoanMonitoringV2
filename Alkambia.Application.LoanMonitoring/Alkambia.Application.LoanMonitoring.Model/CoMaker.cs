using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class CoMaker: BaseEntity
    {
        [Key]
        public Guid CoMakerID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public string ContactNumber { get; set; }

        [ForeignKey("Loan")]
        public Guid LoanID { get; set; }
        public virtual Loan Loan { get; set; }
    }
}
