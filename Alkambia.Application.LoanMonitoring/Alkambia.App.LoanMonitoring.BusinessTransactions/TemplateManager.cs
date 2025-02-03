using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class TemplateManager
    {
        public void Add(Template entity)
        {
            using (var db = new DBDataContext())
            {
                db.Template.Add(entity);
                db.SaveChanges();
            }
        }
        public void SaveorUpdate(Template entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Template.Single(a => a.TemplateID == entity.TemplateID);
                obj = entity;
                db.SaveChanges();
            }
        }
        public void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Template.Single(a => a.TemplateID == Id);
                db.Template.Remove(obj);
                db.SaveChanges();
            }
        }

        public Template Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.Template.Single(a => a.TemplateID == Id);
            }
        }
        public IEnumerable<Template> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Template.Skip(skip).Take(page).ToList();
            }
        }

        public IEnumerable<Template> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.Template.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
    }
}
