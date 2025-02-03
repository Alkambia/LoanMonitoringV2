using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class Template: BaseEntity
    {
        [Key]
        public Guid TemplateID { get; set; }
        public string Content { get; set; }

    }
}
