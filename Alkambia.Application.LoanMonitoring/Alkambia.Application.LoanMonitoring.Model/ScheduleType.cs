using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class ScheduleType: BaseEntity
    {
        [Key]
        public Guid ScheduleTypeID { get; set; }
        public int Type { get; set; } //Daily:0, Monthly: 1, Twice a Month: 2
        public int FirstSched { get; set; } //if 0 then daily
        public int SecondSched { get; set; } //if 0 then once a month

        [ForeignKey("Loan")]
        public Guid LoanID { get; set; }
        public virtual Loan Loan { get; set; }

    }
}
