using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class FixedExpenseManager
    {
        public static void Add(FixedExpense entity)
        {
            using (var db = new DBDataContext())
            {
                db.FixedExpense.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(FixedExpense entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.FixedExpense.Single(a => a.FixedExpenseID == entity.FixedExpenseID);
                obj.Amount = entity.Amount;
                obj.BillingDate = entity.BillingDate;
                obj.LastUpdatedDate = entity.LastUpdatedDate;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.FixedExpense.Single(a => a.FixedExpenseID == Id);
                db.FixedExpense.Remove(obj);
                db.SaveChanges();
            }
        }

        public static FixedExpense Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.FixedExpense.Single(a => a.FixedExpenseID == Id);
            }
        }

        public static IEnumerable<FixedExpense> GetExpenses()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.FixedExpense.ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public static IEnumerable<FixedExpense> GetExpenses(DateTime fr, DateTime to)
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.FixedExpense.Where(x => x.BillingDate >= fr && x.BillingDate <= to).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<FixedExpense>();
            }

        }
        public static IEnumerable<FixedExpense> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.FixedExpense.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<FixedExpense> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.FixedExpense.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<FixedExpense> Get(string displayname)
        {
            using (var db = new DBDataContext())
            {
                return db.FixedExpense.Where(x => x.DisplayName.ToLower().Contains(displayname.ToLower())).ToList();
            }
        }
    }
}
