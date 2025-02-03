using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class RelationManager
    {
        public static void Add(Relation entity)
        {
            using (var db = new DBDataContext())
            {
                db.Relation.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Relation entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Relation.Single(a => a.RelationID == entity.RelationID);
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
                var obj = db.Relation.Single(a => a.RelationID == Id);
                db.Relation.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Relation Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Relation.Single(a => a.RelationID == Id);
            }
        }

        public static IEnumerable<Relation> Get()
        {
            using (var db = new DBDataContext())
            {
                return db.Relation.ToList();
            }
        }

        public static IEnumerable<Relation> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Relation.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Relation> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Relation.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Relation> Get(string displayName)
        {
            using (var db = new DBDataContext())
            {
                return db.Relation.Where(x => x.DisplayName.ToLower().Contains(displayName.ToLower())).ToList();
            }
        }
    }
}
