using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PropertyManager
    {
        public static void Add(Property entity)
        {
            using (var db = new DBDataContext())
            {
                db.Property.Add(entity);
                db.SaveChanges();
            }
        }
        public static void Add(List<Property> entities)
        {
            using (var db = new DBDataContext())
            {
                db.Property.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Property entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Property.Single(a => a.PropertyID == entity.PropertyID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Property.Single(a => a.PropertyID == Id);
                db.Property.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Property Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Property.Single(a => a.PropertyID == Id);
            }
        }
        public static IEnumerable<Property> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Property.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Property> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Property.Where(x => x.Location.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
        public static IEnumerable<Property> GetByPersonalDataID(Guid PersonalDataID)
        {
            using (var db = new DBDataContext())
            {
                return db.Property.Where(a => a.PersonalDataID == PersonalDataID);
            }
        }
    }
}
