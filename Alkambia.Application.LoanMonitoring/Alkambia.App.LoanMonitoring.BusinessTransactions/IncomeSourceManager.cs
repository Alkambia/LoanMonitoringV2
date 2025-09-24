using System;
using System.Collections.Generic;
using System.Linq;
using Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class IncomeSourceManager
    {
        public static void Add(IncomeSource entity)
        {
            using (var db = new DBDataContext())
            {
                db.IncomeSource.Add(entity);
                db.SaveChanges();
            }
        }
        public static void Add(List<IncomeSource> entities)
        {
            using (var db = new DBDataContext())
            {
                db.IncomeSource.AddRange(entities);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(List<IncomeSource> entities)
        {
            using (var db = new DBDataContext())
            {
                foreach (var entity in entities)
                {
                    var obj = db.IncomeSource.FirstOrDefault(a => a.IncomeSourceID == entity.IncomeSourceID);
                    if (obj != null)
                    {
                        db.Entry(obj).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        db.IncomeSource.Add(entity);
                    }
                }

                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(IncomeSource entity)
        {
            using (var db = new DBDataContext())
            {
                if(entity.IncomeSourceID == Guid.Empty)
                {
                    db.IncomeSource.Add(entity);
                }
                else
                {
                    var obj = db.IncomeSource.Single(a => a.IncomeSourceID == entity.IncomeSourceID);
                    obj = entity;
                    db.SaveChanges();
                }
            }
        }

        public static void Delete(List<IncomeSource> entities)
        {
            using (var db = new DBDataContext())
            {
                foreach(var entity in entities)
                {
                    var obj = db.IncomeSource.Single(a => a.IncomeSourceID == entity.IncomeSourceID);
                    db.IncomeSource.Remove(obj);
                    db.SaveChanges();
                }
            }
        }
        public static void Delete(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.IncomeSource.Single(a => a.IncomeSourceID == Id);
                db.IncomeSource.Remove(obj);
                db.SaveChanges();
            }
        }

        public static IncomeSource Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.IncomeSource.Single(a => a.IncomeSourceID == Id);
            }
        }
        public static IEnumerable<IncomeSource> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.IncomeSource.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<IncomeSource> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.IncomeSource.Where(x => x.Nature.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }
        public static IEnumerable<IncomeSource> GetByPersonalDataID(Guid PersonaDataID)
        {
            using (var db = new DBDataContext())
            {
                return db.IncomeSource.Where(x => x.PersonalDataID == PersonaDataID).ToList();
            }
        }
    }
}
