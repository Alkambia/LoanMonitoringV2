using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PaymentScheduleManager
    {
        public static void Add(PaymentSchedule entity)
        {
            using (var db = new DBDataContext())
            {
                db.PaymentSchedule.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(PaymentSchedule entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PaymentSchedule.Single(a => a.PaymentScheduleID == entity.PaymentScheduleID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PaymentSchedule.Single(a => a.PaymentScheduleID == Id);
                db.PaymentSchedule.Remove(obj);
                db.SaveChanges();
            }
        }

        public static void Delete(List<PaymentSchedule> entities)
        {
            using (var db = new DBDataContext())
            {
                //db.PaymentSchedule.RemoveRange(entities);
                foreach(var ent in entities)
                {
                    var obj = db.PaymentSchedule.Single(x => x.PaymentScheduleID == ent.PaymentScheduleID);
                    db.PaymentSchedule.Remove(obj);
                }
                db.SaveChanges();
            }
        }

        public static PaymentSchedule Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentSchedule.Single(a => a.PaymentScheduleID == Id);
            }
        }
        public static IEnumerable<PaymentSchedule> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentSchedule.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<PaymentSchedule> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PaymentSchedule.Where(x => x.PaymentScheduleID.ToString().ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
