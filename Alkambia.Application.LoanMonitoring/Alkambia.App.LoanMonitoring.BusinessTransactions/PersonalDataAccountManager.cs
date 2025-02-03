using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PersonalDataAccountManager
    {
        public static void Add(PersonalDataAccount entity)
        {
            using (var db = new DBDataContext())
            {
                db.PersonalDataAccount.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(PersonalDataAccount entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalDataAccount.Single(a => a.PersonalDataAccountID == entity.PersonalDataAccountID);
                obj.AccountNumber = entity.AccountNumber;
                obj.IsApproved = entity.IsApproved;
                db.SaveChanges();
            }
        }

        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalDataAccount.Single(a => a.PersonalDataID == Id);
                db.PersonalDataAccount.Remove(obj);
                db.SaveChanges();
            }
        }

        public static bool IsAccountNumberExist(string accountNumber)
        {
            using (var db = new DBDataContext())
            {
                var count = db.PersonalDataAccount.Count(a => a.AccountNumber == accountNumber);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsPersonalDataExist(Guid Id)
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    var count = db.PersonalDataAccount.Count(a => a.PersonalDataID == Id);
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            
        }

        public static PersonalDataAccount Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalDataAccount.Single(a => a.PersonalDataAccountID == Id);
            }
        }

        public static bool IsAccountApproved(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalDataAccount.Single(a => a.PersonalDataID == Id);
                return obj.IsApproved;
            }
        }

        public static int GetMaxCode()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    var list = db.PersonalDataAccount.Where(x => x.AccountNumber != string.Empty).ToList();
                    var numbers = list.Select(x => int.Parse(x.AccountNumber)).ToArray();
                    return numbers.Max();
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
