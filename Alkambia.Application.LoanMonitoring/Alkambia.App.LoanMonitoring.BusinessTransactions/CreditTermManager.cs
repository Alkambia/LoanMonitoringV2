using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class CreditTermManager
    {
        public static void Add(CreditTerm entity)
        {
            using (var db = new DBDataContext())
            {
                db.CreditTerm.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(CreditTerm entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CreditTerm.Single(a => a.CreditTermID == entity.CreditTermID);
                obj.Term = entity.Term;
                obj.MonthlyInterest = entity.MonthlyInterest;
                obj.TotalInterest = entity.TotalInterest;
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Description = entity.Description;
                obj.LastUpdatedDate = entity.LastUpdatedDate;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CreditTerm.Single(a => a.CreditTermID == Id);
                db.CreditTerm.Remove(obj);
                db.SaveChanges();
            }
        }

        public static CreditTerm Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditTerm.Single(a => a.CreditTermID == Id);
            }
        }

        public static IEnumerable<CreditTerm> Get()
        {
            using (var db = new DBDataContext())
            {
                return db.CreditTerm.ToList();
            }
        }
        public static IEnumerable<CreditTerm> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditTerm.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<CreditTerm> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditTerm.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
