using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class ExpenseManager
    {
        public static void Add(Expense entity)
        {
            using (var db = new DBDataContext())
            {
                db.Expense.Add(entity);
                db.SaveChanges();
            }
        }
        public static void AddRange(List<Expense> entities)
        {
            using (var db = new DBDataContext())
            {
                db.Expense.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Expense entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Expense.Single(a => a.ExpenseID == entity.ExpenseID);
                obj.LastUpdatedDate = DateTime.Now;
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.ExpenseDate = entity.ExpenseDate;
                obj.Amount = entity.Amount;
                obj.Description = entity.Description;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Expense.Single(a => a.ExpenseID == Id);
                db.Expense.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Expense Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Expense.Single(a => a.ExpenseID == Id);
            }
        }

        public static IEnumerable<Expense> Get(DateTime ExpenseDate)
        {
            using (var db = new DBDataContext())
            {
                return db.Expense.Where(x => x.ExpenseDate == ExpenseDate).ToList();
            }
        }

        public static IEnumerable<Expense> Get(DateTime startingDate, DateTime endDate)
        {
            using (var db = new DBDataContext())
            {
                return db.Expense.Where(x => x.ExpenseDate >= startingDate && x.ExpenseDate <= endDate).ToList();
            }
        }

        public static IEnumerable<Expense> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Expense.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Expense> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Expense.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
