using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.App.LoanMonitoring.Helper.Converter;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using Alkambia.WPF.LoanMonitoring.Views.Report;
using Alkambia.WPF.LoanMonitoring.ModelHelper;
using Alkambia.App.LoanMonitoring.Helper;
using System.Text.RegularExpressions;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ReportController
    {
        static List<Model.Loan> Loans { get; set; }
        static List<ReportHelperModel> reports { get; set; }
        ReportMain ReportMain { get; set; }
        DateRange DateRangeForm { get; set; }
        int Id { get; set; }

        public ReportController(ReportMain ReportMain)
        {
            this.ReportMain = ReportMain;
            init();
            events();
        }
        private void events()
        {
            ReportMain.view_buton.Click += View_buton_Click;
        }

        private void View_buton_Click(object sender, RoutedEventArgs e)
        {
            if(ReportMain.reportDG.SelectedIndex != -1)
            {
                var report = ReportMain.reportDG.SelectedItem as ReportHelperModel;
                if(report.Id == 1)
                {
                    GenerateAgingReport();
                }
                else if (report.Id == 2)
                {
                    InvestmentRemaining();
                }
                else if (report.Id == 3)
                {
                    //Remaining.Income
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 3;
                    DateRangeForm.ShowDialog();
                }
                else if (report.Id == 4)
                {
                    var loans = LoanManager.GetBorrowersWithLoan().ToList();
                    BorrowersWithLoan(loans,false);
                }
                else if (report.Id == 5)
                {
                    var loans = LoanManager.GetBorrowersWithDueDate().ToList();
                    BorrowersWithLoan(loans, true);
                }
                else if (report.Id == 6)
                {
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 6;
                    DateRangeForm.ShowDialog();
                }
                else if (report.Id == 7)
                {
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 7;
                    DateRangeForm.ShowDialog();
                }
                else if (report.Id == 8)
                {
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 8;
                    DateRangeForm.ShowDialog();
                }
                else if (report.Id == 9)
                {
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 9;
                    DateRangeForm.ShowDialog();
                }
                else if (report.Id == 10)
                {
                    DateRangeForm = new DateRange();
                    DateRangeForm.viewBTn.Click += DateRangeViewBTn_Click;
                    DateRangeForm.CancelBtn.Click += CancelBtn_Click;
                    Id = 10;
                    DateRangeForm.ShowDialog();
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DateRangeForm.Close();
        }

        private void DateRangeViewBTn_Click(object sender, RoutedEventArgs e)
        {
            if(DateRangeForm.fromDP.SelectedDate.HasValue && DateRangeForm.toDP.SelectedDate.HasValue)
            {
                var fromDate = DateRangeForm.fromDP.SelectedDate.Value;
                var toDate = DateRangeForm.toDP.SelectedDate.Value;
                if (Id == 3)
                {
                    //GetExpense
                    //GetFixExpense
                    //GetIncome
                    var payments = PaymentManager.GetPaymentsByDateRange(fromDate, toDate).ToList();
                    var fixExpenses = FixedExpenseManager.GetExpenses().ToList();
                    var expenses = ExpenseManager.Get(fromDate, toDate).ToList();
                    IncomeRemaining(payments, fixExpenses, expenses);

                }
                if(Id == 6)
                {
                    var loans = LoanManager.Get(fromDate, toDate).ToList();
                    var payments = PaymentManager.GetPaymentsByDateRange(fromDate, toDate).ToList();
                    var expenses = ExpenseManager.Get(fromDate, toDate).ToList();
                    var incomes = SetIncome(loans, payments, expenses, fromDate, toDate);
                    CreateIncomeReport(incomes);
                }
                if (Id == 7)
                {
                    //payments
                    GeneratePaymentsReport(fromDate, toDate);
                }
                if (Id == 8)
                {
                    //collection
                    GenerateCollectorsReport(fromDate, toDate);
                }
                if (Id == 9)
                {
                    var expenses = ExpenseManager.Get(fromDate, toDate).ToList();
                    var ExpensesAll = SetExpenses(expenses, fromDate, toDate);
                    CreateExpenseReport(ExpensesAll, fromDate, toDate);
                }
                if (Id == 10)
                {
                    new Alkambia.WPF.LoanMonitoring.Controller.ReportExtensions.DisbursementSummaryReport(fromDate, toDate);
                }
                DateRangeForm.Close();
            }
            else
            {
                MessageBox.Show("Please fill the date field!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }

        public void init()
        {
            reports = new List<ReportHelperModel>();
            reports.Add(new ReportHelperModel() {
                Id = 1,
                Name = "Aging",
                DisplayName = "Aging"
            });

            reports.Add(new ReportHelperModel()
            {
                Id = 2,
                Name = "Remaining.Investment",
                DisplayName = "Remaining Investment"
            });
            //reports.Add(new ReportHelperModel()
            //{
            //    Id = 3,
            //    Name = "Remaining.Income",
            //    DisplayName = "Remainining Income"
            //});

            reports.Add(new ReportHelperModel()
            {
                Id = 4,
                Name = "Borrowers.Loans",
                DisplayName = "Borrowers with current loan"
            });
            reports.Add(new ReportHelperModel()
            {
                Id = 5,
                Name = "Payments.Due",
                DisplayName = "Borrowers with due payments"
            });

            /******************/
            reports.Add(new ReportHelperModel()
            {
                Id = 6,
                Name = "Income",
                DisplayName = "Income by date range"
            });

            reports.Add(new ReportHelperModel()
            {
                Id = 7,
                Name = "Payments",
                DisplayName = "Payments by date range"
            });
            reports.Add(new ReportHelperModel()
            {
                Id = 8,
                Name = "Collection",
                DisplayName = "Collectors total collection by date range"
            });
            reports.Add(new ReportHelperModel()
            {
                Id = 9,
                Name = "Expenses",
                DisplayName = "Expenses"
            });
            reports.Add(new ReportHelperModel()
            {
                Id = 10,
                Name = "Disbursement",
                DisplayName = "Cash Disbursement"
            });

            ReportMain.reportDG.ItemsSource = reports;

        }

        private void GenerateCollectorsReport(DateTime fr, DateTime to)
        {
            var collectors = AccountManager.Get(5).ToList();
            var payments = PaymentManager.GetPaymentsByDateRange(fr, to).ToList();
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                //Header
                worksheet.Cells[1, 1] = "COLLECTOR";
                worksheet.Cells[1, 2] = "PRINCIPAL";
                worksheet.Cells[1, 3] = "INTEREST";
                worksheet.Cells[1, 4] = "CHARGE";
                worksheet.Cells[1, 5] = "TOTAL AMOUNT";

                //row
                var rowCount = 2;
                foreach (var col in collectors)
                {
                    var collections = payments.Where(x => x.CollertorID == col.AccountID);
                    worksheet.Cells[rowCount, 1] = col.DisplayName;
                    worksheet.Cells[rowCount, 2] = collections.Sum(x => x.Principal); //MoneyConverter.ConvertDoubleToMoney(collections.Sum(x => x.Principal));
                    worksheet.Cells[rowCount, 3] = collections.Sum(x => x.Interest); //MoneyConverter.ConvertDoubleToMoney(collections.Sum(x => x.Interest));
                    worksheet.Cells[rowCount, 4] = collections.Sum(x => x.Charge); //MoneyConverter.ConvertDoubleToMoney(collections.Sum(x => x.Charge));
                    worksheet.Cells[rowCount, 5] = collections.Sum(x => x.Amount + x.Charge); //MoneyConverter.ConvertDoubleToMoney(collections.Sum(x => x.Amount + x.Charge));
                    rowCount++;
                }

                //Total
                worksheet.Cells[rowCount, 1] = "TOTAL";
                worksheet.Cells[rowCount, 2] = payments.Sum(x => x.Principal); //MoneyConverter.ConvertDoubleToMoney(payments.Sum(x => x.Principal));
                worksheet.Cells[rowCount, 3] = payments.Sum(x => x.Interest); //MoneyConverter.ConvertDoubleToMoney(payments.Sum(x => x.Interest));
                worksheet.Cells[rowCount, 4] = payments.Sum(x => x.Charge); //MoneyConverter.ConvertDoubleToMoney(payments.Sum(x => x.Charge));
                worksheet.Cells[rowCount, 5] = payments.Sum(x => x.Amount); //MoneyConverter.ConvertDoubleToMoney(payments.Sum(x => x.Amount));

                var allRange = worksheet.Range["a1", string.Format("e{0}", rowCount)];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void GeneratePaymentsReport(DateTime fr, DateTime to)
        {
            var collectors = AccountManager.Get(5).ToList();
            var payments = PaymentManager.GetPaymentsByDateRange(fr, to).OrderBy(x => x.Date).ToList();
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                //Header
                worksheet.Cells[1, 1] = "DATE";
                worksheet.Cells[1, 2] = "OR NUMBER";
                worksheet.Cells[1, 3] = "PRINCIPAL";
                worksheet.Cells[1, 4] = "INTEREST";
                worksheet.Cells[1, 5] = "CHARGE";
                worksheet.Cells[1, 6] = "TOTAL";
                worksheet.Cells[1, 7] = "ACCOUNT CODE";
                worksheet.Cells[1, 8] = "BORROWER";
                worksheet.Cells[1, 9] = "COLLECTOR";

                //row
                var rowCount = 2;
                foreach(var pay in payments)
                {
                    worksheet.Cells[rowCount, 1] = pay.Date.ToShortDateString();
                    worksheet.Cells[rowCount, 2] = pay.ORNumber;
                    worksheet.Cells[rowCount, 3] = pay.Principal; // MoneyConverter.ConvertDoubleToMoney(pay.Principal);
                    worksheet.Cells[rowCount, 4] = pay.Interest; // MoneyConverter.ConvertDoubleToMoney(pay.Interest);
                    worksheet.Cells[rowCount, 5] = pay.Charge; // MoneyConverter.ConvertDoubleToMoney(pay.Charge);
                    worksheet.Cells[rowCount, 6] = (pay.Amount + pay.Charge); // MoneyConverter.ConvertDoubleToMoney(pay.Amount + pay.Charge);
                    worksheet.Cells[rowCount, 7].NumberFormat = "@";
                    worksheet.Cells[rowCount, 7].Value = pay.Loan.AccountCode;
                    worksheet.Cells[rowCount, 8] = pay.Loan.LoanApplication.PersonalData.DisplayName;
                    worksheet.Cells[rowCount, 9] = collectors.Single(x => x.AccountID == pay.CollertorID).DisplayName;
                    rowCount++;
                }
                var allRange = worksheet.Range["a1", string.Format("i{0}", rowCount)];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private List<ExpenseModelHelper> SetExpenses(List<Model.Expense> expenses, DateTime fr, DateTime to)
        {
            List<ExpenseModelHelper> objList = new List<ExpenseModelHelper>();
            var totaldays = to.Subtract(fr).TotalDays;
            //Expense
            foreach (var exp in expenses)
            {
                var upperHeaderSplit = exp.DisplayName.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                var obj = new ExpenseModelHelper
                {
                    Date = exp.ExpenseDate,
                    CellIndex = 0,
                    UpperHeader = upperHeaderSplit.Count() > 1 ? upperHeaderSplit[0].ToUpper() : string.Empty,
                    DisplayName = upperHeaderSplit.Count() > 1 ? upperHeaderSplit[1].ToUpper() : exp.DisplayName.ToUpper(),
                    Amount = exp.Amount
                };
                objList.Add(obj);
            }

            objList = objList.OrderByDescending(x => x.UpperHeader).ToList();
            int head = 1;
            foreach(var exp in objList)
            {
                if(!string.IsNullOrEmpty(exp.UpperHeader.Trim()))
                {
                    exp.CellIndex = head;
                    head++;
                }
                
            }

            return objList.OrderBy(x => x.CellIndex).ToList();
        }
        private void CreateExpenseReport(List<ExpenseModelHelper> Expenses, DateTime fr, DateTime to)
        {
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                //set header
                worksheet.Cells[3, 1] = "DATE";
                int headerStartCount = 2;

                //SetCommonHeaderFirst
                var commonHeaders = Expenses.GroupBy(x => x.DisplayName).Where(g => g.Count() > 1).Select(t => t.First()).OrderBy(u => u.UpperHeader).ToList();
                var noCommon = Expenses.Where(p => !commonHeaders.Any(p2 => p2.DisplayName == p.DisplayName)).ToList();
                var reportHeaders = commonHeaders.Union(noCommon).OrderBy(x => x.UpperHeader).ToList();
                foreach (var head in reportHeaders)
                {
                    worksheet.Cells[3, headerStartCount] = head.DisplayName.ToUpper();
                    head.CellIndex = headerStartCount;
                    headerStartCount++;
                    
                }
                worksheet.Cells[3, headerStartCount] = "TOTAL";
                string headerColumn = GetColumnName(worksheet.Columns[headerStartCount].Address);
                worksheet.Cells.Range["a3", string.Format("{0}3", headerColumn)].Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //set upperHeader
                var upperheader = reportHeaders.Where(x => x.UpperHeader != string.Empty).GroupBy(y => y.UpperHeader).Where(g => g.Count() > 1)
                    .Select(t => t.Key).ToList();
                foreach (var uphead in upperheader)
                {
                    var heads = reportHeaders.Where(x => x.UpperHeader.Equals(uphead)).OrderBy(t => t.CellIndex).ToList();
                    worksheet.Cells[2, heads.FirstOrDefault().CellIndex] = heads.FirstOrDefault().UpperHeader;
                    string columnAddress1 = worksheet.Columns[heads.FirstOrDefault().CellIndex].Address;
                    string first = GetColumnName(columnAddress1.ToString());

                    string columnAddress2 = worksheet.Columns[heads.LastOrDefault().CellIndex].Address;
                    string second = GetColumnName(columnAddress2.ToString());
                    var range = worksheet.Range[string.Format("{0}2", first), string.Format("{0}2", second)];
                    range.Merge();

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                //set Row
                int rowCountStart = 4;
                var totaldays = to.Subtract(fr).TotalDays;
                for (int i = 0; i <= totaldays; i++)
                {
                    worksheet.Cells[rowCountStart, 1] = fr.AddDays(i).ToShortDateString();
                    headerStartCount = 2;
                    double total = 0;
                    
                    foreach(var h in reportHeaders)
                    {
                        var data = Expenses.Where(x => x.Date.Date == fr.AddDays(i).Date && x.DisplayName == h.DisplayName).ToList();
                        if (data.Count() > 0)
                        {
                            worksheet.Cells[rowCountStart, headerStartCount] = data.Sum(x => x.Amount);// MoneyConverter.ConvertDoubleToMoney(data.Sum(x => x.Amount));
                            total = total + data.Sum(x => x.Amount);
                        }

                        headerStartCount++;
                    }
                    worksheet.Cells[rowCountStart, headerStartCount] = total; // MoneyConverter.ConvertDoubleToMoney(total);
                    rowCountStart++;
                }

                //Set Total
                worksheet.Cells[rowCountStart, 1] = "TOTAL";
                headerStartCount = 2;
                double grandtotal = 0;
                foreach (var h in reportHeaders)
                {
                    var total = Expenses.Where(x => x.DisplayName == h.DisplayName).Sum(x => x.Amount);
                    worksheet.Cells[rowCountStart, headerStartCount] = total; // MoneyConverter.ConvertDoubleToMoney(total);
                    headerStartCount++;
                    grandtotal += total;
                }
                worksheet.Cells[rowCountStart, headerStartCount] = grandtotal; // MoneyConverter.ConvertDoubleToMoney(grandtotal);
                string columnAddress = worksheet.Columns[headerStartCount].Address;
                string columnName = GetColumnName(columnAddress);

                var allRange = worksheet.Range["a3", string.Format("{0}{1}", columnName,(rowCountStart))];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private string GetColumnName(string columnAddress)
        {
            string columnName = string.Empty;
            Regex reg = new Regex(@"(\$)(\w*):");
            if (reg.IsMatch(columnAddress))
            {
                Match match = reg.Match(columnAddress);
                columnName = match.Groups[2].Value;
            }
            return columnName;
        }

        private List<IncomeModelHelper> SetIncome(List<Model.Loan> loans, List<Model.Payment> payments, List<Model.Expense> expenses, DateTime fr, DateTime to)
        {
            List<IncomeModelHelper> objList = new List<IncomeModelHelper>();
            var totaldays = to.Subtract(fr).TotalDays;
            for(int i = 0; i <= totaldays; i++)
            {
                var miscFee = loans.Where(t => t.ReleaseDate.Date == fr.AddDays(i).Date).Sum(t => t.Miscellaneous);
                var interest = payments.Where(t => t.Date.Date == fr.AddDays(i).Date).Sum(t => t.Interest);
                var charge = payments.Where(t => t.Date.Date == fr.AddDays(i).Date).Sum(t => t.Charge);
                var expense = expenses.Where(t => t.ExpenseDate.Date == fr.AddDays(i).Date).Sum(t => t.Amount);
                var netIncome = ((miscFee+interest+charge)- expense);
                var obj = new IncomeModelHelper()
                {
                    Date = fr.AddDays(i),
                    MiscFee = miscFee,
                    Interest = interest,
                    Charge = charge,
                    Total = miscFee +interest + charge,
                    LessExp = expense,
                    NetIncome = netIncome
                };
                objList.Add(obj);
            }
            return objList;
        }
        private void CreateIncomeReport(List<IncomeModelHelper> incomes)
        {
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                worksheet.Cells[3, 1] = "DATE";
                worksheet.Cells[3, 2] = "MISC. FEE";
                worksheet.Cells[3, 3] = "INTEREST INCOME";

                worksheet.Cells[3, 4] = "CHARGE INCOME";

                worksheet.Cells[3, 5] = "TOTAL";
                worksheet.Cells[3, 6] = "LESS EXPENSE";
                worksheet.Cells[3, 7] = "NET INCOME";

                int row = 4;
                foreach(var income in incomes)
                {
                    worksheet.Cells[row, 1] = income.Date.ToShortDateString();
                    worksheet.Cells[row, 2] = income.MiscFee; // MoneyConverter.ConvertDoubleToMoney(income.MiscFee);
                    worksheet.Cells[row, 3] = income.Interest; // MoneyConverter.ConvertDoubleToMoney(income.Interest);

                    worksheet.Cells[row, 4] = income.Charge; // MoneyConverter.ConvertDoubleToMoney(income.Charge);

                    worksheet.Cells[row, 5] = income.Total; // MoneyConverter.ConvertDoubleToMoney(income.Total);
                    worksheet.Cells[row, 6] = income.LessExp; // MoneyConverter.ConvertDoubleToMoney(income.LessExp);
                    worksheet.Cells[row, 7] = income.NetIncome; // MoneyConverter.ConvertDoubleToMoney(income.NetIncome);
                    row++;
                }

                var totalMiscFee = incomes.Sum(x => x.MiscFee); // MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.MiscFee));
                var totalInterest = incomes.Sum(x => x.Interest); // MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.Interest));

                var totalCharge = incomes.Sum(x => x.Charge); // MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.Charge));

                var total = incomes.Sum(x => x.Total); // MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.Total));
                var lessExpense = incomes.Sum(x => x.LessExp); // MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.LessExp));
                var netInc = incomes.Sum(x => x.NetIncome);// MoneyConverter.ConvertDoubleToMoney(incomes.Sum(x => x.NetIncome));

                worksheet.Cells[row + 1, 1] = "Total";
                worksheet.Cells[row + 1, 2] = totalMiscFee;
                worksheet.Cells[row + 1, 3] = totalInterest;

                worksheet.Cells[row + 1, 4] = totalCharge;

                worksheet.Cells[row + 1, 5] = total;
                worksheet.Cells[row + 1, 6] = lessExpense;
                worksheet.Cells[row + 1, 7] = netInc;


                var allRange = worksheet.Range["a3", string.Format("g{0}",(row + 1))];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);

            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private static void BorrowersWithDueDate()
        {

        }
        private static void InvestmentRemaining()
        {
            double principals = PaymentManager.GetPrincipalReturns();
            double loanAmount = LoanManager.GetReleaseLoanAmounts();
            double investmentTotal = InvestmentManager.GetInvestmentAmounts();
            double remainingInvestment = (investmentTotal - loanAmount) + principals;

            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                worksheet.Cells[3, 1] = "Total Investment";
                worksheet.Cells[3, 2] = "Total Loan Release";
                worksheet.Cells[3, 3] = "Total Investment Returns";
                worksheet.Cells[3, 4] = "Remaining on Investment";

                worksheet.Cells[4, 1] = investmentTotal; // MoneyConverter.ConvertDoubleToMoney(investmentTotal);
                worksheet.Cells[4, 2] = loanAmount; // MoneyConverter.ConvertDoubleToMoney(loanAmount);
                worksheet.Cells[4, 3] = principals; // MoneyConverter.ConvertDoubleToMoney(principals);
                worksheet.Cells[4, 4] = remainingInvestment; // MoneyConverter.ConvertDoubleToMoney(remainingInvestment);

                var allRange = worksheet.Range["a3", "d4"];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private static void BorrowersWithLoan(List<Model.Loan> loans, bool isDueData)
        {
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                worksheet.Cells[1, 1] = "ACCOUNT CODE";
                worksheet.Cells[1, 2] = "BORROWER";
                worksheet.Cells[1, 3] = "ADDRESS";
                worksheet.Cells[1, 4] = "DATE RELEASED";
                worksheet.Cells[1, 5] = "PRINCIPAL";
                worksheet.Cells[1, 6] = "INTEREST";
                worksheet.Cells[1, 7] = "TOTAL PAYMENTS";
                worksheet.Cells[1, 8] = "AMOUNTS RECEIVABLE";
                if(isDueData)
                {
                    worksheet.Cells[1, 9] = "Last Due Date";
                    worksheet.Cells[1, 10] = "AMOUNT DUE";
                    worksheet.Cells[1, 11] = "COLLECTOR";
                    worksheet.Cells[1, 12] = "REMARKS";
                }

                int count = 2;
                foreach (var loan in loans)
                {
                    var paymentSum = loan.Payments.Sum(x => x.Amount);
                    var totalReceivable = (loan.Principal + loan.Interest) - paymentSum;
                    worksheet.Cells[count, 1].NumberFormat = "@";
                    worksheet.Cells[count, 1].Value = loan.AccountCode;
                    worksheet.Cells[count, 2] = loan.LoanApplication.PersonalData.DisplayName;

                    worksheet.Cells[count, 3] = loan.LoanApplication.PersonalData.HomeAddress; // MoneyConverter.ConvertDoubleToMoney(loan.Principal);
                    worksheet.Cells[count, 4] = loan.ReleaseDate.ToShortDateString();

                    worksheet.Cells[count, 5] = loan.Principal; // MoneyConverter.ConvertDoubleToMoney(loan.Principal);
                    worksheet.Cells[count, 6] = loan.Interest; // MoneyConverter.ConvertDoubleToMoney(loan.Interest);
                    worksheet.Cells[count, 7] = paymentSum; // MoneyConverter.ConvertDoubleToMoney(paymentSum);
                    worksheet.Cells[count, 8] = totalReceivable; // MoneyConverter.ConvertDoubleToMoney(totalReceivable);
                    if (isDueData)
                    {
                        double index = loan.Payments.Sum(p => p.Amount)/loan.Amortization;
                        var current_index = (int)index;
                        var min_sched = loan.PaymentSchedules.OrderBy(ps => ps.Schedule).ToList()[current_index];
                        worksheet.Cells[count, 9] = min_sched.Schedule.ToShortDateString();

                        var lessAmount = 0.0;
                        if ((index % 1) != 0)
                        {
                            var ExactAmount = (current_index) * loan.Amortization;
                            lessAmount = ExactAmount - loan.Payments.Sum(p => p.Amount);
                        }

                        var duedates = loan.PaymentSchedules.Where(psched => psched.Schedule >= min_sched.Schedule && psched.Schedule <= DateTime.Now);
                        if(duedates.Count() > 0)
                        {
                            worksheet.Cells[count, 10] = (loan.Amortization * duedates.Count()) + lessAmount;
                        }
                        //var scheds = loan.PaymentSchedules.Where(paymentSched => paymentSched.Schedule > loan.Payments.Max(payment => payment.PaymentScheduleDate));
                        //int dueCount = 1;
                        //if(scheds != null && scheds.Count() > 0)
                        //{
                        //    var min_sched = scheds.Min(min => min.Schedule).Date.ToShortDateString();
                        //    worksheet.Cells[count, 9] = min_sched;

                        //    var dues = scheds.Where(x => x.Schedule <= DateTime.Now);
                        //    if(dues != null)
                        //    {
                        //        dueCount = dueCount * dues.Count();
                        //    }
                        //}
                        //worksheet.Cells[count, 10] = loan.Amortization * dueCount;
                        //worksheet.Cells[count, 7] = loan.PaymentSchedules.Where(paymentSched => paymentSched.Schedule >= loan.Payments.Max(payment => payment.Date)).Min(min => min.Schedule).Date.ToShortDateString();
                    }
                    count++;
                }
                string colFormat = "h{0}";
                if (isDueData)
                {
                    colFormat = "l{0}";
                }

                var allRange = worksheet.Range["a1", string.Format(colFormat, count)];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private static void IncomeRemaining(List<Model.Payment> payments, List<Model.FixedExpense> fixExpenses,List<Model.Expense> expenses)
        {
            var paymentsTotal = payments.Sum(x => x.Amount);
            var chargeTotal = payments.Sum(x => x.Charge);
            var fixExpTotal = fixExpenses.Sum(x => x.Amount);
            var expTotal = expenses.Sum(x => x.Amount);

            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];

                worksheet.Cells[1, 2] = "INCOME";
                worksheet.Cells[1, 3] = "EXPENSES";
                worksheet.Cells[1, 4] = "REMAINING INCOME";

                worksheet.Cells[2, 1] = "Total Payment Income:";
                worksheet.Cells[2, 2] = paymentsTotal; // MoneyConverter.ConvertDoubleToMoney(paymentsTotal);
                worksheet.Cells[3, 1] = "Total Charge Income";
                worksheet.Cells[3, 2] = chargeTotal; // MoneyConverter.ConvertDoubleToMoney(chargeTotal);

                int count = 4;
                foreach (var fix in fixExpenses)
                {
                    worksheet.Cells[count, 1] = fix.DisplayName;
                    worksheet.Cells[count, 3] = fix.Amount; // MoneyConverter.ConvertDoubleToMoney(fix.Amount);
                    count++;
                }

                foreach (var exp in expenses)
                {
                    worksheet.Cells[count, 1] = exp.DisplayName;
                    worksheet.Cells[count, 3] = exp.Amount; // MoneyConverter.ConvertDoubleToMoney(exp.Amount);
                    count++;
                }

                worksheet.Cells[count+1, 1] = "Total";
                worksheet.Cells[count + 1, 2] = paymentsTotal + chargeTotal; // MoneyConverter.ConvertDoubleToMoney(paymentsTotal+ chargeTotal);
                worksheet.Cells[count + 1, 3] = fixExpTotal + expTotal; // MoneyConverter.ConvertDoubleToMoney(fixExpTotal + expTotal);
                worksheet.Cells[count + 1, 4] = ((paymentsTotal + chargeTotal) - (fixExpTotal + expTotal));// MoneyConverter.ConvertDoubleToMoney((paymentsTotal + chargeTotal)- (fixExpTotal + expTotal));

                var allRange = worksheet.Range["a1", string.Format("d{0}", count+1)];
                allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path, filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void GenerateAgingReport()
        {
            var Agings = GetAgings();
            var application = new Excel.Application();
            if(application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];
                worksheet.Cells[3, 1] = "ACCOUNT NO.";
                worksheet.Cells[3, 2] = "VOUCHER NO.";
                worksheet.Cells[3, 3] = "NAME OF CUSTOMER";
                worksheet.Cells[3, 4] = "ADDRESS";
                worksheet.Cells[3, 5] = "CONTACT NO.";
                worksheet.Cells[3, 6] = "AMOUNT LOAN";
                worksheet.Cells[3, 7] = "DATE RELEASED";
                worksheet.Cells[3, 8] = "TERMS OF CREDIT";
                worksheet.Cells[3, 9] = "INTEREST RATE PER MONTH";
                worksheet.Cells[3, 10] = "INTEREST";
                worksheet.Cells[3, 11] = "TOTAL";
                worksheet.Cells[3, 12] = "AMORTIZATION/SEMI MONTHLY";
                worksheet.Cells[3, 13] = "INTEREST";
                worksheet.Cells[3, 14] = "CAPITAL";
                worksheet.Cells[3, 15] = "TOTAL PAYMENTS";
                worksheet.Cells[3, 16] = "BALANCE";
                worksheet.Cells[3, 17] = "ACCOUNT STATUS";
                worksheet.Cells[3, 18] = "This Month";
                worksheet.Cells[3, 19] = "Second Month";
                worksheet.Cells[3, 20] = "Third Month";
                worksheet.Cells[3, 21] = "Fourth Month";
                worksheet.Cells[3, 22] = "Fifth Month";
                worksheet.Cells[3, 23] = "Six Month";
                worksheet.Cells[3, 24] = "Seventh Month";
                worksheet.Cells[3, 25] = "Eight Month";
                worksheet.Cells[3, 26] = "Ninth Month";
                worksheet.Cells[3, 27] = "Tenth Month";
                worksheet.Cells[3, 28] = "Eleventh Month";
                worksheet.Cells[3, 29] = "Twelfth Month";
                //worksheet.Cells[3, 29] = "Thirteenth Month";
                worksheet.Cells[3, 30] = "1Yr. & Above";

                var headerRange = worksheet.Range["a3", "ac3"];
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                var rowIndex = 4;
                foreach(var aging in Agings)
                {
                    worksheet.Cells[rowIndex, 1].NumberFormat = "@";
                    worksheet.Cells[rowIndex, 1].Value = aging.AccountNumber;

                    worksheet.Cells[rowIndex, 2].NumberFormat = "@";
                    worksheet.Cells[rowIndex, 2].Value = aging.Voucher;
                    worksheet.Cells[rowIndex, 3] = aging.DisplayName;
                    worksheet.Cells[rowIndex, 4] = aging.Address;
                    worksheet.Cells[rowIndex, 5] = aging.ContactNo;
                    worksheet.Cells[rowIndex, 6] = aging.AmountLoan;
                    worksheet.Cells[rowIndex, 7] = aging.DateReleased;
                    worksheet.Cells[rowIndex, 8] = aging.Terms;
                    worksheet.Cells[rowIndex, 9] = aging.InterestRate;
                    worksheet.Cells[rowIndex, 10] = aging.Interest;
                    worksheet.Cells[rowIndex, 11] = aging.Total;
                    worksheet.Cells[rowIndex, 12] = aging.Amortization;
                    worksheet.Cells[rowIndex, 13] = aging.PaymentsInterest;
                    worksheet.Cells[rowIndex, 14] = aging.PaymentsCapital;
                    worksheet.Cells[rowIndex, 15] = aging.PaymentsTotal;
                    worksheet.Cells[rowIndex, 16] = aging.PaymetsBalance;
                    worksheet.Cells[rowIndex, 17] = aging.AccountStatus;
                    worksheet.Cells[rowIndex, 18] = aging.CurrentMonth;
                    worksheet.Cells[rowIndex, 19] = aging.SecondMonth;
                    worksheet.Cells[rowIndex, 20] = aging.ThirdMonth;
                    worksheet.Cells[rowIndex, 21] = aging.FourthMonth;
                    worksheet.Cells[rowIndex, 22] = aging.FifthMonth;
                    worksheet.Cells[rowIndex, 23] = aging.SixthMonth;
                    worksheet.Cells[rowIndex, 24] = aging.SeventhMonth;
                    worksheet.Cells[rowIndex, 25] = aging.EightMonth;
                    worksheet.Cells[rowIndex, 26] = aging.NinthMonth;
                    worksheet.Cells[rowIndex, 27] = aging.TenthMonth;
                    worksheet.Cells[rowIndex, 28] = aging.EleventhMonth;
                    worksheet.Cells[rowIndex, 29] = aging.TwelfthMonth;
                    worksheet.Cells[rowIndex, 30] = aging.Thirteenth;
                    //worksheet.Cells[rowIndex, 30] = aging.OneYearAbove;
                    rowIndex++;
                }

                //var contentRange = worksheet.Range["a4", string.Format("ac{0}", rowIndex)];
                var contentRange = worksheet.Range["a4", string.Format("ad{0}", rowIndex)];
                contentRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //var paymetsRange = worksheet.Range["l2", "n2"];
                var paymetsRange = worksheet.Range["m2", "o2"];
                paymetsRange.Merge();
                worksheet.Cells[2, 12] = "PAYMENTS";
                paymetsRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                worksheet.Rows.AutoFit();
                worksheet.Range["a1", "ac4"].Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //worksheet.Range["a1", "ac3"].Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                var path = System.IO.Path.GetTempPath();
                var filename = System.IO.Path.GetRandomFileName();
                var fullPath = string.Format("{0}{1}.xlsx", path,filename);
                workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
                    missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                workBook.Close(true, missing, missing);
                application.Quit();
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        private static List<ModelHelper.AgingHelperModel> GetAgings()
        {
            Loans = LoanManager.Get().ToList();
            List<ModelHelper.AgingHelperModel> Agings = new List<ModelHelper.AgingHelperModel>();
            foreach (var loan in Loans)
            {
                string accountNumber = string.Empty;
                if (loan.LoanApplication.PersonalData.PersonalDataAccounts != null && loan.LoanApplication.PersonalData.PersonalDataAccounts.Count() > 0)
                {
                    accountNumber = loan.LoanApplication.PersonalData.PersonalDataAccounts.FirstOrDefault().AccountNumber;
                }

                var dummy = new ModelHelper.AgingHelperModel()
                {
                    AccountNumber = accountNumber,
                    Voucher = loan.AccountCode,
                    DisplayName = loan.LoanApplication.PersonalData.DisplayName,
                    Address = loan.LoanApplication.PersonalData.HomeAddress,
                    ContactNo = loan.LoanApplication.PersonalData.ContactNumber,
                    AmountLoan = loan.Principal.ToString(),// MoneyConverter.ConvertDoubleToMoney(loan.Principal),
                    DateReleased = loan.ReleaseDate.ToShortDateString(),
                    Terms = loan.LoanTerm,
                    InterestRate = string.Format("{0}%", loan.InterestRate),
                    Interest = loan.Interest.ToString(), // MoneyConverter.ConvertDoubleToMoney(loan.Interest),
                    Total = (loan.Principal + loan.Interest).ToString(),// MoneyConverter.ConvertDoubleToMoney(loan.Principal + loan.Interest),
                    Amortization = loan.Amortization.ToString(), // MoneyConverter.ConvertDoubleToMoney(loan.Amortization),
                };
            dummy.PaymentsInterest = loan.Payments.Sum(x => x.Interest).ToString();// MoneyConverter.ConvertDoubleToMoney(loan.Payments.Sum(x => x.Interest));
            dummy.PaymentsCapital = loan.Payments.Sum(x => x.Principal).ToString(); // MoneyConverter.ConvertDoubleToMoney(loan.Payments.Sum(x => x.Principal));
            dummy.PaymentsTotal = loan.Payments.Sum(x => x.Amount).ToString(); // MoneyConverter.ConvertDoubleToMoney(loan.Payments.Sum(x => x.Amount));
            dummy.PaymetsBalance = ((loan.Principal + loan.Interest) - loan.Payments.Sum(x => x.Amount)).ToString(); // MoneyConverter.ConvertDoubleToMoney((loan.Principal + loan.Interest) - loan.Payments.Sum(x => x.Amount));

                var totalLoan = loan.Interest + loan.Principal;
                var totalPayments = loan.Payments.Sum(x => x.Amount);
                if((totalLoan- totalPayments) > 0 && loan.Status.Name != "Loan.AtLarge")
                {
                    var lastPaymentDate = loan.Payments.Count() > 0 ? loan.Payments.Max(x => x.Date) : loan.FirstDueDate;
                    if (DateTime.Now >= lastPaymentDate.AddYears(1))
                    {
                        dummy.AccountStatus = "WO";
                    }
                    else
                    {
                        dummy.AccountStatus = "CU";
                    }
                }
                else
                {
                    dummy.AccountStatus = "FP";
                }

                if ((totalLoan - totalPayments) > 0)
                {
                    HelperClient.AgingClientHelper.MonthlyAgingReport(loan, dummy);
                }
                Agings.Add(dummy);
            }
            return Agings;
        }
        
    }
}
