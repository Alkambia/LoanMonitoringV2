using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class DigitalInfoManager
    {
        public static void Add(DigitalInfo entity)
        {
            using (var db = new DBDataContext())
            {
                db.DigitalInfo.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(DigitalInfo entity)
        {
            using (var db = new DBDataContext())
            {
                try
                {
                    var obj = db.DigitalInfo.Single(a => a.DigitalInfoID == entity.DigitalInfoID);
                    obj.Photo = entity.Photo;
                    db.SaveChanges();
                }
                catch
                {
                    Add(entity);
                }
                
            }
        }
        public void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.DigitalInfo.Single(a => a.DigitalInfoID == Id);
                db.DigitalInfo.Remove(obj);
                db.SaveChanges();
            }
        }

        public DigitalInfo Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.DigitalInfo.Single(a => a.DigitalInfoID == Id);
            }
        }
        public IEnumerable<DigitalInfo> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.DigitalInfo.Skip(skip).Take(page).ToList();
            }
        }

        public IEnumerable<DigitalInfo> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.DigitalInfo.Where(x => x.DigitalInfoID.ToString().ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
