using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Relation : BaseEntity
    {
        [Key]
        public Guid RelationID { get; set; }

    }
}
