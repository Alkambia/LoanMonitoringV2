using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;
using Alkambia.App.LoanMonitoring.Helper;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class LoanManager
    {
        public static void Add(Loan entity)
        {
            using (var db = new DBDataContext())
            {
                db.Loan.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Loan entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Loan.Single(a => a.LoanID == entity.LoanID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Loan.Single(a => a.LoanID == Id);
                db.Loan.Remove(obj);
                db.SaveChanges();
            }
        }

        public static IEnumerable<Loan> GetByDisplayNameOrAccountCode(string search)
        {
            return new DBDataContext().Loan.Where(a => a.AccountCode == search || a.LoanApplication.PersonalData.DisplayName.ToUpper().Contains(search.ToUpper())).ToList();
        }
        public static Loan Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Loan.Single(a => a.LoanID == Id);
            }
        }
        public static Loan Get(string accountCode)
        {
            //using (var db = new DBDataContext())
            //{
                return new DBDataContext().Loan.Single(a => a.AccountCode == accountCode);
            //}
        }

        public static IEnumerable<Loan> Get()
        {
            return new DBDataContext().Loan.OrderBy(x => x.AccountCode).ToList();
        }

        public static IEnumerable<Loan> GetBorrowersWithLoan()
        {
            return new DBDataContext().Loan.Where(x => (x.Interest + x.Principal) > x.Payments.Sum(y => y.Amount) || x.Payments.Count() == 0).ToList();
        }

        public static bool WithUnpaidLoan(Guid PersonalDataId)
        {
            return new DBDataContext().Loan.Where(x => x.LoanApplication.PersonalDataID == PersonalDataId && (x.Interest + x.Principal) > x.Payments.Sum(y => y.Amount)).Count() > 0;
        }

        public static IEnumerable<Loan> GetBorrowersWithDueDate()
        {
            var borrowersWithLoan = GetBorrowersWithLoan().ToList();
            var borrowersWithDue = borrowersWithLoan.Where(x => ScheduleCreator.PaymentSchedule(x).Date < DateTime.Now.Date).ToList();

            return borrowersWithDue;
        }

        public static double GetReleaseLoanAmounts()
        {
            return new DBDataContext().Loan.Sum(x => x.Principal);
        }
        public static IEnumerable<Loan> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Loan.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Loan> Get(DateTime fr, DateTime to)
        {
            using (var db = new DBDataContext())
            {
                return db.Loan.Where(x => x.ReleaseDate >= fr && x.ReleaseDate <= to).ToList();
            }
        }

        public static IEnumerable<Loan> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Loan.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Loan> GetByDisplayName(string search)
        {
            return new DBDataContext().Loan.Where(x => x.LoanApplication.PersonalData.DisplayName.ToLower().Contains(search.ToLower())).ToList();
        }

        public static bool Exist(string accountCode)
        {
            using (var db = new DBDataContext())
            {
                return db.Loan.Where(x => x.AccountCode == accountCode).ToList().Count() != 0;
            }
        }

        public static int GetMaxCode()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    var list = db.Loan.Where(x => x.AccountCode != string.Empty).ToList();
                    var numbers = list.Select(x => int.Parse(x.AccountCode)).ToArray();
                    return numbers.Max();
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
