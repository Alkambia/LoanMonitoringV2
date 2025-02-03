using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using Model = Alkambia.App.LoanMonitoring.Model;

namespace Alkambia.WPF.LoanMonitoring.Controller.TemplateExtensions
{
    public class PersonalSheet
    {
        string TempPathFull { get; set; }
        public PersonalSheet(Model.Loan loan)
        {
            var Statuses = StatusManager.GetDisplayName(string.Empty).ToList();
            var dir = System.IO.Directory.GetCurrentDirectory();
            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            TempPathFull = string.Format("{0}{1}.xls", temppath, filename);

            string url = string.Format(@"{0}\Documents\{1}", dir, "InformationSheet2.xlsx");
            int sheet = 1;
            System.IO.File.Copy(url, TempPathFull);

            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                workBook = application.Workbooks.Open(TempPathFull);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[sheet];

                worksheet.Cells.Replace(What: "{DisplayName}", Replacement: loan.LoanApplication.PersonalData.DisplayName, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{DateOfBirth}", Replacement: loan.LoanApplication.PersonalData.DateOfBirth, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                var age = new DateTime(DateTime.Now.Subtract(loan.LoanApplication.PersonalData.DateOfBirth).Ticks).Year - 1;
                worksheet.Cells.Replace(What: "{Age}", Replacement: age, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{Gender}", Replacement: loan.LoanApplication.PersonalData.Gender, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{Spouse}", Replacement: loan.LoanApplication.PersonalData.Spouse, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{Dependents}", Replacement: loan.LoanApplication.PersonalData.Dependents, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{DependentStudying}", Replacement: loan.LoanApplication.PersonalData.DependentStudying, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{HomeAddress}", Replacement: loan.LoanApplication.PersonalData.HomeAddress, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{HouseText}", Replacement: loan.LoanApplication.PersonalData.HouseText, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);

                worksheet.Cells.Replace(What: "{ContactNumber}", Replacement: loan.LoanApplication.PersonalData.ContactNumber, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{TIN}", Replacement: loan.LoanApplication.PersonalData.TIN, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{LoanAmount}", Replacement: loan.LoanApplication.LoanAmount, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{CreditTerm.Term}", Replacement: loan.LoanApplication.CreditTerm.DisplayName, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{LoanPurpose}", Replacement: loan.LoanApplication.LoanPurpose, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{ProvincialAddress}", Replacement: loan.LoanApplication.PersonalData.ProvincialAddress, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{Nationality}", Replacement: loan.LoanApplication.PersonalData.Nationality, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{MailingAddress}", Replacement: loan.LoanApplication.PersonalData.MailingAddress, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{EducationalAttainment}", Replacement: loan.LoanApplication.PersonalData.EducationalAttainment, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{BirthPlace}", Replacement: loan.LoanApplication.PersonalData.BirthPlace, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                worksheet.Cells.Replace(What: "{ApplicationDate}", Replacement: loan.LoanApplication.ApplicationDate.Date, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);

                var employers = loan.LoanApplication.PersonalData.Employers.ToList();
                if(employers.Count() > 0)
                {
                    var maxdate = employers.Max(x => x.EmployedFrom);
                    var current_emp = employers.FirstOrDefault(x => x.EmployedFrom == maxdate);
                    worksheet.Cells.Replace(What: "{EmployerName}", Replacement: current_emp.EmployerName, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{Position}", Replacement: current_emp.Position, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{Address}", Replacement: current_emp.Address, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{LenghtOfService}", Replacement: current_emp.LenghtOfService, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{BusinessNature}", Replacement: current_emp.BusinessNature, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{Status.DisplayName}", Replacement: current_emp.Status.DisplayName, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{Telephone}", Replacement: current_emp.Telephone, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{MonthlySalary}", Replacement: current_emp.MonthlySalary, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                    worksheet.Cells.Replace(What: "{Superior}", Replacement: current_emp.Superior, LookAt: Excel.XlLookAt.xlPart, SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: false);
                }

                int count = 0;
                var incomeSources = loan.LoanApplication.PersonalData.IncomeSources.ToList();
                if(incomeSources.Count() > 0)
                {
                    int row = 25;
                    foreach(var source in incomeSources)
                    {
                        if (count < 3)
                        {
                            worksheet.Cells[row, 1] = source.Nature;
                            worksheet.Cells[row, 3] = source.Income;
                            row++;
                        }
                        else
                        {
                            break;
                        }
                        count++;
                    }
                    
                }

                var creditReferrences = loan.LoanApplication.PersonalData.CreditReferences.ToList();
                if (creditReferrences.Count() > 0)
                {
                    count = 0;
                    int row = 30;
                    foreach (var referrence in creditReferrences)
                    {
                        if (count < 3)
                        {
                            worksheet.Cells[row, 1] = referrence.Creditor;
                            worksheet.Cells[row, 2] = referrence.Address;
                            worksheet.Cells[row, 4] = referrence.AmountLoan;
                            worksheet.Cells[row, 5] = ConverGrant(referrence.Granted);
                            worksheet.Cells[row, 7] = Statuses.FirstOrDefault(x => x.StatusID == referrence.StatusID).DisplayName;
                            row++;
                        }
                        else
                        {
                            break;
                        }
                        count++;
                    }
                }

                var personalReferences = loan.LoanApplication.PersonalData.PersonalReferences.ToList();
                if (personalReferences.Count() > 0)
                {
                    var reference = personalReferences.FirstOrDefault();
                    worksheet.Cells[35, 1] = reference.Name;
                    worksheet.Cells[35, 2] = reference.Address;
                    worksheet.Cells[35, 4] = reference.EmploymentBusiness;
                    worksheet.Cells[35, 7] = reference.Relation.DisplayName;
                }

                var properties = loan.LoanApplication.PersonalData.Properties.ToList();
                if (properties.Count() > 0)
                {
                    count = 0;
                    int row = 40;
                    foreach (var property in properties)
                    {
                        if (count < 4)
                        {
                            worksheet.Cells[row, 1] = property.Kind.DisplayName;
                            worksheet.Cells[row, 2] = property.Location;
                            worksheet.Cells[row, 4] = property.Value;
                            worksheet.Cells[row, 6] = ConverGrant(property.Encumbrances);
                            row++;
                        }
                        else
                        {
                            break;
                        }
                        count++;
                    }
                }
                workBook.Close(true, missing, missing);
                application.Quit();

            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        public void Open()
        {
            System.Diagnostics.Process.Start(TempPathFull);
        }
        private string ConverGrant(int x)
        {
            if(x == 0)
            {
                return "No";
            }
            else
            {
                return "Yes";
            }
        }
    }
}
