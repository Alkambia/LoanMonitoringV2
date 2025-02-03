using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class PaymentCharge : BaseEntity
    {
        public Guid PaymentChargeID { get; set; }
        public Double Percentage { get; set; }
    }
}
