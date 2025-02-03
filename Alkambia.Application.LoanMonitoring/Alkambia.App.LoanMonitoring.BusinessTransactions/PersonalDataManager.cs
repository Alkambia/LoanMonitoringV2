using Alkambia.App.LoanMonitoring.DataSource;
using Alkambia.App.LoanMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alkambia.App.LoanMonitoring.BusinessTransactions
{
    public class PersonalDataManager
    {
        public static void Add(PersonalData entity)
        {
            using (var db = new DBDataContext())
            {
                db.PersonalData.Add(entity);
                db.SaveChanges();
            }
        }
        public static void SaveorUpdate(PersonalData entity)
        {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalData.Single(a => a.PersonalDataID == entity.PersonalDataID);
                obj.FirstName = entity.FirstName;
                obj.MiddleName = entity.MiddleName;
                obj.LastName = entity.LastName;
                obj.DateOfBirth = entity.DateOfBirth;
                obj.Gender = entity.Gender;
                obj.Spouse = entity.Spouse;
                obj.Dependents = entity.Dependents;
                obj.DisplayName = entity.DisplayName;

                obj.DependentStudying = entity.DependentStudying;
                obj.HomeAddress = entity.HomeAddress;
                obj.BirthPlace = entity.BirthPlace;
                obj.House = entity.House;
                obj.ContactNumber = entity.ContactNumber;
                obj.TIN = entity.TIN;
                obj.ProvincialAddress = entity.ProvincialAddress;
                obj.MailingAddress = entity.MailingAddress;
                obj.Nationality = entity.Nationality;
                obj.EducationalAttainment = entity.EducationalAttainment;

                db.SaveChanges();

                var employers = entity.Employers.Where(x => !obj.Employers.Any(y => y.EmployerID == x.EmployerID)).ToList();
                var incomeSources = entity.IncomeSources.Where(x => !obj.IncomeSources.Any(y => y.IncomeSourceID == x.IncomeSourceID)).ToList();
                var creditReferences = entity.CreditReferences.Where(x => !obj.CreditReferences.Any(y => y.CreditReferenceID == x.CreditReferenceID)).ToList();
                var personalReferences = entity.PersonalReferences.Where(x => !obj.PersonalReferences.Any(y => y.PersonalReferenceID == x.PersonalReferenceID)).ToList();
                var properties = entity.Properties.Where(x => !obj.Properties.Any(y => y.PropertyID == x.PropertyID)).ToList();
                var loanApplications = entity.LoanApplications.Where(x => !obj.LoanApplications.Any(y => y.LoanApplicationID == x.LoanApplicationID)).ToList();
                var personalDataAccounts = entity.PersonalDataAccounts.Where(x => !obj.PersonalDataAccounts.Any(y => y.PersonalDataAccountID == x.PersonalDataAccountID)).ToList();

                var person = new Model.PersonalData() {
                    Employers = employers,
                    IncomeSources = incomeSources,
                    CreditReferences = creditReferences,
                    PersonalReferences = personalReferences,
                    Properties = properties,
                    DigitalInfos = new List<Model.DigitalInfo>(),
                    LoanApplications = loanApplications,
                    PersonalDataAccounts = personalDataAccounts
                };

                clearDependents(person);

                if (employers.Count() > 0)
                {
                    EmployerManager.Add(employers);
                }
                if (incomeSources.Count() > 0)
                {
                    IncomeSourceManager.Add(incomeSources);
                }
                if (creditReferences.Count() > 0)
                {
                    CreditReferenceManager.Add(creditReferences);
                }
                if (personalReferences.Count() > 0)
                {
                    PersonalReferenceManager.Add(personalReferences);
                }
                if (properties.Count() > 0)
                {
                    PropertyManager.Add(properties);
                }
                
                if (loanApplications.Count() > 0)
                {
                    LoanApplicationManager.Add(loanApplications);
                }

                if(entity.DigitalInfos.Count > 0)
                {
                    DigitalInfoManager.SaveorUpdate(entity.DigitalInfos.FirstOrDefault());
                }
                if (entity.PersonalDataAccounts.Count > 0)
                {
                    //if(!PersonalDataAccountManager.IsAccountNumberExist(entity.PersonalDataAccounts.FirstOrDefault().AccountNumber) 
                    //    && PersonalDataAccountManager.IsPersonalDataExist(entity.PersonalDataID))
                    if(PersonalDataAccountManager.IsPersonalDataExist(entity.PersonalDataID))
                    {
                        PersonalDataAccountManager.SaveorUpdate(entity.PersonalDataAccounts.FirstOrDefault());
                    }
                    else {
                        PersonalDataAccountManager.Add(entity.PersonalDataAccounts.FirstOrDefault());
                    }
                }
                //obj.DigitalInfos = entity


            }
        }
        private static void clearDependents(Model.PersonalData person)
        {
            foreach (var entity in person.Employers)
            {
                entity.PersonalData = null;
                entity.Status = null;
            }
            foreach (var entity in person.IncomeSources)
            {
                entity.PersonalData = null;
            }
            foreach (var entity in person.CreditReferences)
            {
                entity.PersonalData = null;
                entity.Status = null;
            }
            foreach (var entity in person.PersonalReferences)
            {
                entity.PersonalData = null;
                entity.Relation = null;
            }
            foreach (var entity in person.Properties)
            {
                entity.PersonalData = null;
                entity.Kind = null;
            }
            foreach (var entity in person.DigitalInfos)
            {
                entity.PersonalData = null;
            }
            foreach (var entity in person.LoanApplications)
            {
                entity.PersonalData = null;
                entity.Approvals = null;
                entity.CreditTerm = null;
                entity.Releases = null;
                entity.Status = null;
            }
            foreach (var entity in person.PersonalDataAccounts)
            {
                entity.PersonalData = null;
            }
        }
        public static void Delete(Guid Id) {
            using (var db = new DBDataContext())
            {
                var obj = db.PersonalData.Single(a => a.PersonalDataID == Id);
                db.PersonalData.Remove(obj);
                db.SaveChanges();
            }
        }

        public static bool Exist(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                var count = db.PersonalData.Count(a => a.PersonalDataID == Id);
                if(count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static PersonalData Get(Guid Id)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalData.Single(a => a.PersonalDataID == Id);
            }
        }
        public static IEnumerable<PersonalData> Get(int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalData.Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<PersonalData> Get(string search, int skip, int page)
        {
            using (var db = new DBDataContext())
            {
                return db.PersonalData.Where(x => x.DisplayName.ToLower().Contains(search.ToLower()))
                .Skip(skip).Take(page).ToList();
            }
        }

        public static IEnumerable<PersonalData> Get(string search)
        {
            //using (var db = new DBDataContext())
            //{
                try
                {
                    return new DBDataContext().PersonalData.Where(x => x.DisplayName.ToLower().Contains(search.ToLower())).ToList();
                }
                catch {
                    return new List<PersonalData>();
                }
                
            //}
        }


    }
}
