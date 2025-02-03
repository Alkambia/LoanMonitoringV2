using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Miscellaneous : BaseEntity
    {
        [Key]
        public Guid MiscellaneousID { get; set; }
        public double Percentage { get; set; }
        public double AdditionalCharge { get; set; }
    }
}
