using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Investment: BaseEntity
    {
        [Key]
        public Guid InvestmentID { get; set; }
        public double Capital { get; set; }
		public bool? IsApproved { get; set; }
    }
}
