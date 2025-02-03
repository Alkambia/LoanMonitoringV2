using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Helper
{
    public class AccountCodeGenerator
    {
        public static string GenerateCode()
        {
            Random random = new Random();
            //Temporary
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9),
                random.Next(0, 9));
        }
        public static string GenerateCode(int codeLengt, int currentCode)
        {
            var codeString = currentCode.ToString();
            var lenght = codeString.Length;
            
            for(int x = 0; x < codeLengt-lenght; x++)
            {
                codeString = string.Format("0{0}", codeString);
            }
            return codeString;
        }
    }
}
