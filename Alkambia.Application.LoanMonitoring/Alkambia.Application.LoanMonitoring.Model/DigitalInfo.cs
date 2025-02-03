using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class DigitalInfo
    {
        [Key]
        public Guid DigitalInfoID { get; set; }
        public byte[] Photo { get; set; }
        public string Barcode { get; set; }

        //one to one
        [ForeignKey("PersonalData")]
        public Guid PersonalDataID { get; set; }
        public virtual PersonalData PersonalData { get; set; }
    }
}
