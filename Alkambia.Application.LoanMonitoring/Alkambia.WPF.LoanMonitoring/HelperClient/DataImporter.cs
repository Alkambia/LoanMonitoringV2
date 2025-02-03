using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Model = Alkambia.App.LoanMonitoring.Model;
using System.Runtime.InteropServices;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Alkambia.WPF.LoanMonitoring.ModelHelper;
using Alkambia.App.LoanMonitoring.Helper;

namespace Alkambia.WPF.LoanMonitoring.HelperClient
{
    public class DataImporter
    {
        static List<Model.PersonalData> personalDatas { get; set; }
        static List<Model.LoanApplication> loanApplications { get; set; }
        static List<Model.Loan> loans { get; set; }
        static List<Model.Payment> payments { get; set; }
        static List<Model.ScheduleType> scheduleTypes { get; set; }
        static Model.Status StatusReleased { get; set; }
        static Model.Status StatusNew { get; set; }
        static Model.Status StatusApproved { get; set; }
        static Model.Miscellaneous miscClass { get; set; }
        static Model.Status StatusCurrent { get; set; }
        static List<ImportDataHelper> importDatas { get; set; }
        static List<Model.CreditTerm> creditTerms { get; set; }
        static List<Model.Account> Collectors { get; set; }
        public static void ImportExcelData(string url)
        {
            StatusReleased = StatusManager.GetName("Status.Released");
            StatusCurrent = StatusManager.GetName("Loan.Current");
            StatusNew = StatusManager.GetName("Status.New");
            StatusApproved = StatusManager.GetName("Status.Approved");
            Collectors = AccountManager.Get(5).ToList();
            creditTerms = CreditTermManager.Get().ToList();
            miscClass = MiscellaneousManager.Get();
            importDatas = new List<ImportDataHelper>();

            personalDatas = loadClientData<Model.PersonalData>(url, 1);
            loanApplications = loadClientData<Model.LoanApplication>(url, 2);
            loans = loadClientData<Model.Loan>(url, 3);
            payments = loadClientData<Model.Payment>(url, 4);
            scheduleTypes = loadClientData<Model.ScheduleType>(url, 5);
            
            ProcessImport();
        }

        private static T GetObject<T>() where T: new()
        {
            return new T();
        }

        private static List<T> loadClientData<T>(string url, int sheet) where T: new()
        {
            var t = new List<T>();
            var headers = new List<string>();
            int ExcelRow = 0;
            int ExcelCol = 0;
            try
            {
                var application = new Excel.Application();
                if (application != null)
                {
                    object missing = System.Reflection.Missing.Value;
                    Excel.Workbook xlWorkbook = application.Workbooks.Open(@url);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[sheet];
                    Excel.Range xlRange = xlWorksheet.UsedRange;
                    int rowCount = xlRange.Rows.Count;
                    int colCount = xlRange.Columns.Count;

                    
                    for (int i = 1; i <= rowCount; i++) 
                    {
                        ExcelRow++;
                        ExcelCol = 0;
                        string collector = string.Empty;
                        ImportDataHelper _dummy = new ImportDataHelper();
                        _dummy.Payments = new Dictionary<Guid, string>();
                        var obj = GetObject<T>();
                        for (int j = 1; j <= colCount; j++)
                        {
                            ExcelCol++;
                            //header
                            if (i == 1)
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    headers.Add(xlRange.Cells[i, j].Value2.ToString());
                                }
                            }
                            //row
                            else
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    Type _type = obj.GetType();
                                    PropertyInfo _propertyInfo = _type.GetProperty(headers[j-1]);

                                    if(headers[j - 1] == "AccountCode")
                                    {
                                        _dummy.AccountCode = xlRange.Cells[i, j].Value2;
                                    }
                                    if (headers[j - 1] == "Collector")
                                    {
                                        collector = xlRange.Cells[i, j].Value2;
                                    }
                                    if (_propertyInfo !=null)
                                    {
                                        object val = xlRange.Cells[i, j].Value2;
                                        if (_propertyInfo.PropertyType.Name == "DateTime")
                                        {
                                            string sDate = val.ToString();
                                            double date = double.Parse(sDate);
                                            DateTime dt = DateTime.FromOADate(date);
                                            _propertyInfo.SetValue(obj, dt);
                                        }
                                        else if (_propertyInfo.PropertyType.Name == "Int32")
                                        {
                                            string sDate = val.ToString();
                                            var data = int.Parse(sDate);
                                            _propertyInfo.SetValue(obj, data);
                                        }
                                        else if (_propertyInfo.PropertyType.Name == "Double")
                                        {
                                            string sDate = val.ToString();
                                            var data = double.Parse(sDate);
                                            _propertyInfo.SetValue(obj, data);
                                        }
                                        else
                                        {
                                            _propertyInfo.SetValue(obj, val.ToString());
                                        }
                                        
                                    }
                                    
                                }
                            }
                        }
                        if (i != 1)
                        {
                            Type _type = obj.GetType();

                            var x = importDatas.FirstOrDefault(xx => xx.AccountCode == _dummy.AccountCode);
                            if (x == null)
                            {
                                importDatas.Add(_dummy);
                                x = importDatas.FirstOrDefault(xx => xx.AccountCode == _dummy.AccountCode);
                            }
                            

                            if (obj.GetType().FullName == "Alkambia.App.LoanMonitoring.Model.PersonalData")
                            {
                                x.PersonalDataID = Guid.NewGuid();
                                PropertyInfo _propertyInfo = _type.GetProperty("PersonalDataID");
                                _propertyInfo.SetValue(obj, x.PersonalDataID);
                                
                            }
                            if (obj.GetType().FullName == "Alkambia.App.LoanMonitoring.Model.LoanApplication")
                            {
                                x.LoanApplicationID = Guid.NewGuid();
                                PropertyInfo _propertyInfo = _type.GetProperty("LoanApplicationID");
                                _propertyInfo.SetValue(obj, x.LoanApplicationID);
                            }
                            if (obj.GetType().FullName == "Alkambia.App.LoanMonitoring.Model.Loan")
                            {
                                x.LoanID = Guid.NewGuid();
                                PropertyInfo _propertyInfo = _type.GetProperty("LoanID");
                                _propertyInfo.SetValue(obj, x.LoanID);
                            }
                            if (obj.GetType().FullName == "Alkambia.App.LoanMonitoring.Model.Payment")
                            {
                                var paymentId = Guid.NewGuid();
                                x.Payments.Add(paymentId, collector);
                                PropertyInfo _propertyInfo = _type.GetProperty("PaymentID");
                                _propertyInfo.SetValue(obj, paymentId);
                            }
                            if (obj.GetType().FullName == "Alkambia.App.LoanMonitoring.Model.ScheduleType")
                            {
                                x.ScheduleTypeID = Guid.NewGuid();
                                PropertyInfo _propertyInfo = _type.GetProperty("ScheduleTypeID");
                                _propertyInfo.SetValue(obj, x.ScheduleTypeID);
                            }

                            t.Add(obj);
                        }
                            
                    }

                    //cleanup
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //release com objects to fully kill excel process from running in the background
                    Marshal.ReleaseComObject(xlRange);
                    Marshal.ReleaseComObject(xlWorksheet);

                    //close and release
                    xlWorkbook.Close(true, missing, missing);
                    Marshal.ReleaseComObject(xlWorkbook);

                    //quit and release
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                }
                else
                {
                    MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch(Exception ex)
            {
                var tx = ex;
                MessageBox.Show(string.Format("An error occured while importing, on Row {0}, Column{1}, Sheet {2}", ExcelRow, ExcelCol, sheet), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return t;

        }

        private static void ProcessImport()
        {
            int RowCount = 0;
            try
            {
                importDatas = importDatas.Where(x => x.AccountCode != null).ToList();
                foreach (var data in importDatas)
                {
                    RowCount++;
                    var personal = personalDatas.Single(x => x.PersonalDataID == data.PersonalDataID);
                    personal.Employers = new List<Model.Employer>();
                    personal.IncomeSources = new List<Model.IncomeSource>();
                    personal.CreditReferences = new List<Model.CreditReference>();
                    personal.PersonalReferences = new List<Model.PersonalReference>();
                    personal.Properties = new List<Model.Property>();
                    personal.DigitalInfos = new List<Model.DigitalInfo>();
                    personal.LoanApplications = new List<Model.LoanApplication>();

                    var loanApp = loanApplications.Single(x => x.LoanApplicationID == data.LoanApplicationID);
                    var loan = loans.Single(x => x.LoanID == data.LoanID);
                    var schedule = scheduleTypes.Single(x => x.ScheduleTypeID == data.ScheduleTypeID);

                    loanApp.CreditTermID = creditTerms.Single(x => x.Term == loan.LoanTerm).CreditTermID;
                    loanApp.PersonalDataID = data.PersonalDataID;
                    loanApp.StatusID = StatusApproved.StatusID;
                    loanApp.Approvals = new List<Model.Approval>();
                    loanApp.Releases = new List<Model.Release>();
                    personal.LoanApplications.Add(loanApp);
                    

                    personal.DisplayName = string.Format("{0} {1} {2}", personal.FirstName, personal.MiddleName, personal.LastName);
                    

                    var approval1 = new Model.Approval()
                    {
                        ApprovalID = Guid.NewGuid(),
                        StatusID = StatusReleased.StatusID,
                        ApprovalDate = DateTime.Now,
                        LoanApplicationID = data.LoanApplicationID,
                    };
                    

                    var LoanEntry = new Model.Loan()
                    {
                        LoanID = Guid.NewGuid(),
                        AccountCode = data.AccountCode,
                        ReleaseDate = loan.ReleaseDate,
                        FirstDueDate = loan.FirstDueDate,
                        MaturityDate = loan.MaturityDate,
                        LoanTerm = loan.LoanTerm,
                        Amortization = loan.Amortization,
                        InterestRate = loan.InterestRate,
                        Principal = loan.Principal,
                        Interest = loan.Interest,
                        LoanApplicationID = data.LoanApplicationID,
                        StatusID = StatusCurrent.StatusID,
                        ScheduleTypes = new List<Model.ScheduleType>(),
                        Releases = new List<Model.Release>(),
                        Miscellaneous = loan.Miscellaneous,
                        CashProceeds = loan.CashProceeds,
                        Payments = new List<Model.Payment>()
                    };

                    schedule.LoanID = data.LoanID;
                    LoanEntry.ScheduleTypes.Add(schedule);

                    //Create PaymentSchedule Entry
                    //var schedules = ScheduleCreator.GenerateSchedules(LoanEntry, schedule);
                    var PaymentSchedules = ScheduleCreator.GenerateSchedules(LoanEntry, schedule);
                    LoanEntry.PaymentSchedules = PaymentSchedules;

                    //Release
                    var release = new Model.Release()
                    {
                        ReleaseID = Guid.NewGuid(),
                        LoanApplicationID = data.LoanApplicationID,
                        LoanID = data.LoanID,
                        DateReleased = loan.ReleaseDate
                    };

                    LoanEntry.Releases.Add(release);
                    LoanEntry.MaturityDate = PaymentSchedules.Max(x => x.Schedule).Date;

                    //Payments
                    foreach (var p in data.Payments)
                    {
                        Model.Account acc = Collectors.FirstOrDefault(x => x.DisplayName == p.Value);
                        if(acc == null)
                        {
                            acc = new Model.Account() {
                                AccountID = Guid.NewGuid(),
                                Name = p.Value.Replace(" ", ""),
                                DisplayName = p.Value,
                                AccountType = 5,
                                UserName = p.Value.Replace(" ", ""),
                                Password = p.Value.Replace(" ", "")
                            };
                            AccountManager.Add(acc);
                        }
                        var pm = payments.Single(x => x.PaymentID == p.Key);
                        pm.LoanID = LoanEntry.LoanID;
                        pm.CollertorID = acc.AccountID;
                        LoanEntry.Payments.Add(pm);
                    }
                    

                    clearDependents(personal);
                    PersonalDataManager.Add(personal);
                    ApprovalManager.Add(approval1);
                    LoanManager.Add(LoanEntry);
                }
                MessageBox.Show("Import Success","Notification",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch(Exception ee)
            {
                MessageBox.Show(string.Format("Error on Row: {0}, Please contact your system administrator, further error: {1}", RowCount, ee.Message), "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
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
        }
    }
}
