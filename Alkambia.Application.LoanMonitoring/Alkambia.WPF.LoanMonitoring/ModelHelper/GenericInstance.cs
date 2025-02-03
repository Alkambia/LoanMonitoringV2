using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.WPF.LoanMonitoring.ModelHelper
{
    public class GenericInstance<T> where T: new()
    {
        public T GetNewItem()
        {
            return new T();
        }
    }
}
