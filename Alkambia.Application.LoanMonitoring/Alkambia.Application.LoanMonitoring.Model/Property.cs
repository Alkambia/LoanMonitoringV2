using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Property
    {
        [Key]
        public Guid PropertyID { get; set; }
        public string Location { get; set; }
        public double Value { get; set; }
        public int Encumbrances { get; set; } //0: false 1: true

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }


        [ForeignKey("Kind")]
        public Guid KindID { get; set; }
        public virtual Kind Kind { get; set; }

    }
}
