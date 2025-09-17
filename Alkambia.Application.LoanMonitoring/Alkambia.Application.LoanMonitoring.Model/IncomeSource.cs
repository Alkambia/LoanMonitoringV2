using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class IncomeSource
    {
        [Key]
        public Guid IncomeSourceID { get; set; }
        public string Nature { get; set; }
        public double Income { get; set; }
        public string Type { get; set; }

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }
    }
}
