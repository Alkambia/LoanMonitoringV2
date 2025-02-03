using System;
using System.Collections.Generic;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;
using System.Linq;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PersonalReferenceManager
    {
        public static void Add(PersonalReference entity)
        {
            using (var db = new DBDataContext())
            {
                db.PersonalReference.Add(entity);
                db.SaveChanges();
            }
        }
        public static void Add(List<PersonalReference> entities)
        {
            using (var db = new DBDataContext())
            {
                db.PersonalReference.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(PersonalReference entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalReference.Single(a => a.PersonalReferenceID == entity.PersonalReferenceID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalReference.Single(a => a.PersonalReferenceID == Id);
                db.PersonalReference.Remove(obj);
                db.SaveChanges();
            }
        }

        public static PersonalReference Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalReference.Single(a => a.PersonalReferenceID == Id);
            }
        }
        public static IEnumerable<PersonalReference> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalReference.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<PersonalReference> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalReference.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
        public static IEnumerable<PersonalReference> GetByPersonalDataID(Guid PersonalDataID)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalReference.Where(a => a.PersonalDataID == PersonalDataID);
            }
        }
    }
}
