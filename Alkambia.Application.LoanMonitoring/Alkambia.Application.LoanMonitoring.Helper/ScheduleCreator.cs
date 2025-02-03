using Alkambia.App.LoanMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Helper
{
    public class ScheduleCreator
    {
        public static DateTime PaymentSchedule(Loan LoanClass)
        {
            var schedules = LoanClass.PaymentSchedules.OrderBy(x => x.Schedule).ToList();
            var payments = LoanClass.Payments;
            var paymentSched = schedules.FirstOrDefault().Schedule;
            int schedCount = 0;
            DateTime currentSched = DateTime.Now;
            foreach (var sched in schedules)
            {
                double sum = 0;
                var paymentsDate = new List<Model.Payment>();
                if (schedCount == 0)
                {
                    paymentsDate = payments.Where(x => x.PaymentScheduleDate <= sched.Schedule).ToList();
                }
                else
                {
                    paymentsDate = payments.Where(x => x.PaymentScheduleDate > schedules[schedCount - 1].Schedule && x.PaymentScheduleDate <= sched.Schedule).ToList();
                }

                if (paymentsDate.Count() > 0)
                {
                    sum = paymentsDate.Sum(x => x.Amount);
                    if (sum < sched.InstallmentAmount)
                    {
                        paymentSched = sched.Schedule;
                    }
                    else
                    {
                        if (schedCount < schedules.Count())
                        {
                            paymentSched = schedules[schedCount + 1].Schedule;
                        }
                        else
                        {
                            paymentSched = sched.Schedule;
                        }
                    }
                }
                else
                {
                    break;
                }
                schedCount++;
            }
            return paymentSched;
        }
        public static List<PaymentSchedule> GenerateSchedules(Loan LoanEntry, ScheduleType scheduleType)
        {
            double count = 0;
            if (scheduleType.Type == 0)//Perday
            {
                var dateTerm = LoanEntry.FirstDueDate.AddMonths(LoanEntry.LoanTerm);
                count = dateTerm.Subtract(LoanEntry.FirstDueDate).TotalDays;
            }
            else if (scheduleType.Type == 2)
            {
                count = LoanEntry.LoanTerm * 2;
            }
            else
            {
                count = LoanEntry.LoanTerm;
            }

            var semiCount = 0;
            int SchedDay = LoanEntry.FirstDueDate.Day;

            //test
            bool isSecondDay = false;
            int curr_month = 0;

            var schedules = new List<PaymentSchedule>();
            for (int i = 0; i < count; i++)
            {
                var principal = ((LoanEntry.Principal / (LoanEntry.Principal + LoanEntry.Interest)) * LoanEntry.Amortization);
                var interest = LoanEntry.Amortization - principal;
                var paymentSched = new PaymentSchedule();
                paymentSched.PaymentScheduleID = Guid.NewGuid();
                paymentSched.InstallmentAmount = LoanEntry.Amortization;
                paymentSched.Principal = principal;
                paymentSched.Interest = interest;
                paymentSched.LoanID = LoanEntry.LoanID;


                if (scheduleType.Type == 0)
                {
                    paymentSched.Schedule = LoanEntry.FirstDueDate.AddDays(i);
                }
                if (scheduleType.Type == 2)
                {
                    if (schedules.Count() > 0)
                    {
                        if (SchedDay == scheduleType.FirstSched)
                        {
                            SchedDay = scheduleType.SecondSched;
                            isSecondDay = true;
                        }
                        else
                        {
                            SchedDay = scheduleType.FirstSched;
                            isSecondDay = false;
                        }
                        semiCount = (scheduleType.SecondSched - scheduleType.FirstSched);
                        var curr = schedules.Max(x => x.Schedule).AddDays(semiCount);
                        curr_month = curr.Month;
                        if (isSecondDay)
                        {
                            var m = schedules.Max(x => x.Schedule).Month;
                            if (curr_month != m)
                            {
                                curr_month = m;
                            }
                            var monthdays = DateTime.DaysInMonth(curr.Year, curr.Month);
                            if (monthdays < SchedDay)
                            {
                                SchedDay = monthdays - (monthdays - SchedDay);
                            }
                        }
                        //if (DateTime.DaysInMonth(curr.Year, curr.Month) == 31)
                        //{
                        //    curr.AddDays(1);
                        //}
                        //if (DateTime.DaysInMonth(curr.Year, curr.Month) == 29)
                        //{
                        //    curr.AddDays(-1);
                        //}
                        //if (DateTime.DaysInMonth(curr.Year, curr.Month) == 28)
                        //{
                        //    curr.AddDays(-2);
                        //}
                        var createdDate = CreateDate(curr.Year, curr_month, SchedDay);
                        if(createdDate < schedules.Max(x => x.Schedule))
                        {
                            createdDate = createdDate.AddMonths(1);
                        }
                        paymentSched.Schedule = createdDate;
                        
                    }
                    else
                    {
                        paymentSched.Schedule = LoanEntry.FirstDueDate;
                        SchedDay = LoanEntry.FirstDueDate.Day;
                    }
                }
                if (scheduleType.Type == 1)
                {
                    var current = LoanEntry.FirstDueDate.AddMonths(i);
                    var missing = scheduleType.FirstSched - current.Day;
                    current.AddDays(missing);
                    paymentSched.Schedule = current;

                }
                if (paymentSched.Schedule != DateTime.MinValue)
                {
                    schedules.Add(paymentSched);
                }
            }

            return schedules.OrderBy(x => x.Schedule).ToList();
        }

        private static DateTime CreateDate(int year, int month, int day)
        {
            try
            {
                return new DateTime(year, month, day);
            }
            catch
            {
                return CreateDate(year, month, day - 1);
            }
        }
    }
}
