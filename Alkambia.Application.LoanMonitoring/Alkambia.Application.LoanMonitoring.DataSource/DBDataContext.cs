using Alkambia.App.LoanMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.DataSource
{
    public class DBDataContext: DbContext
    {

        public DBDataContext(string connectionstring)
            : base(connectionstring)
        {
        }
        public DBDataContext()
        : base("name=DbContext")
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<PersonalData> PersonalData { get; set; }
        public virtual DbSet<DigitalInfo> DigitalInfo { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<IncomeSource> IncomeSource { get; set; }
        public virtual DbSet<CreditReference> CreditReference { get; set; }
        public virtual DbSet<PersonalReference> PersonalReference { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<LoanApplication> LoanApplication { get; set; }
        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<ScheduleType> ScheduleType { get; set; }
        public virtual DbSet<CreditTerm> CreditTerm { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Investment> Investment { get; set; }
        public virtual DbSet<CoMaker> CoMaker { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<FixedExpense> FixedExpense { get; set; }
        public virtual DbSet<PaymentCharge> PaymentCharge { get; set; }
        public virtual DbSet<PaymentSchedule> PaymentSchedule { get; set; }
        public virtual DbSet<Release> Release { get; set; }
        public virtual DbSet<LoanPercentage> LoanPercentage { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfiguration { get; set; }
        public virtual DbSet<Kind> Kind { get; set; }
        public virtual DbSet<Relation> Relation { get; set; }
        public virtual DbSet<Miscellaneous> Miscellaneous { get; set; }

        public virtual DbSet<PersonalDataAccount> PersonalDataAccount { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Account>().ToTable("Account", "public");
            modelBuilder.Entity<PersonalData>().ToTable("PersonalData", "public");
            modelBuilder.Entity<DigitalInfo>().ToTable("DigitalInfo", "public");
            modelBuilder.Entity<Employer>().ToTable("Employer", "public");
            modelBuilder.Entity<IncomeSource>().ToTable("IncomeSource", "public");
            modelBuilder.Entity<CreditReference>().ToTable("CreditReference", "public");
            modelBuilder.Entity<PersonalReference>().ToTable("PersonalReference", "public");
            modelBuilder.Entity<Property>().ToTable("Property", "public");
            modelBuilder.Entity<LoanApplication>().ToTable("LoanApplication", "public");
            modelBuilder.Entity<Approval>().ToTable("Approval", "public");
            modelBuilder.Entity<Status>().ToTable("Status", "public");
            modelBuilder.Entity<Loan>().ToTable("Loan", "public");
            modelBuilder.Entity<ScheduleType>().ToTable("ScheduleType", "public");
            modelBuilder.Entity<CreditTerm>().ToTable("CreditTerm", "public");
            modelBuilder.Entity<Expense>().ToTable("Expense", "public");
            modelBuilder.Entity<Template>().ToTable("Template", "public");
            modelBuilder.Entity<Investment>().ToTable("Investment", "public");
            modelBuilder.Entity<CoMaker>().ToTable("CoMaker", "public");
            modelBuilder.Entity<Payment>().ToTable("Payment", "public");
            modelBuilder.Entity<FixedExpense>().ToTable("FixedExpense", "public");
            modelBuilder.Entity<PaymentCharge>().ToTable("PaymentCharge", "public");
            modelBuilder.Entity<PaymentSchedule>().ToTable("PaymentSchedule", "public");
            modelBuilder.Entity<Release>().ToTable("Release", "public");
            modelBuilder.Entity<LoanPercentage>().ToTable("LoanPercentage", "public");
            modelBuilder.Entity<SystemConfiguration>().ToTable("SystemConfiguration", "public");
            modelBuilder.Entity<Kind>().ToTable("Kind", "public");
            modelBuilder.Entity<Relation>().ToTable("Relation", "public");
            modelBuilder.Entity<Miscellaneous>().ToTable("Miscellaneous", "public");
            modelBuilder.Entity<PersonalDataAccount>().ToTable("PersonalDataAccount", "public");

            //set from config
            bool onModelCreating = false;
            if(onModelCreating)
            {
                base.OnModelCreating(modelBuilder);
            }else
            {
                modelBuilder.Conventions
                .Remove<StoreGeneratedIdentityKeyConvention>();
            }
            
        }
    }
}
