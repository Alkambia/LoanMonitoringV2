using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class CreditReferenceManager
    {
        public static void Add(CreditReference entity)
        {
            using (var db = new DBDataContext())
            {
                db.CreditReference.Add(entity);
                db.SaveChanges();
            }
        }
        public static void Add(List<CreditReference> entities)
        {
            using (var db = new DBDataContext())
            {
                db.CreditReference.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(CreditReference entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CreditReference.Single(a => a.CreditReferenceID == entity.CreditReferenceID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CreditReference.Single(a => a.CreditReferenceID == Id);
                db.CreditReference.Remove(obj);
                db.SaveChanges();
            }
        }

        public static CreditReference Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditReference.Single(a => a.CreditReferenceID == Id);
            }
        }
        public static IEnumerable<CreditReference> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditReference.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<CreditReference> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditReference.Where(x => x.Creditor.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<CreditReference> GetByPersonalDataID(Guid PersonalDataID)
        {
            using (var db = new DBDataContext())
            {
                return db.CreditReference.Where(a => a.PersonalDataID == PersonalDataID);
            }
        }
    }
}
