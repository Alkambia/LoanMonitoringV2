using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.WPF.LoanMonitoring.ModelHelper
{
    public class AgingHelperModel
    {
        public string AccountNumber { get; set; }
        public string Voucher { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string AmountLoan { get; set; }
        public string DateReleased { get; set; }
        public int Terms { get; set; }
        public string InterestRate { get; set; }
        public string Interest { get; set; }
        public string Total { get; set; }
        public string Amortization { get; set; }

        public string PaymentsInterest { get; set; }
        public string PaymentsCapital { get; set; }
        public string PaymentsTotal { get; set; }
        public string PaymetsBalance { get; set; }
        public string AccountStatus { get; set; }
        public string CurrentMonth { get; set; }
        public string SecondMonth { get; set; }
        public string ThirdMonth { get; set; }
        public string FourthMonth { get; set; }
        public string FifthMonth { get; set; }
        public string SixthMonth { get; set; }
        public string SeventhMonth { get; set; }
        public string EightMonth { get; set; }
        public string NinthMonth { get; set; }
        public string TenthMonth { get; set; }
        public string EleventhMonth { get; set; }
        public string TwelfthMonth { get; set; }
        public string Thirteenth { get; set; }
        //public string ThirtyOneToSixtyDays { get; set; }
        //public string SixtyOneToNinetyDays { get; set; }
        //public string NinetyOneToOnehundredTwentyDays { get; set; }
        //public string OnehundredTwentyOneToOneHundredFiftyDays { get; set; }
        //public string OneHundredFiftyOneToOneYear { get; set; }
        public string OneYearAbove { get; set; }
    }
}
