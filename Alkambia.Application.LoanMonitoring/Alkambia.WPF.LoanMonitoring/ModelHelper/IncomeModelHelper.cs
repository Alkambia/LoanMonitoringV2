using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.WPF.LoanMonitoring.ModelHelper
{
    public class IncomeModelHelper
    {
        public DateTime Date { get; set; }
        public double MiscFee { get; set; }
        public double Interest { get; set; }
        public double Charge { get; set; }
        public double Total { get; set; }
        public double LessExp { get; set; }
        public double NetIncome { get; set; }
    }
}
