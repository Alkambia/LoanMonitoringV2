using Alkambia.App.LoanMonitoring.Helper;
using Alkambia.App.LoanMonitoring.Helper.Converter;
using Alkambia.WPF.LoanMonitoring.ModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.WPF.LoanMonitoring.HelperClient
{
    public class AgingClientHelper
    {
        private static void DailyAging(Model.Loan loan, AgingHelperModel helperModel, int month, DateTime nextpaymentDate, double installmentAmount)
        {

            var CurrentNextMonth = DateTime.Now.AddMonths(month);
            var totalDays = CurrentNextMonth.Subtract(DateTime.Now.AddMonths(month - 1)).TotalDays;
            var unpaidbill = installmentAmount;
            for (int y = 1; y <= totalDays; y++)
            {
                if (DateTime.Now > nextpaymentDate.AddMonths(month - 1).AddDays(y) && nextpaymentDate.AddMonths(month - 1).AddDays(y) < CurrentNextMonth)
                {
                    switch(month)
                    {
                        case 1:
                            helperModel.CurrentMonth = unpaidbill.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 2:
                            helperModel.SecondMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 3:
                            helperModel.ThirdMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 4:
                            helperModel.FourthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 5:
                            helperModel.FifthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 6:
                            helperModel.SixthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 7:
                            helperModel.SeventhMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 8:
                            helperModel.EightMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 9:
                            helperModel.NinthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 10:
                            helperModel.TenthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 11:
                            helperModel.EleventhMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 12:
                            helperModel.TwelfthMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 13:
                            helperModel.ThirdMonth = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                        case 14:
                            helperModel.OneYearAbove = unpaidbill.ToString(); //MoneyConverter.ConvertDoubleToMoney(unpaidbill);
                            break;
                    }
                    
                    unpaidbill += unpaidbill;
                }
            }
        }

        private static void SemiMonthlyAging(Model.Loan loan, AgingHelperModel helperModel, int month, DateTime nextpaymentDate, double installmentAmount)
        {

            var unpaidMoney = MoneyConverter.ConvertDoubleToMoney(installmentAmount);
            int daysInterval = loan.ScheduleTypes.FirstOrDefault().SecondSched - loan.ScheduleTypes.FirstOrDefault().FirstSched;
            var CurrentNextMonth = DateTime.Now.AddMonths(month);

            switch (month)
            {
                case 1:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.CurrentMonth = installmentAmount.ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.CurrentMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 2:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.SecondMonth = installmentAmount.ToString();// MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.SecondMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 3:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.ThirdMonth = installmentAmount.ToString();// MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.ThirdMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 4:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.FourthMonth = installmentAmount.ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.FourthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 5:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.FifthMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.FifthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 6:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.SixthMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.SixthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 7:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.SeventhMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.SeventhMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 8:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.EightMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.EightMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 9:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.NinthMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.NinthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 10:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.TenthMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.TenthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 11:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.EleventhMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.EleventhMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 12:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.TwelfthMonth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.TwelfthMonth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 13:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.Thirteenth = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.Thirteenth = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
                case 14:
                    if (DateTime.Now > nextpaymentDate.AddMonths(month - 1))
                    {
                        helperModel.OneYearAbove = installmentAmount.ToString(); //MoneyConverter.ConvertDoubleToMoney(installmentAmount);
                    }
                    if (DateTime.Now > nextpaymentDate.AddMonths(month).AddDays(daysInterval) && nextpaymentDate.AddDays(daysInterval) < CurrentNextMonth)
                    {
                        helperModel.OneYearAbove = (installmentAmount * 2).ToString();//MoneyConverter.ConvertDoubleToMoney(installmentAmount * 2);
                    }
                    break;
            }
        }

        public static void MonthlyAgingReport(Model.Loan loan, AgingHelperModel helperModel)
        {
            if (loan.ScheduleTypes.FirstOrDefault().Type == 0)//Perday
            {
                var nextpaymentDate = ScheduleCreator.PaymentSchedule(loan);
                var unpaid = loan.PaymentSchedules.Where(x => x.Schedule == nextpaymentDate).FirstOrDefault();

                for(int i = 1; i <= 14; i++)
                {
                    DailyAging(loan, helperModel, i, nextpaymentDate, unpaid.InstallmentAmount);
                }
                
            }
            else if (loan.ScheduleTypes.FirstOrDefault().Type == 2)
            {

                var nextpaymentDate = ScheduleCreator.PaymentSchedule(loan);
                var unpaid = loan.PaymentSchedules.Where(x => x.Schedule == nextpaymentDate).FirstOrDefault();

                for (int i = 1; i <= 14; i++)
                {
                    SemiMonthlyAging(loan, helperModel, i, nextpaymentDate, unpaid.InstallmentAmount);
                }
            }
            else
            {
                //this month
                var nextpaymentDate = ScheduleCreator.PaymentSchedule(loan);
                var unpaid = loan.PaymentSchedules.Where(x => x.Schedule == nextpaymentDate).FirstOrDefault();
                if (DateTime.Now > nextpaymentDate)
                {
                    helperModel.CurrentMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }

                //secondMonth
                if (DateTime.Now > nextpaymentDate.AddMonths(1))
                {
                    helperModel.SecondMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //thirdMonth
                if (DateTime.Now > nextpaymentDate.AddMonths(2))
                {
                    helperModel.ThirdMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //fourthMonth
                if (DateTime.Now > nextpaymentDate.AddMonths(3))
                {
                    helperModel.FourthMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }

                //5th
                if (DateTime.Now > nextpaymentDate.AddMonths(4))
                {
                    helperModel.FifthMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }

                //6th
                if (DateTime.Now > nextpaymentDate.AddMonths(5))
                {
                    helperModel.SixthMonth = unpaid.InstallmentAmount.ToString();// MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //7th
                if (DateTime.Now > nextpaymentDate.AddMonths(6))
                {
                    helperModel.SeventhMonth = unpaid.InstallmentAmount.ToString();// MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //8th
                if (DateTime.Now > nextpaymentDate.AddMonths(7))
                {
                    helperModel.EightMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //9th
                if (DateTime.Now > nextpaymentDate.AddMonths(8))
                {
                    helperModel.NinthMonth = unpaid.InstallmentAmount.ToString();// MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //10th
                if (DateTime.Now > nextpaymentDate.AddMonths(9))
                {
                    helperModel.TenthMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //11th
                if (DateTime.Now > nextpaymentDate.AddMonths(10))
                {
                    helperModel.EleventhMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
                //12th
                if (DateTime.Now > nextpaymentDate.AddMonths(11))
                {
                    helperModel.TwelfthMonth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }

                //13th
                if (DateTime.Now > nextpaymentDate.AddMonths(12))
                {
                    helperModel.Thirteenth = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }

                //above1year
                if (DateTime.Now > nextpaymentDate.AddMonths(13))
                {
                    helperModel.OneYearAbove = unpaid.InstallmentAmount.ToString(); // MoneyConverter.ConvertDoubleToMoney(unpaid.InstallmentAmount);
                }
            }
        }
    }
}
