using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.WPF.LoanMonitoring.ModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using Alkambia.App.LoanMonitoring.Helper.Converter;
using System.Text.RegularExpressions;

namespace Alkambia.WPF.LoanMonitoring.Controller.ReportExtensions
{
    public class DisbursementSummaryReport
    {
        public DisbursementSummaryReport(DateTime fromDate, DateTime toDate)
        {
            var expenses = ExpenseManager.Get(fromDate, toDate).ToList();
            var ExpensesAll = SetExpenses(expenses, fromDate, toDate);
            CreateExpenseReport(ExpensesAll, fromDate, toDate);
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

        private void CreateExpenseReport(List<ExpenseModelHelper> Expenses, DateTime fr, DateTime to)
        {
            var application = new Excel.Application();
            if (application != null)
            {
                object missing = System.Reflection.Missing.Value;
                Excel.Workbook workBook = application.Workbooks.Add(missing);
                var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];
                //Title
                worksheet.Cells[1, 1] = "Cash Disbursement";
                

                //set header
                worksheet.Cells[5, 1] = "Date";
                worksheet.Cells[5, 2] = "Particulars";
                worksheet.Cells[5, 3] = "Invoice #";
                worksheet.Cells[5, 4] = "Cash";
                int headerStartCount = 5;

                //SetCommonHeaderFirst
                var commonHeaders = Expenses.GroupBy(x => x.DisplayName).Where(g => g.Count() > 1).Select(t => t.First()).ToList();
                var noCommon = Expenses.Where(p => !commonHeaders.Any(p2 => p2.DisplayName == p.DisplayName)).ToList();
                var reportHeaders = commonHeaders.Union(noCommon).ToList();
                foreach (var head in reportHeaders)
                {
                    worksheet.Cells[5, headerStartCount] = head.DisplayName;
                    head.CellIndex = headerStartCount;
                    headerStartCount++;
                }

                //worksheet.Columns[heads.FirstOrDefault().CellIndex].Address
                var addr = worksheet.Columns[headerStartCount + 3].Address;
                string headerColumn = GetColumnName(addr);
                worksheet.Range["a1", string.Format("{0}{1}",headerColumn, 1)].Merge();
                worksheet.Cells.Range["a1", string.Format("{0}1", headerColumn)].Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                var invoiceNumbers = Expenses.GroupBy(x => x.InvoiceNumber).Select(t => t.First()).ToList();
                //set Row
                int rowCountStart = 6;
                foreach(var x in invoiceNumbers)
                {
                    var cash = Expenses.Where(t => t.InvoiceNumber == x.InvoiceNumber).Sum(t => t.Amount);
                    worksheet.Cells[rowCountStart, 1] = x.Date.ToShortDateString();
                    worksheet.Cells[rowCountStart, 2] = x.Particular;
                    worksheet.Cells[rowCountStart, 3] = x.InvoiceNumber;
                    worksheet.Cells[rowCountStart, 4] = cash;
                    headerStartCount = 5;

                    foreach (var h in reportHeaders)
                    {
                        var data = Expenses.Where(t => t.InvoiceNumber == x.InvoiceNumber && t.DisplayName == h.DisplayName).ToList();
                        if (data.Count() > 0)
                        {
                            worksheet.Cells[rowCountStart, headerStartCount] = data.Sum(t => t.Amount);
                        }

                        headerStartCount++;
                    }
                    rowCountStart++;

                }
                //cash total
                double cashtotal = Expenses.Sum(t => t.Amount);
                worksheet.Cells[rowCountStart + 7, 1] = "TOTAL";
                worksheet.Cells[rowCountStart + 7, 4] = cashtotal;
                worksheet.Cells[rowCountStart + 7, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                Excel.Borders border = worksheet.Cells[rowCountStart + 7, 4].Borders;
                border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                border[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

                headerStartCount = 5;
                
                foreach (var h in reportHeaders)
                {
                    var total = Expenses.Where(x => x.DisplayName == h.DisplayName).Sum(x => x.Amount);
                    worksheet.Cells[rowCountStart + 7, headerStartCount] = total;
                    worksheet.Cells[rowCountStart + 7, headerStartCount].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    border = worksheet.Cells[rowCountStart + 7, headerStartCount].Borders;
                    border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                    border[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerStartCount++;
                }

                //Summary
                worksheet.Cells[rowCountStart + 9, 1] = "Summary";
                worksheet.Range[string.Format("a{0}", rowCountStart + 9), string.Format("c{0}", rowCountStart + 9)].Merge();
                worksheet.Cells[rowCountStart + 10, 2] = "Dr";
                worksheet.Cells[rowCountStart + 10, 3] = "Cr";

                worksheet.Cells[rowCountStart + 11, 1] = "Cash";
                worksheet.Cells[rowCountStart + 11, 2] = cashtotal;


                rowCountStart = rowCountStart + 12;

                double allTotal = 0;
                foreach (var exp in reportHeaders)
                {
                    var totalSum = Expenses.Where(x => x.DisplayName == exp.DisplayName).Sum(x => x.Amount);
                    worksheet.Cells[rowCountStart, 1] = exp.DisplayName;
                    worksheet.Cells[rowCountStart, 3] = totalSum;
                    allTotal = allTotal + totalSum;
                    rowCountStart++;
                }

                //Total
                worksheet.Cells[rowCountStart, 1] = "Total";
                worksheet.Cells[rowCountStart, 2] = cashtotal;
                worksheet.Cells[rowCountStart, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                border = worksheet.Cells[rowCountStart, 2].Borders;
                border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;

                worksheet.Cells[rowCountStart, 3] = allTotal;
                worksheet.Cells[rowCountStart, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                border = worksheet.Cells[rowCountStart, 3].Borders;
                border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;

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
                var upperHeaderSplit = !string.IsNullOrEmpty(exp.DisplayName) ? exp.DisplayName.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
                var obj = new ExpenseModelHelper
                {
                    Date = exp.ExpenseDate,
                    CellIndex = 0,
                    UpperHeader = string.Empty,
                    DisplayName = upperHeaderSplit.Count() > 1 ? upperHeaderSplit[0] : exp.DisplayName,
                    InvoiceNumber = exp.Name,
                    Particular = exp.Description,
                    Amount = exp.Amount
                };
                objList.Add(obj);
            }



            return objList.OrderBy(x => x.Date).ToList();
        }
    }
}
