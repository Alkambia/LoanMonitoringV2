using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.WPF.LoanMonitoring.ModelHelper
{
    public class ImportDataHelper
    {
        public string AccountCode { get; set; }
        public Guid PersonalDataID { get; set; }
        public Guid LoanApplicationID { get; set; }
        public Guid LoanID { get; set; }
        //public Guid PaymentID { get; set; }
        public Dictionary<Guid, string> Payments { get; set; }
        public Guid ScheduleTypeID { get; set; }
    }
}
