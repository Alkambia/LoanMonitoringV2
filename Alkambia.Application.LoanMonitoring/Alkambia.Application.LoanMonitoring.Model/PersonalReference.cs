using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class PersonalReference : BaseEntity
    {
        [Key]
        public Guid PersonalReferenceID { get; set; } 
        public string Address { get; set; }
        public string EmploymentBusiness { get; set; }

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }

        [ForeignKey("Relation")]
        public Guid RelationID { get; set; }
        public virtual Relation Relation { get; set; }
    }
}
