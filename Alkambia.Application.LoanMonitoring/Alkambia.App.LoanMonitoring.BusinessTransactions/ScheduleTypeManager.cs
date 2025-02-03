using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class ScheduleTypeManager
    {
        public static void Add(ScheduleType entity)
        {
            using (var db = new DBDataContext())
            {
                db.ScheduleType.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(ScheduleType entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.ScheduleType.Single(a => a.ScheduleTypeID == entity.ScheduleTypeID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.ScheduleType.Single(a => a.ScheduleTypeID == Id);
                db.ScheduleType.Remove(obj);
                db.SaveChanges();
            }
        }

        public static void Delete(List<ScheduleType> entities)
        {
            using (var db = new DBDataContext())
            {
                //db.ScheduleType.RemoveRange(entities);
                foreach (var ent in entities)
                {
                    var obj = db.ScheduleType.Single(x => x.ScheduleTypeID == ent.ScheduleTypeID);
                    db.ScheduleType.Remove(obj);
                }
                db.SaveChanges();
            }
        }

        public static ScheduleType Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.ScheduleType.Single(a => a.ScheduleTypeID == Id);
            }
        }
        public static IEnumerable<ScheduleType> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.ScheduleType.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<ScheduleType> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.ScheduleType.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
