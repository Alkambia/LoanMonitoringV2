namespace Alkambia.App.LoanMonitoring.DataSource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Account",
                c => new
                    {
                        AccountID = c.Guid(nullable: false),
                        AccountType = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID);
            
            CreateTable(
                "public.Approval",
                c => new
                    {
                        ApprovalID = c.Guid(nullable: false),
                        StatusID = c.Guid(nullable: false),
                        ApprovalDate = c.DateTime(nullable: false),
                        LoanApplicationID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ApprovalID)
                .ForeignKey("public.LoanApplication", t => t.LoanApplicationID, cascadeDelete: true)
                .Index(t => t.LoanApplicationID);
            
            CreateTable(
                "public.LoanApplication",
                c => new
                    {
                        LoanApplicationID = c.Guid(nullable: false),
                        ApplicationDate = c.DateTime(nullable: false),
                        LoanAmount = c.Double(nullable: false),
                        LoanPurpose = c.String(),
                        PersonalDataID = c.Guid(nullable: false),
                        CreditTermID = c.Guid(nullable: false),
                        StatusID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.LoanApplicationID)
                .ForeignKey("public.CreditTerm", t => t.CreditTermID, cascadeDelete: true)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.PersonalDataID)
                .Index(t => t.CreditTermID)
                .Index(t => t.StatusID);
            
            CreateTable(
                "public.CreditTerm",
                c => new
                    {
                        CreditTermID = c.Guid(nullable: false),
                        Term = c.Int(nullable: false),
                        MonthlyInterest = c.Double(nullable: false),
                        TotalInterest = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CreditTermID);
            
            CreateTable(
                "public.PersonalData",
                c => new
                    {
                        PersonalDataID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Spouse = c.String(),
                        Dependents = c.Int(nullable: false),
                        DependentStudying = c.Int(nullable: false),
                        HomeAddress = c.String(),
                        BirthPlace = c.String(),
                        House = c.Int(nullable: false),
                        ContactNumber = c.String(),
                        TIN = c.String(),
                        ProvincialAddress = c.String(),
                        MailingAddress = c.String(),
                        Nationality = c.String(),
                        EducationalAttainment = c.String(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PersonalDataID);
            
            CreateTable(
                "public.CreditReference",
                c => new
                    {
                        CreditReferenceID = c.Guid(nullable: false),
                        Creditor = c.String(),
                        Address = c.String(),
                        AmountLoan = c.Double(nullable: false),
                        Granted = c.Int(nullable: false),
                        PersonalDataID = c.Guid(nullable: false),
                        StatusID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CreditReferenceID)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.PersonalDataID)
                .Index(t => t.StatusID);
            
            CreateTable(
                "public.Status",
                c => new
                    {
                        StatusID = c.Guid(nullable: false),
                        StatusEntity = c.String(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.StatusID);
            
            CreateTable(
                "public.DigitalInfo",
                c => new
                    {
                        DigitalInfoID = c.Guid(nullable: false),
                        Photo = c.Binary(),
                        Barcode = c.String(),
                        PersonalDataID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DigitalInfoID)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .Index(t => t.PersonalDataID);
            
            CreateTable(
                "public.Employer",
                c => new
                    {
                        EmployerID = c.Guid(nullable: false),
                        EmployerName = c.String(),
                        Address = c.String(),
                        BusinessNature = c.String(),
                        Position = c.String(),
                        LenghtOfService = c.String(),
                        Telephone = c.String(),
                        Superior = c.String(),
                        MonthlySalary = c.Double(nullable: false),
                        EmployedFrom = c.DateTime(nullable: false),
                        EmployedTo = c.DateTime(),
                        PersonalDataID = c.Guid(nullable: false),
                        StatusID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.EmployerID)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.PersonalDataID)
                .Index(t => t.StatusID);
            
            CreateTable(
                "public.IncomeSource",
                c => new
                    {
                        IncomeSourceID = c.Guid(nullable: false),
                        Nature = c.String(),
                        Income = c.Double(nullable: false),
                        PersonalDataID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IncomeSourceID)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .Index(t => t.PersonalDataID);
            
            CreateTable(
                "public.Loan",
                c => new
                    {
                        LoanID = c.Guid(nullable: false),
                        AccountCode = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        FirstDueDate = c.DateTime(nullable: false),
                        MaturityDate = c.DateTime(nullable: false),
                        Amortization = c.Double(nullable: false),
                        InterestRate = c.Double(nullable: false),
                        Principal = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                        CashProceeds = c.Double(nullable: false),
                        Miscellaneous = c.Double(nullable: false),
                        LoanApplicationID = c.Guid(nullable: false),
                        LoanTerm = c.Int(nullable: false),
                        StatusID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                        PersonalData_PersonalDataID = c.Guid(),
                    })
                .PrimaryKey(t => t.LoanID)
                .ForeignKey("public.LoanApplication", t => t.LoanApplicationID, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.StatusID, cascadeDelete: true)
                .ForeignKey("public.PersonalData", t => t.PersonalData_PersonalDataID)
                .Index(t => t.LoanApplicationID)
                .Index(t => t.StatusID)
                .Index(t => t.PersonalData_PersonalDataID);
            
            CreateTable(
                "public.CoMaker",
                c => new
                    {
                        CoMakerID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                        HomeAddress = c.String(),
                        ContactNumber = c.String(),
                        LoanID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CoMakerID)
                .ForeignKey("public.Loan", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            CreateTable(
                "public.Payment",
                c => new
                    {
                        PaymentID = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ORNumber = c.String(),
                        Principal = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                        Charge = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        PaymentScheduleDate = c.DateTime(nullable: false),
                        LoanID = c.Guid(nullable: false),
                        CollertorID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("public.Loan", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            CreateTable(
                "public.PaymentSchedule",
                c => new
                    {
                        PaymentScheduleID = c.Guid(nullable: false),
                        Schedule = c.DateTime(nullable: false),
                        InstallmentAmount = c.Double(nullable: false),
                        Principal = c.Double(nullable: false),
                        Interest = c.Double(nullable: false),
                        LoanID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentScheduleID)
                .ForeignKey("public.Loan", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            CreateTable(
                "public.Release",
                c => new
                    {
                        ReleaseID = c.Guid(nullable: false),
                        LoanApplicationID = c.Guid(nullable: false),
                        LoanID = c.Guid(nullable: false),
                        DateReleased = c.DateTime(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ReleaseID)
                .ForeignKey("public.Loan", t => t.LoanID, cascadeDelete: true)
                .ForeignKey("public.LoanApplication", t => t.LoanApplicationID, cascadeDelete: true)
                .Index(t => t.LoanApplicationID)
                .Index(t => t.LoanID);
            
            CreateTable(
                "public.ScheduleType",
                c => new
                    {
                        ScheduleTypeID = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        FirstSched = c.Int(nullable: false),
                        SecondSched = c.Int(nullable: false),
                        LoanID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleTypeID)
                .ForeignKey("public.Loan", t => t.LoanID, cascadeDelete: true)
                .Index(t => t.LoanID);
            
            CreateTable(
                "public.PersonalReference",
                c => new
                    {
                        PersonalReferenceID = c.Guid(nullable: false),
                        Address = c.String(),
                        EmploymentBusiness = c.String(),
                        PersonalDataID = c.Guid(nullable: false),
                        RelationID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PersonalReferenceID)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .ForeignKey("public.Relation", t => t.RelationID, cascadeDelete: true)
                .Index(t => t.PersonalDataID)
                .Index(t => t.RelationID);
            
            CreateTable(
                "public.Relation",
                c => new
                    {
                        RelationID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.RelationID);
            
            CreateTable(
                "public.Property",
                c => new
                    {
                        PropertyID = c.Guid(nullable: false),
                        Location = c.String(),
                        Value = c.Double(nullable: false),
                        Encumbrances = c.Int(nullable: false),
                        PersonalDataID = c.Guid(nullable: false),
                        KindID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PropertyID)
                .ForeignKey("public.Kind", t => t.KindID, cascadeDelete: true)
                .ForeignKey("public.PersonalData", t => t.PersonalDataID, cascadeDelete: true)
                .Index(t => t.PersonalDataID)
                .Index(t => t.KindID);
            
            CreateTable(
                "public.Kind",
                c => new
                    {
                        KindID = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.KindID);
            
            CreateTable(
                "public.Expense",
                c => new
                    {
                        ExpenseID = c.Guid(nullable: false),
                        ExpenseDate = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseID);
            
            CreateTable(
                "public.FixedExpense",
                c => new
                    {
                        FixedExpenseID = c.Guid(nullable: false),
                        Amount = c.Double(nullable: false),
                        BillingDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.FixedExpenseID);
            
            CreateTable(
                "public.Investment",
                c => new
                    {
                        InvestmentID = c.Guid(nullable: false),
                        Capital = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.InvestmentID);
            
            CreateTable(
                "public.LoanPercentage",
                c => new
                    {
                        LoanPercentageID = c.Guid(nullable: false),
                        Percentage = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.LoanPercentageID);
            
            CreateTable(
                "public.Miscellaneous",
                c => new
                    {
                        MiscellaneousID = c.Guid(nullable: false),
                        Percentage = c.Double(nullable: false),
                        AdditionalCharge = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MiscellaneousID);
            
            CreateTable(
                "public.PaymentCharge",
                c => new
                    {
                        PaymentChargeID = c.Guid(nullable: false),
                        Percentage = c.Double(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentChargeID);
            
            CreateTable(
                "public.SystemConfiguration",
                c => new
                    {
                        ConfigurationID = c.Guid(nullable: false),
                        CompanyName = c.String(),
                        CompanyLogo = c.Binary(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ConfigurationID);
            
            CreateTable(
                "public.Template",
                c => new
                    {
                        TemplateID = c.Guid(nullable: false),
                        Content = c.String(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TemplateID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Approval", "LoanApplicationID", "public.LoanApplication");
            DropForeignKey("public.LoanApplication", "StatusID", "public.Status");
            DropForeignKey("public.LoanApplication", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.Property", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.Property", "KindID", "public.Kind");
            DropForeignKey("public.PersonalReference", "RelationID", "public.Relation");
            DropForeignKey("public.PersonalReference", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.Loan", "PersonalData_PersonalDataID", "public.PersonalData");
            DropForeignKey("public.Loan", "StatusID", "public.Status");
            DropForeignKey("public.ScheduleType", "LoanID", "public.Loan");
            DropForeignKey("public.Release", "LoanApplicationID", "public.LoanApplication");
            DropForeignKey("public.Release", "LoanID", "public.Loan");
            DropForeignKey("public.PaymentSchedule", "LoanID", "public.Loan");
            DropForeignKey("public.Payment", "LoanID", "public.Loan");
            DropForeignKey("public.Loan", "LoanApplicationID", "public.LoanApplication");
            DropForeignKey("public.CoMaker", "LoanID", "public.Loan");
            DropForeignKey("public.IncomeSource", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.Employer", "StatusID", "public.Status");
            DropForeignKey("public.Employer", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.DigitalInfo", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.CreditReference", "StatusID", "public.Status");
            DropForeignKey("public.CreditReference", "PersonalDataID", "public.PersonalData");
            DropForeignKey("public.LoanApplication", "CreditTermID", "public.CreditTerm");
            DropIndex("public.Property", new[] { "KindID" });
            DropIndex("public.Property", new[] { "PersonalDataID" });
            DropIndex("public.PersonalReference", new[] { "RelationID" });
            DropIndex("public.PersonalReference", new[] { "PersonalDataID" });
            DropIndex("public.ScheduleType", new[] { "LoanID" });
            DropIndex("public.Release", new[] { "LoanID" });
            DropIndex("public.Release", new[] { "LoanApplicationID" });
            DropIndex("public.PaymentSchedule", new[] { "LoanID" });
            DropIndex("public.Payment", new[] { "LoanID" });
            DropIndex("public.CoMaker", new[] { "LoanID" });
            DropIndex("public.Loan", new[] { "PersonalData_PersonalDataID" });
            DropIndex("public.Loan", new[] { "StatusID" });
            DropIndex("public.Loan", new[] { "LoanApplicationID" });
            DropIndex("public.IncomeSource", new[] { "PersonalDataID" });
            DropIndex("public.Employer", new[] { "StatusID" });
            DropIndex("public.Employer", new[] { "PersonalDataID" });
            DropIndex("public.DigitalInfo", new[] { "PersonalDataID" });
            DropIndex("public.CreditReference", new[] { "StatusID" });
            DropIndex("public.CreditReference", new[] { "PersonalDataID" });
            DropIndex("public.LoanApplication", new[] { "StatusID" });
            DropIndex("public.LoanApplication", new[] { "CreditTermID" });
            DropIndex("public.LoanApplication", new[] { "PersonalDataID" });
            DropIndex("public.Approval", new[] { "LoanApplicationID" });
            DropTable("public.Template");
            DropTable("public.SystemConfiguration");
            DropTable("public.PaymentCharge");
            DropTable("public.Miscellaneous");
            DropTable("public.LoanPercentage");
            DropTable("public.Investment");
            DropTable("public.FixedExpense");
            DropTable("public.Expense");
            DropTable("public.Kind");
            DropTable("public.Property");
            DropTable("public.Relation");
            DropTable("public.PersonalReference");
            DropTable("public.ScheduleType");
            DropTable("public.Release");
            DropTable("public.PaymentSchedule");
            DropTable("public.Payment");
            DropTable("public.CoMaker");
            DropTable("public.Loan");
            DropTable("public.IncomeSource");
            DropTable("public.Employer");
            DropTable("public.DigitalInfo");
            DropTable("public.Status");
            DropTable("public.CreditReference");
            DropTable("public.PersonalData");
            DropTable("public.CreditTerm");
            DropTable("public.LoanApplication");
            DropTable("public.Approval");
            DropTable("public.Account");
        }
    }
}
