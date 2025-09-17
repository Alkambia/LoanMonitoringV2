using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Model
{
    public class PersonalData: BaseEntity
    {
        [Key]
        public Guid PersonalDataID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Spouse { get; set; }
        public int Dependents { get; set; }
        public int DependentStudying { get; set; }
        public string HomeAddress { get; set; }
        public string BirthPlace { get; set; }
        public int House { get; set; }
        public string HouseText { get; set; }
        public string ContactNumber { get; set; }
        public string TIN { get; set; }
        public string ProvincialAddress { get; set; }
        public string MailingAddress { get; set; }
        public string Nationality { get; set; }
        public string EducationalAttainment { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<DigitalInfo> DigitalInfos { get; set; }
        public virtual ICollection<Employer> Employers { get; set; }
        public virtual ICollection<IncomeSource> IncomeSources { get; set; }
        public virtual ICollection<CreditReference> CreditReferences { get; set; }
        public virtual ICollection<PersonalReference> PersonalReferences { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<LoanApplication> LoanApplications { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }

        public virtual ICollection<PersonalDataAccount> PersonalDataAccounts { get; set; }
    }
}
