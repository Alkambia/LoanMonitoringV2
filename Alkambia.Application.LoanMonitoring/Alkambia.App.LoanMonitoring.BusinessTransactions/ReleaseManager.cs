using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class ReleaseManager
    {
        public static void Add(Release entity)
        {
            using (var db = new DBDataContext())
            {
                db.Release.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Release entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Release.Single(a => a.ReleaseID == entity.ReleaseID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Release.Single(a => a.ReleaseID == Id);
                db.Release.Remove(obj);
                db.SaveChanges();
            }
        }
        public static void Delete(List<Release> entities)
        {
            using (var db = new DBDataContext())
            {
                //db.Release.RemoveRange(entities);
                foreach (var ent in entities)
                {
                    var obj = db.Release.Single(x => x.ReleaseID == ent.ReleaseID);
                    db.Release.Remove(obj);
                }
                db.SaveChanges();
            }
        }

        public static Release Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Release.Single(a => a.ReleaseID == Id);
            }
        }
        public static IEnumerable<Release> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Release.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Release> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Release.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
