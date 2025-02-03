using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class StatusManager
    {
        public static void Add(Status entity)
        {
            using (var db = new DBDataContext())
            {
                db.Status.Add(entity);
                //db.Status.Add(new Status() {
                //    StatusID = entity.StatusID,
                //    StatusEntity = entity.StatusEntity,
                //    Name = entity.Name,
                //    DisplayName = entity.DisplayName,
                //    Description = entity.Description,
                //    CreatedDate = entity.CreatedDate
                //});
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Status entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Status.Single(a => a.StatusID == entity.StatusID);
                obj.StatusEntity = entity.StatusEntity;
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
                var obj = db.Status.Single(a => a.StatusID == Id);
                db.Status.Remove(obj);
                db.SaveChanges();
            }
        }

        public static Status Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Status.Single(a => a.StatusID == Id);
            }
        }

        public static Status GetName(string name)
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Status.Single(a => a.Name == name);
                }
            }
            catch
            {
                return new Model.Status();
            }
            
        }

        public static IEnumerable<Status> GetDisplayName(string DisplayName)
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Status.Where(a => a.DisplayName.Contains(DisplayName.ToLower())).ToList();
                }
            }
            catch
            {
                return new List<Status>();
            }
            
        }

        public static IEnumerable<Status> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Status.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Status> GetByEntityName(string statusEntity)
        {
            using (var db = new DBDataContext())
            {
                return db.Status.Where(x => x.StatusEntity.Equals(statusEntity)).ToList();
            }
        }

        public static IEnumerable<Status> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Status.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
