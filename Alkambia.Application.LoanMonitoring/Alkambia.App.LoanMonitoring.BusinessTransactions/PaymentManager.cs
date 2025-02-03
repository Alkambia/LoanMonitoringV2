using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PaymentManager
    {
        public static void Add(Payment entity)
        {
            using (var db = new DBDataContext())
            {
                entity.Loan = null;
                db.Payment.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Payment entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Payment.Single(a => a.PaymentID == entity.PaymentID);
                obj.LastUpdatedDate = DateTime.Now;
                obj.Date = entity.Date;
                obj.ORNumber = entity.ORNumber;
                obj.Principal = entity.Principal;
                obj.Interest = entity.Interest;
                obj.Charge = entity.Charge;
                obj.Amount = entity.Amount;
                obj.CollertorID = entity.CollertorID;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Payment.Single(a => a.PaymentID == Id);
                db.Payment.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Payment Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Payment.Single(a => a.PaymentID == Id);
            }
        }

        public static double GetPrincipalReturns()
        {
            using (var db = new DBDataContext())
            {
                return db.Payment.Sum(x => x.Principal);
            }
        }
        public static IEnumerable<Payment> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Payment.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Payment> GetPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return new DBDataContext().Payment.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public static IEnumerable<Payment> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Payment.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Payment> Get(string search)
        {
            return new DBDataContext().Payment.Where(x => x.Loan.AccountCode.ToLower() == search.ToLower()).ToList();
        }
    }
}
