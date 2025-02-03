using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class LoanPercentageManager
    {
        public static void Add(LoanPercentage entity)
        {
            using (var db = new DBDataContext())
            {
                db.LoanPercentage.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(LoanPercentage entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.LoanPercentage.Single(a => a.LoanPercentageID == entity.LoanPercentageID);
                obj.LastUpdatedDate = DateTime.Now;
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Percentage = entity.Percentage;
                obj.Description = entity.Description;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.LoanPercentage.Single(a => a.LoanPercentageID == Id);
                db.LoanPercentage.Remove(obj);
                db.SaveChanges();
            }
        }

        public static LoanPercentage Get()
        {
            using (var db = new DBDataContext())
            {
                try
                {
                    var percentages = new DBDataContext().LoanPercentage.Single(x => x.LoanPercentageID != Guid.Empty);
                    return percentages;
                }
                catch
                {
                    return null;
                }
                
            }
            
        }

        public static LoanPercentage Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanPercentage.Single(a => a.LoanPercentageID == Id);
            }
        }
        public static IEnumerable<LoanPercentage> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanPercentage.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<LoanPercentage> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanPercentage.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
