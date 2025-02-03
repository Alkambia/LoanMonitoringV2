using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class SystemConfiguration: BaseEntity
    {
        [Key]
        public Guid ConfigurationID { get; set; }
        public string CompanyName { get; set; }
        public byte[] CompanyLogo { get; set; }
    }
}
