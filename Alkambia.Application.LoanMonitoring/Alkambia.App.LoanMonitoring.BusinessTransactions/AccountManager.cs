using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class AccountManager
    {
        public static void Add(Account entity)
        {
            using (var db = new DBDataContext())
            {
                db.Account.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Account entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Account.Single(a => a.AccountID == entity.AccountID);
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Description = entity.Description;
                obj.AccountType = entity.AccountType;
                obj.LastUpdatedDate = entity.LastUpdatedDate;
                obj.UserName = entity.UserName;
                obj.Password = entity.Password;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Account.Single(a => a.AccountID == Id);
                db.Account.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Account Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Account.Single(a => a.AccountID == Id);
            }
        }
        public static IEnumerable<Account> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Account.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Account> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Account.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Account> Get(string search)
        {
            using (var db = new DBDataContext())
            {
                return db.Account.Where(x => x.DisplayName.ToLower().Contains(search.ToLower())).ToList();
            }
        }

        public static IEnumerable<Account> Get(int accountType)
        {
            using (var db = new DBDataContext())
            {
                return db.Account.Where(x => x.AccountType == accountType).ToList();
            }
        }

        public static Account Get(string username, string password)
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Account.First(x => x.UserName == username && x.Password == password);
                }
            }
            catch
            {
                return null;
            }
            
        }

        public static bool IsAccountsExist()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Account.Count() > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
