using System;
using System.Collections.Generic;
using System.Linq;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class KindManager
    {
        public static void Add(Kind entity)
        {
            using (var db = new DBDataContext())
            {
                db.Kind.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Kind entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Kind.Single(a => a.KindID == entity.KindID);
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
                var obj = db.Kind.Single(a => a.KindID == Id);
                db.Kind.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Kind Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Kind.Single(a => a.KindID == Id);
            }
        }

        public static IEnumerable<Kind> Get()
        {
            using (var db = new DBDataContext())
            {
                return db.Kind.ToList();
            }
        }
        public static IEnumerable<Kind> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Kind.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Kind> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Kind.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
        public static IEnumerable<Kind> Get(string displayName)
        {
            using (var db = new DBDataContext())
            {
                return db.Kind.Where(x => x.DisplayName.ToLower().Contains(displayName.ToLower())).ToList();
            }
        }
    }
}
