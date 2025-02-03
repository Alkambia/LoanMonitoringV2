using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using Alkambia.App.LoanMonitoring.Helper.Converter;
using System.Drawing;
using Alkambia.App.LoanMonitoring.Helper;

namespace Alkambia.WPF.LoanMonitoring.Controller.ReportExtensions
{
    public class PaymentSummaryReport
    {
        //public PaymentSummaryReport(Model.Loan loan)
        //{
        //    try
        //    {
        //        var collectors = AccountManager.Get(5).ToList();
        //        var application = new Excel.Application();
        //        if (application != null)
        //        {
        //            object missing = System.Reflection.Missing.Value;
        //            Excel.Workbook workBook = application.Workbooks.Add(missing);
        //            var worksheet = (Excel.Worksheet)workBook.Worksheets.Item[1];
        //            var payments = loan.Payments.OrderBy(x => x.Date);
        //            double totalBalance = 0;

        //            //Header
        //            worksheet.Cells[1, 1] = "ACCOUNT CODE";
        //            worksheet.Cells[1, 2] = "DATE";
        //            worksheet.Cells[1, 3] = "OR NUMBER";
        //            worksheet.Cells[1, 4] = "PRINCIPAL";
        //            worksheet.Cells[1, 5] = "INTEREST";
        //            worksheet.Cells[1, 6] = "CHARGE";
        //            worksheet.Cells[1, 7] = "TOTAL PAYMENTS";
        //            worksheet.Cells[1, 8] = "BALANCE";
        //            worksheet.Cells[1, 9] = "COLLECTOR";
        //            var rowCount = 2;
        //            foreach (var pay in payments)
        //            {
        //                totalBalance += pay.Amount;
        //                var balance = (loan.Principal + loan.Interest) - totalBalance;
        //                worksheet.Cells[rowCount, 1].NumberFormat = "@";
        //                worksheet.Cells[rowCount, 1].Value = pay.Loan.AccountCode;
        //                worksheet.Cells[rowCount, 2] = pay.Date.ToShortDateString();
        //                worksheet.Cells[rowCount, 3] = pay.ORNumber;
        //                worksheet.Cells[rowCount, 4] = pay.Principal; // MoneyConverter.ConvertDoubleToMoney(pay.Principal);
        //                worksheet.Cells[rowCount, 5] = pay.Interest; // MoneyConverter.ConvertDoubleToMoney(pay.Interest);
        //                worksheet.Cells[rowCount, 6] = pay.Charge; // MoneyConverter.ConvertDoubleToMoney(pay.Charge);
        //                worksheet.Cells[rowCount, 7] = (pay.Amount + pay.Charge);//MoneyConverter.ConvertDoubleToMoney(pay.Amount + pay.Charge);
        //                worksheet.Cells[rowCount, 8] = balance; // MoneyConverter.ConvertDoubleToMoney(balance);
        //                worksheet.Cells[rowCount, 9] = collectors.Single(x => x.AccountID == pay.CollertorID).DisplayName;
        //                rowCount++;
        //            }

        //            var allRange = worksheet.Range["a1", string.Format("i{0}", rowCount)];
        //            allRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

        //            worksheet.Columns.AutoFit();
        //            worksheet.Rows.AutoFit();

        //            var path = System.IO.Path.GetTempPath();
        //            var filename = System.IO.Path.GetRandomFileName();
        //            var fullPath = string.Format("{0}{1}.xlsx", path, filename);
        //            workBook.SaveAs(string.Format(@"{0}{1}", path, string.Format("{0}.xlsx", filename)), Excel.XlFileFormat.xlOpenXMLWorkbook, missing,
        //                missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
        //            workBook.Close(true, missing, missing);
        //            application.Quit();
        //            System.Diagnostics.Process.Start(fullPath);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("An error occured while creating a summary, please contact you system administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //}

        public PaymentSummaryReport(Model.Loan LoanClass)
        {
            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.doc", temppath, filename);


            var application = new Word.Application();

            if (application != null)
            {
                application.ShowAnimation = false;
                object missing = System.Reflection.Missing.Value;

                var document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                document.PageSetup.LeftMargin = Convert.ToSingle(10);
                document.PageSetup.RightMargin = Convert.ToSingle(10);
                document.PageSetup.TopMargin = Convert.ToSingle(10);
                document.PageSetup.BottomMargin = Convert.ToSingle(10);
                document.PageSetup.PaperSize = Word.WdPaperSize.wdPaperLetter;

                var para0 = document.Content.Paragraphs.Add(ref missing);
                para0.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                Word.Table firstTable = document.Tables.Add(para0.Range, 8, 6, ref missing, ref missing);
                firstTable.AllowAutoFit = true;

                firstTable.Range.Font.Name = "Calibri Light";
                firstTable.Range.Font.Size = float.Parse("9");
                firstTable.Range.Paragraphs.SpaceAfter = 0;

                //Row 1
                firstTable.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[1].Cells[1].Range.Text = "Account No.:";
                if (LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.Count() > 0)
                {
                    firstTable.Rows[1].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.First().AccountNumber;
                }

                firstTable.Rows[1].Cells[4].Range.Text = "Schedule:";
                firstTable.Rows[1].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

                if (LoanClass.LoanApplication.PersonalData.DigitalInfos.Count() > 0)
                {
                    Image profileImage = new Bitmap(CameraHelper.ByteArrayToImage(LoanClass.LoanApplication.PersonalData.DigitalInfos.FirstOrDefault().Photo),
                        new System.Drawing.Size(110, 110));

                    Clipboard.SetDataObject(profileImage);
                    firstTable.Rows[1].Cells[6].Range.Paste();
                    firstTable.Rows[1].Cells[6].Range.InsertParagraphAfter();
                }



                string schedule = string.Empty;
                var schedType = LoanClass.ScheduleTypes.First();
                if (schedType.Type == 0)//Perday
                {
                    schedule = "Daily";
                }
                else if (schedType.Type == 2)
                {
                    schedule = "SEMI MONTHLY";
                }
                else
                {
                    schedule = "MONTHLY";
                }
                firstTable.Rows[1].Cells[5].Range.Text = schedule;

                //Row 2
                //firstTable.Rows[2].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[2].Cells[1].Range.Text = "Name of Borrower:";
                //firstTable.Rows[2].Cells[2].Range.Text = string.Format("{0}, {1}", LoanClass.LoanApplication.PersonalData.LastName, LoanClass.LoanApplication.PersonalData.FirstName);
                firstTable.Rows[2].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[2].Cells[1].Range.Text = "Voucher Number:";
                firstTable.Rows[2].Cells[2].Range.Text = LoanClass.AccountCode;
                firstTable.Rows[2].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[2].Cells[4].Range.Text = "Amortization:";
                firstTable.Rows[2].Cells[5].Range.Text = string.Format("{0}", MoneyConverter.ConvertDoubleToMoney(LoanClass.Amortization));



                //Row 3

                //firstTable.Rows[3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[3].Cells[1].Range.Text = "Date Release:";
                //firstTable.Rows[3].Cells[2].Range.Text = LoanClass.ReleaseDate.ToShortDateString();
                firstTable.Rows[3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[3].Cells[1].Range.Text = "Name of Borrower:";
                firstTable.Rows[3].Cells[2].Range.Text = string.Format("{0}, {1}", LoanClass.LoanApplication.PersonalData.LastName, LoanClass.LoanApplication.PersonalData.FirstName);
                firstTable.Rows[3].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[3].Cells[4].Range.Text = "Terms Of Credit:";
                firstTable.Rows[3].Cells[5].Range.Text = LoanClass.LoanApplication.CreditTerm.DisplayName;

                //Row 4
                //firstTable.Rows[4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[4].Cells[1].Range.Text = "First Due Date:";
                //firstTable.Rows[4].Cells[2].Range.Text = LoanClass.FirstDueDate.ToShortDateString();
                firstTable.Rows[4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[4].Cells[1].Range.Text = "Date Release:";
                firstTable.Rows[4].Cells[2].Range.Text = LoanClass.ReleaseDate.ToShortDateString();
                firstTable.Rows[4].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[4].Cells[4].Range.Text = "Interest Rate:";
                firstTable.Rows[4].Cells[5].Range.Text = string.Format("{0}% per Month", LoanClass.InterestRate);

                //Row 5
                //firstTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[5].Cells[1].Range.Text = "Maturity:";
                //firstTable.Rows[5].Cells[2].Range.Text = LoanClass.MaturityDate.ToShortDateString();
                firstTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[5].Cells[1].Range.Text = "First Due Date:";
                firstTable.Rows[5].Cells[2].Range.Text = LoanClass.FirstDueDate.ToShortDateString();
                firstTable.Rows[5].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[5].Cells[4].Range.Text = "Principal:";
                firstTable.Rows[5].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal);

                //Row 6
                //firstTable.Rows[6].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[6].Cells[1].Range.Text = "Address:";
                //firstTable.Rows[6].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.HomeAddress;
                firstTable.Rows[6].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[6].Cells[1].Range.Text = "Maturity:";
                firstTable.Rows[6].Cells[2].Range.Text = LoanClass.MaturityDate.ToShortDateString();
                firstTable.Rows[6].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[6].Cells[4].Range.Text = "Interest:";
                firstTable.Rows[6].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Interest);

                //Row 7
                //firstTable.Rows[7].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                //firstTable.Rows[7].Cells[1].Range.Text = "Contact Number:";
                //firstTable.Rows[7].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.ContactNumber;
                firstTable.Rows[7].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[7].Cells[1].Range.Text = "Address:";
                firstTable.Rows[7].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.HomeAddress;


                //row8
                firstTable.Rows[8].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[8].Cells[1].Range.Text = "Contact Number:";
                firstTable.Rows[8].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.ContactNumber;
                firstTable.Rows[1].Cells[6].Merge(firstTable.Rows[8].Cells[6]);

                var para1 = document.Content.Paragraphs.Add(ref missing);
                para1.Range.Text = "LEDGER CARD";
                para1.Range.Font.Name = "Calibri Light";
                para1.Range.Font.Size = float.Parse("10");
                para1.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                para1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.InsertParagraphAfter();


                Word.Table secondTable = document.Tables.Add(para1.Range, LoanClass.Payments.Count() + 1, 7, ref missing, ref missing);
                //secondTable.Borders.Enable = 1;

                secondTable.Range.Paragraphs.SpaceAfter = 0;

                var payments = LoanClass.Payments.OrderBy(x => x.Date).ToList();
                double totalBalance = 0;

                List<string> headers = new List<string>(new string[]
                {
                "DATE",
                "OR NO.",
                "PRINCIPAL AMOUNT",
                "INTEREST",
                "CHARGES",
                "TOTAL AMOUNT PAID",
                "BALANCE"
                });

                secondTable.AllowAutoFit = true;

                secondTable.Range.Font.Name = "Calibri Light";
                secondTable.Range.Font.Size = float.Parse("8");

                int row_count = -1;
                foreach (Word.Row row in secondTable.Rows)
                {
                    if (row_count < payments.Count())
                    {
                        int col_count = 0;
                        foreach (Word.Cell cell in row.Cells)
                        {
                            if (cell.RowIndex == 1)
                            {
                                cell.Range.Text = headers[col_count];
                                cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                            }
                            else
                            {
                                if (col_count == 0)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].Date.ToShortDateString());
                                }
                                if (col_count == 1)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].ORNumber);
                                }
                                if (col_count == 2)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].Principal);
                                }
                                if (col_count == 3)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].Interest);
                                }
                                if (col_count == 4)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].Charge);
                                }
                                if (col_count == 5)
                                {
                                    cell.Range.Text = string.Format("{0}", payments[row_count].Amount);
                                }
                                if (col_count == 6)
                                {
                                    totalBalance += payments[row_count].Amount;
                                    var balance = (LoanClass.Principal + LoanClass.Interest) - totalBalance;
                                    cell.Range.Text = string.Format("{0}", balance);
                                }
                                cell.Range.Font.Name = "Calibri Light";
                                cell.Range.Font.Size = float.Parse("10");
                                cell.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            }
                            cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            col_count++;
                        }
                    }

                    row_count++;
                }
                //Save the document
                document.SaveAs2(currentPath);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                application.Quit(ref missing, ref missing, ref missing);
                application = null;
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        //public PaymentSummaryReport(Model.Loan LoanClass)
        //{
        //    var temppath = System.IO.Path.GetTempPath();
        //    var filename = System.IO.Path.GetRandomFileName();
        //    var currentPath = string.Format("{0}{1}.doc", temppath, filename);


        //    var application = new Word.Application();

        //    if (application != null)
        //    {
        //        application.ShowAnimation = false;
        //        object missing = System.Reflection.Missing.Value;

        //        var document = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
        //        document.PageSetup.LeftMargin = Convert.ToSingle(10);
        //        document.PageSetup.RightMargin = Convert.ToSingle(10);
        //        document.PageSetup.TopMargin = Convert.ToSingle(10);
        //        document.PageSetup.BottomMargin = Convert.ToSingle(10);
        //        document.PageSetup.PageWidth = 576;
        //        document.PageSetup.PageHeight = 360;

        //        var para0 = document.Content.Paragraphs.Add(ref missing);
        //        para0.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
        //        Word.Table firstTable = document.Tables.Add(para0.Range, 7, 6, ref missing, ref missing);
        //        firstTable.AllowAutoFit = true;

        //        firstTable.Range.Font.Name = "Calibri";
        //        firstTable.Range.Font.Size = 9;
        //        firstTable.Range.Paragraphs.SpaceAfter = 0;

        //        //Row 1
        //        firstTable.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[1].Cells[1].Range.Text = "Account No.:";
        //        firstTable.Rows[1].Cells[2].Range.Text = LoanClass.AccountCode;
        //        firstTable.Rows[1].Cells[4].Range.Text = "Schedule:";
        //        firstTable.Rows[1].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

        //        if (LoanClass.LoanApplication.PersonalData.DigitalInfos.Count() > 0)
        //        {
        //            Image profileImage = new Bitmap(CameraHelper.ByteArrayToImage(LoanClass.LoanApplication.PersonalData.DigitalInfos.FirstOrDefault().Photo), new System.Drawing.Size(150, 150));

        //            Clipboard.SetDataObject(profileImage);
        //            firstTable.Rows[1].Cells[6].Range.Paste();
        //            firstTable.Rows[1].Cells[6].Range.InsertParagraphAfter();
        //        }



        //        string schedule = string.Empty;
        //        var schedType = LoanClass.ScheduleTypes.First();
        //        if (schedType.Type == 0)//Perday
        //        {
        //            schedule = "Daily";
        //        }
        //        else if (schedType.Type == 2)
        //        {
        //            schedule = "SEMI MONTHLY";
        //        }
        //        else
        //        {
        //            schedule = "MONTHLY";
        //        }
        //        firstTable.Rows[1].Cells[5].Range.Text = schedule;

        //        //Row 2
        //        firstTable.Rows[2].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[2].Cells[1].Range.Text = "Name of Borrower:";
        //        firstTable.Rows[2].Cells[2].Range.Text = string.Format("{0}, {1}", LoanClass.LoanApplication.PersonalData.LastName, LoanClass.LoanApplication.PersonalData.FirstName);
        //        firstTable.Rows[2].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[2].Cells[4].Range.Text = "Amortization:";
        //        firstTable.Rows[2].Cells[5].Range.Text = string.Format("{0}", MoneyConverter.ConvertDoubleToMoney(LoanClass.Amortization));



        //        //Row 3
        //        firstTable.Rows[3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[3].Cells[1].Range.Text = "Date Release:";
        //        firstTable.Rows[3].Cells[2].Range.Text = LoanClass.ReleaseDate.ToShortDateString();
        //        firstTable.Rows[3].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[3].Cells[4].Range.Text = "Terms Of Credit:";
        //        firstTable.Rows[3].Cells[5].Range.Text = LoanClass.LoanApplication.CreditTerm.DisplayName;

        //        //Row 4
        //        firstTable.Rows[4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[4].Cells[1].Range.Text = "First Due Date:";
        //        firstTable.Rows[4].Cells[2].Range.Text = LoanClass.FirstDueDate.ToShortDateString();
        //        firstTable.Rows[4].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[4].Cells[4].Range.Text = "Interest Rate:";
        //        firstTable.Rows[4].Cells[5].Range.Text = string.Format("{0}% per Month", LoanClass.InterestRate);

        //        //Row 5
        //        firstTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[5].Cells[1].Range.Text = "Maturity:";
        //        firstTable.Rows[5].Cells[2].Range.Text = LoanClass.MaturityDate.ToShortDateString();
        //        firstTable.Rows[5].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[5].Cells[4].Range.Text = "Principal:";
        //        firstTable.Rows[5].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal);

        //        //Row 6
        //        firstTable.Rows[6].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[6].Cells[1].Range.Text = "Address:";
        //        firstTable.Rows[6].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.HomeAddress;
        //        firstTable.Rows[6].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[6].Cells[4].Range.Text = "Interest:";
        //        firstTable.Rows[6].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Interest);

        //        //Row 7
        //        firstTable.Rows[7].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[7].Cells[1].Range.Text = "Contact Number:";
        //        firstTable.Rows[7].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.ContactNumber;
        //        firstTable.Rows[1].Cells[6].Merge(firstTable.Rows[7].Cells[6]);

        //        var para1 = document.Content.Paragraphs.Add(ref missing);
        //        para1.Range.Text = "LEDGER CARD";
        //        para1.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
        //        para1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //        para1.Range.InsertParagraphAfter();

        //        Word.Table secondTable = document.Tables.Add(para1.Range, LoanClass.PaymentSchedules.Count() + 4, 11, ref missing, ref missing);
        //        //secondTable.Borders.Enable = 1;
        //        secondTable.Range.Paragraphs.SpaceAfter = 0;

        //        var schedules = LoanClass.PaymentSchedules.OrderBy(x => x.Schedule).ToList();

        //        List<string> headers = new List<string>(new string[]
        //        {
        //        "SHEDULE OF PAYMENT",
        //        "DATE OF PAYMENTS",
        //        "OR NO.",
        //        "PRINCIPAL AMOUNT",
        //        "INTEREST",
        //        "REBATES",
        //        "CHARGES",
        //        "TOTAL AMOUNT PAID",
        //        "BALANCE"
        //        });

        //        secondTable.AllowAutoFit = true;

        //        int row_count = -1;
        //        foreach (Word.Row row in secondTable.Rows)
        //        {
        //            if (row_count < schedules.Count())
        //            {
        //                int col_count = 0;
        //                foreach (Word.Cell cell in row.Cells)
        //                {
        //                    if (cell.RowIndex == 1)
        //                    {
        //                        if (col_count == 0)
        //                        {
        //                            cell.Merge(row.Cells[3]);
        //                        }
        //                        cell.Range.Text = headers[col_count];
        //                        cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
        //                    }
        //                    else
        //                    {
        //                        if (col_count == 0)
        //                        {
        //                            cell.Range.Text = string.Format("{0}", schedules[row_count].Schedule.ToShortDateString());
        //                        }
        //                        if (col_count == 1)
        //                        {
        //                            cell.Range.Text = string.Format("{0}", (row_count + 1));
        //                        }
        //                        if (col_count == 2)
        //                        {
        //                            cell.Range.Text = string.Format("{0}", schedules[row_count].InstallmentAmount);
        //                        }
        //                    }
        //                    cell.Range.Font.Name = "Calibri";
        //                    cell.Range.Font.Size = 8;
        //                    cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //                    cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                    col_count++;
        //                }
        //            }

        //            row_count++;
        //        }

        //        secondTable.Rows[schedules.Count() + 3].Cells[1].Range.Text = "Total";
        //        secondTable.Rows[schedules.Count() + 3].Cells[1].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //        secondTable.Rows[schedules.Count() + 3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

        //        secondTable.Rows[schedules.Count() + 3].Cells[3].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal + LoanClass.Interest);
        //        secondTable.Rows[schedules.Count() + 3].Cells[3].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //        secondTable.Rows[schedules.Count() + 3].Cells[3].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

        //        secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Text = string.Format("{0} Pesos", MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal + LoanClass.Interest));
        //        secondTable.Rows[schedules.Count() + 4].Cells.Merge();
        //        secondTable.Rows[schedules.Count() + 4].Cells[1].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //        secondTable.Rows[schedules.Count() + 4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //        secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Bold = 0;
        //        //Save the document
        //        document.SaveAs2(currentPath);
        //        document.Close(ref missing, ref missing, ref missing);
        //        document = null;
        //        application.Quit(ref missing, ref missing, ref missing);
        //        application = null;
        //        System.Diagnostics.Process.Start(currentPath);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Excel is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    }
        //}

    }
}
