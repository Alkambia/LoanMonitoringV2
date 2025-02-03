using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Employer
    {
        [Key]
        public Guid EmployerID { get; set; }
        public string EmployerName { get; set; }
        public string Address { get; set; }
        public string BusinessNature { get; set; }
        public string Position { get; set; }
        public string LenghtOfService { get; set; }
        public string Telephone { get; set; }
        public string Superior { get; set; }
        public double MonthlySalary { get; set; }
        public DateTime EmployedFrom { get; set; }
        public DateTime? EmployedTo { get; set; }

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }

        [ForeignKey("Status")]
        public Guid StatusID { get; set; }
        public virtual Status Status { get; set; }
    }
}
