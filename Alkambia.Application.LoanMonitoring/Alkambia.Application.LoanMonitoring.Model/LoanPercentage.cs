using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class LoanPercentage: BaseEntity
    {
        [Key]
        public Guid LoanPercentageID { get; set; }
        public double Percentage { get; set; }
    }
}
