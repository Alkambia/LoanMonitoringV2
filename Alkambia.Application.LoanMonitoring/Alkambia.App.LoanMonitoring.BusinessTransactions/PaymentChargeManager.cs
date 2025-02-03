using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PaymentChargeManager 
    {
        public static void Add(PaymentCharge entity)
        {
            using (var db = new DBDataContext())
            {
                db.PaymentCharge.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(PaymentCharge entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PaymentCharge.Single(a => a.PaymentChargeID == entity.PaymentChargeID);
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Description = entity.Description;
                obj.Percentage = entity.Percentage;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PaymentCharge.Single(a => a.PaymentChargeID == Id);
                db.PaymentCharge.Remove(obj);
                db.SaveChanges();
            }
        }

        public static bool Exist()
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentCharge.Count() > 0;
            }
        }

        public static PaymentCharge Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentCharge.Single(a => a.PaymentChargeID == Id);
            }
        }

        public static PaymentCharge Get()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.PaymentCharge.Single();
                }
            }
            catch
            {
                return null;
            }
            
        }
        public static IEnumerable<PaymentCharge> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentCharge.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<PaymentCharge> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentCharge.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
