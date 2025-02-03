using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alkambia.WPF.LoanMonitoring.Converter
{
    public class AccountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;
            switch(int.Parse(value.ToString()))
            {
                case 0: result = "Super User";
                    break;
                case 1: result = "Administrator";
                    break;
                case 2: result = "Cashier";
                    break;
                case 3: result = "Reviewer";
                    break;
                case 4: result = "Guest";
                    break;
                case 5: result = "Collector";
                    break;
                default: result = "Not Found";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
