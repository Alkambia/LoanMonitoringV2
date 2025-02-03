using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class ApprovalManager
    {
        static Model.Status StatusNew { get; set; }
        static Model.Status StatusReleased { get; set; }
        public static void Add(Approval entity)
        {
            using (var db = new DBDataContext())
            {
                db.Approval.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Approval entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Approval.Single(a => a.ApprovalID == entity.ApprovalID);
                obj.StatusID = entity.StatusID;
                db.SaveChanges();
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Approval.Single(a => a.ApprovalID == Id);
                db.Approval.Remove(obj);
                db.SaveChanges();
            }
        }
        
        public static Approval Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Approval.Single(a => a.ApprovalID == Id);
            }
        }

        public static IEnumerable<Approval> Get()
        {
            using (var db = new DBDataContext())
            {
                return db.Approval.ToList();
            }
        }

        public static IEnumerable<Approval> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Approval.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Approval> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Approval.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<Approval> GetUnReleasedApprovals()
        {
            if (StatusNew == null)
            {
                StatusNew = StatusManager.GetName("Status.New");
            }
            return new DBDataContext().Approval.Where(x => x.StatusID == StatusNew.StatusID).ToList();
        }

        public static IEnumerable<Approval> GetReleasedApprovals()
        {
            if (StatusReleased == null)
            {
                StatusReleased = StatusManager.GetName("Status.Released");
            }
            return new DBDataContext().Approval.Where(x => x.StatusID == StatusReleased.StatusID).ToList();

        }
    }
}
