using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class InvestmentManager
    {
        public static void Add(Investment entity)
        {
            using (var db = new DBDataContext())
            {
                db.Investment.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Investment entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Investment.Single(a => a.InvestmentID == entity.InvestmentID);
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Description = entity.Description;
                obj.Capital = entity.Capital;
                obj.IsApproved = entity.IsApproved;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Investment.Single(a => a.InvestmentID == Id);
                db.Investment.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Investment Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Investment.Single(a => a.InvestmentID == Id);
            }
        }

        public static double GetInvestmentAmounts()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Investment.Sum(x => x.Capital);
                }
            }
            catch
            {
                return 0;
            }
            
        }

        public static IEnumerable<Investment> Get(string displayName)
        {
            using (var db = new DBDataContext())
            {
                return db.Investment.Where(x => x.DisplayName.ToLower().Contains(displayName.ToLower())).ToList();
            }
        }

        public static IEnumerable<Investment> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Investment.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Investment> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Investment.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
