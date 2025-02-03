using Alkambia.App.LoanMonitoring.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class MiscellaneousManager
    {
        public static void Add(Model.Miscellaneous entity)
        {
            using (var db = new DBDataContext())
            {
                db.Miscellaneous.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(Model.Miscellaneous entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.Miscellaneous.Single(a => a.MiscellaneousID == entity.MiscellaneousID);
                obj.Name = entity.Name;
                obj.DisplayName = entity.DisplayName;
                obj.Description = entity.Description;
                obj.LastUpdatedDate = entity.LastUpdatedDate;
                obj.Percentage = entity.Percentage;
                obj.AdditionalCharge = entity.AdditionalCharge;
                db.SaveChanges();
            }
        }

        public static Model.Miscellaneous Get()
        {
            try
            {
                using (var db = new DBDataContext())
                {
                    return db.Miscellaneous.FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }

        }
    }
}
