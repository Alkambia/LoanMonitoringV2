using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Account : BaseEntity
    {
        [Key]
        public Guid AccountID { get; set; }
        public int AccountType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
