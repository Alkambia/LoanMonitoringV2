using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class CoMakerManager
    {
        public void Add(CoMaker entity)
        {
            using (var db = new DBDataContext())
            {
                db.CoMaker.Add(entity);
                db.SaveChanges();
            }
        }
        public void SaveorUpdate(CoMaker entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CoMaker.Single(a => a.CoMakerID == entity.CoMakerID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.CoMaker.Single(a => a.CoMakerID == Id);
                db.CoMaker.Remove(obj);
                db.SaveChanges();
            }
        }

        public CoMaker Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.CoMaker.Single(a => a.CoMakerID == Id);
            }
        }
        public IEnumerable<CoMaker> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CoMaker.Skip(skip).Take(page).ToList();
            }
        }

        public IEnumerable<CoMaker> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.CoMaker.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
