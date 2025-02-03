using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class SystemConfigurationManager 
    {
        public static void Add(SystemConfiguration entity)
        {
            using (var db = new DBDataContext())
            {
                db.SystemConfiguration.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(SystemConfiguration entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.SystemConfiguration.Single(a => a.ConfigurationID == entity.ConfigurationID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.SystemConfiguration.Single(a => a.ConfigurationID == Id);
                db.SystemConfiguration.Remove(obj);
                db.SaveChanges();
            }
        }

        public static SystemConfiguration Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.SystemConfiguration.Single(a => a.ConfigurationID == Id);
            }
        }
        public static IEnumerable<SystemConfiguration> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.SystemConfiguration.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<SystemConfiguration> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.SystemConfiguration.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static bool CheckDBConnecton()
        {
            try
            {
                new DBDataContext().Database.Connection.Open();
                new DBDataContext().Database.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
