using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class EmployerManager
    {
        public static void Add(Employer entity)
        {
            using (var db = new DBDataContext())
            {
                db.Employer.Add(entity);
                db.SaveChanges();
            }
        }
        public static void Add(List<Employer> entities)
        {
            using (var db = new DBDataContext())
            {
                db.Employer.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Employer entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Employer.Single(a => a.EmployerID == entity.EmployerID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Employer.Single(a => a.EmployerID == Id);
                db.Employer.Remove(obj);
                db.SaveChanges();
            }
        }
        
        public static Employer Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Employer.Single(a => a.EmployerID == Id);
            }
        }
        public static IEnumerable<Employer> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Employer.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Employer> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Employer.Where(x => x.EmployerName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
        public static IEnumerable<Employer> GetByPersonalDataID(Guid PersonalDataID)
        {
            using (var db = new DBDataContext())
            {
                return db.Employer.Where(a => a.PersonalDataID == PersonalDataID);
            }
        }
    }
}
