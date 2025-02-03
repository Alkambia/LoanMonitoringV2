using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class LoanApplicationManager
    {
        static Model.Status StatusNew { get; set; }
        static Model.Status StatusDeclined { get; set; }
        public static void Add(LoanApplication entity)
        {
            using (var db = new DBDataContext())
            {
                db.LoanApplication.Add(entity);
                db.SaveChanges();
                
            }
        }
        public static void Add(List<LoanApplication> entities)
        {
            using (var db = new DBDataContext())
            {
                db.LoanApplication.AddRange(entities);
                db.SaveChanges();

            }
        }
        public static void SaveorUpdate(LoanApplication entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.LoanApplication.Single(a => a.LoanApplicationID == entity.LoanApplicationID);
                obj.StatusID = entity.StatusID;
                obj.Description = entity.Description;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.LoanApplication.Single(a => a.LoanApplicationID == Id);
                db.LoanApplication.Remove(obj);
                db.SaveChanges();
            }
        }

        public static LoanApplication Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanApplication.Single(a => a.LoanApplicationID == Id);
            }
        }

        public static IEnumerable<LoanApplication> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanApplication.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<LoanApplication> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.LoanApplication.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<LoanApplication> GetUnApproveLoans()
        {
            if(StatusNew == null)
            {
                StatusNew = StatusManager.GetName("Status.New");
            }
            //using (var db = new DBDataContext())
            //{
            try
            {
                return new DBDataContext().LoanApplication.Where(x => x.StatusID == StatusNew.StatusID).ToList();
            }
            catch
            {
                return null;
            }
            //}
        }

        public static IEnumerable<LoanApplication> GetUnApproveLoans(string DisplayName)
        {
            if (StatusNew == null)
            {
                StatusNew = StatusManager.GetName("Status.New");
            }
            //using (var db = new DBDataContext())
            //{
            try
            {
                return new DBDataContext().LoanApplication.Where(x => x.StatusID == StatusNew.StatusID && x.PersonalData.DisplayName.ToLower().Contains(DisplayName.ToLower())).ToList();
            }
            catch
            {
                return null;
            }
            
            //}
        }

        public static IEnumerable<LoanApplication> GetUnApproveLoans(string DisplayName, int skip, int page)
        {
            if (StatusNew == null)
            {
                StatusNew = StatusManager.GetName("Status.New");
            }
            using (var db = new DBDataContext())
            {
                return db.LoanApplication.Where(x => x.StatusID == StatusNew.StatusID && x.PersonalData.DisplayName.ToLower().Contains(DisplayName.ToLower())).ToList();
            }
        }

        public static IEnumerable<LoanApplication> GetDeclinedLoans(string DisplayName)
        {
            if (StatusDeclined == null)
            {
                StatusDeclined = StatusManager.GetName("Status.Declined");
            }
            try
            {
                return new DBDataContext().LoanApplication.Where(x => x.StatusID == StatusDeclined.StatusID && x.PersonalData.DisplayName.ToLower().Contains(DisplayName.ToLower())).ToList();
            }
            catch
            {
                return null;
            }
            

        }


    }
}
