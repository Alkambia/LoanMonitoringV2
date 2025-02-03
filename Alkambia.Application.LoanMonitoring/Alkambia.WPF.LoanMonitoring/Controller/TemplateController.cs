using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using DBModel = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.Helper.Converter;
using System.Windows;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Drawing;
using Alkambia.App.LoanMonitoring.Helper;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class TemplateController
    {

        public static void LoadContract(DBModel.Loan LoanClass, string document)
        {
            //var currentName = string.Format("Contract{0}{1}{2}.doc", LoanClass.LoanApplication.PersonalData.LastName,DateTime.UtcNow.ToLongDateString().Replace(",",""),Guid.NewGuid()).Replace(" ","");
            var dir = System.IO.Directory.GetCurrentDirectory();

            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.doc", temppath, filename);

            //var currentPath = string.Format(@"{0}\Documents\{1}", dir, currentName);

            //Copy
            System.IO.File.Copy(string.Format(@"{0}\Documents\{1}", dir, document), currentPath);

            //Modify
            var application = new Word.Application();
            if (application != null)
            {
                application.Documents.Open(currentPath);

                Word.Range range = application.ActiveDocument.Content;
                range.Find.ClearFormatting();
                range.Find.Execute(FindText: "{DisplayName}", ReplaceWith: LoanClass.LoanApplication.PersonalData.DisplayName, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm}", ReplaceWith: LoanClass.LoanTerm, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.LoanTerm), Replace: Word.WdReplace.wdReplaceAll);

                var monthlyBill = ((LoanClass.Principal + LoanClass.Interest) / LoanClass.LoanTerm);
                range.Find.Execute(FindText: "{MonthlyBill}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(monthlyBill), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{MonthlyBill.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(monthlyBill), Replace: Word.WdReplace.wdReplaceAll);

                range.Find.Execute(FindText: "{Amortization}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Amortization), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Amortization.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Amortization), Replace: Word.WdReplace.wdReplaceAll);

                range.Find.Execute(FindText: "{FirstDueDate}", ReplaceWith: LoanClass.FirstDueDate.ToLongDateString(), Replace: Word.WdReplace.wdReplaceAll);

                var schedType = LoanClass.ScheduleTypes.First();
                if (schedType.Type == 0)//Perday
                {
                    range.Find.Execute(FindText: "{ScheduleType}", ReplaceWith: "Daily", Replace: Word.WdReplace.wdReplaceAll);
                }
                else if (schedType.Type == 2)
                {
                    range.Find.Execute(FindText: "{ScheduleType}", ReplaceWith: string.Format("{0}th & {1}th ", schedType.FirstSched, schedType.SecondSched), Replace: Word.WdReplace.wdReplaceAll);
                }
                else
                {
                    range.Find.Execute(FindText: "{ScheduleType}", ReplaceWith: string.Format("{0}th", schedType.FirstSched), Replace: Word.WdReplace.wdReplaceAll);
                }


                range.Find.Execute(FindText: "{MonthlyInterest}", ReplaceWith: LoanClass.LoanApplication.CreditTerm.MonthlyInterest, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{MonthlyInterest.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.LoanApplication.CreditTerm.MonthlyInterest), Replace: Word.WdReplace.wdReplaceAll);

                application.ActiveDocument.Close();
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Word is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }

        //public static void LoadLedger(DBModel.Loan LoanClass)
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
        //        Word.Table firstTable = document.Tables.Add(para0.Range, 8, 6, ref missing, ref missing);
        //        firstTable.AllowAutoFit = true;

        //        firstTable.Range.Font.Name = "Calibri Light";
        //        firstTable.Range.Font.Size = float.Parse("9");
        //        firstTable.Range.Paragraphs.SpaceAfter = 0;

        //        //Row 1
        //        firstTable.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[1].Cells[1].Range.Text = "Account No.:";
        //        if (LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.Count() > 0)
        //        {
        //            firstTable.Rows[1].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.First().AccountNumber;
        //        }

        //        firstTable.Rows[1].Cells[4].Range.Text = "Schedule:";
        //        firstTable.Rows[1].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

        //        if (LoanClass.LoanApplication.PersonalData.DigitalInfos.Count() > 0)
        //        {
        //            Image profileImage = new Bitmap(CameraHelper.ByteArrayToImage(LoanClass.LoanApplication.PersonalData.DigitalInfos.FirstOrDefault().Photo), 
        //                new System.Drawing.Size(110, 110));

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
        //        firstTable.Rows[2].Cells[1].Range.Text = "Voucher Number:";
        //        firstTable.Rows[2].Cells[2].Range.Text = LoanClass.AccountCode;
        //        firstTable.Rows[2].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[2].Cells[4].Range.Text = "Amortization:";
        //        firstTable.Rows[2].Cells[5].Range.Text = string.Format("{0}", MoneyConverter.ConvertDoubleToMoney(LoanClass.Amortization));

        //        //Row 3
        //        firstTable.Rows[3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[3].Cells[1].Range.Text = "Name of Borrower:";
        //        firstTable.Rows[3].Cells[2].Range.Text = string.Format("{0}, {1}", LoanClass.LoanApplication.PersonalData.LastName, LoanClass.LoanApplication.PersonalData.FirstName);
        //        firstTable.Rows[3].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[3].Cells[4].Range.Text = "Terms Of Credit:";
        //        firstTable.Rows[3].Cells[5].Range.Text = LoanClass.LoanApplication.CreditTerm.DisplayName;

        //        //Row 4
        //        firstTable.Rows[4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[4].Cells[1].Range.Text = "Date Release:";
        //        firstTable.Rows[4].Cells[2].Range.Text = LoanClass.ReleaseDate.ToShortDateString();
        //        firstTable.Rows[4].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[4].Cells[4].Range.Text = "Interest Rate:";
        //        firstTable.Rows[4].Cells[5].Range.Text = string.Format("{0}% per Month", LoanClass.InterestRate);

        //        //Row 5
        //        firstTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[5].Cells[1].Range.Text = "First Due Date:";
        //        firstTable.Rows[5].Cells[2].Range.Text = LoanClass.FirstDueDate.ToShortDateString();
        //        firstTable.Rows[5].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[5].Cells[4].Range.Text = "Principal:";
        //        firstTable.Rows[5].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal);

        //        //Row 6
        //        firstTable.Rows[6].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[6].Cells[1].Range.Text = "Maturity:";
        //        firstTable.Rows[6].Cells[2].Range.Text = LoanClass.MaturityDate.ToShortDateString();
        //        firstTable.Rows[6].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[6].Cells[4].Range.Text = "Interest:";
        //        firstTable.Rows[6].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Interest);

        //        //Row 7
        //        firstTable.Rows[7].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[7].Cells[1].Range.Text = "Address:";
        //        firstTable.Rows[7].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.HomeAddress;

        //        //row8
        //        firstTable.Rows[8].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
        //        firstTable.Rows[8].Cells[1].Range.Text = "Contact Number:";
        //        firstTable.Rows[8].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.ContactNumber;
        //        firstTable.Rows[1].Cells[6].Merge(firstTable.Rows[8].Cells[6]);

        //        var para1 = document.Content.Paragraphs.Add(ref missing);
        //        para1.Range.Text = "LEDGER CARD";
        //        para1.Range.Font.Name = "Calibri Light";
        //        para1.Range.Font.Size = float.Parse("10.5");
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

        //        secondTable.Range.Font.Name = "Calibri Light";
        //        secondTable.Range.Font.Size = float.Parse("8");

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
        //                        cell.Range.Font.Name = "Calibri Light";
        //                        cell.Range.Font.Size = float.Parse("10");
        //                    }
        //                    cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //                    cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //                    col_count++;
        //                }
        //            }

        //            row_count++;
        //        }

        //        secondTable.Rows[schedules.Count() + 3].Cells[1].Range.Text = "Total";
        //        secondTable.Rows[schedules.Count() + 3].Cells[1].Range.Font.Size = float.Parse("10.5");
        //        secondTable.Rows[schedules.Count() + 3].Cells[1].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //        secondTable.Rows[schedules.Count() + 3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

        //        secondTable.Rows[schedules.Count() + 3].Cells[3].Range.Font.Size = float.Parse("10.5");
        //        secondTable.Rows[schedules.Count() + 3].Cells[3].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal + LoanClass.Interest);
        //        secondTable.Rows[schedules.Count() + 3].Cells[3].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
        //        secondTable.Rows[schedules.Count() + 3].Cells[3].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

        //        secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Font.Size = float.Parse("10.5");
        //        secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Text = string.Format("{0} Pesos",MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal + LoanClass.Interest));
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

        public static void LoadLedger(DBModel.Loan LoanClass)
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
                document.PageSetup.PageWidth = 576;
                document.PageSetup.PageHeight = 360;

                var para0 = document.Content.Paragraphs.Add(ref missing);
                para0.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                Word.Table firstTable = document.Tables.Add(para0.Range, 7, 6, ref missing, ref missing);
                firstTable.AllowAutoFit = true;

                firstTable.Range.Font.Name = "Calibri";
                firstTable.Range.Font.Size = 9;
                firstTable.Range.Paragraphs.SpaceAfter = 0;

                //Row 1
                firstTable.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[1].Cells[1].Range.Text = "Voucher No.:";
                firstTable.Rows[1].Cells[2].Range.Text = LoanClass.AccountCode;
                firstTable.Rows[1].Cells[4].Range.Text = "Schedule:";
                firstTable.Rows[1].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

                if (LoanClass.LoanApplication.PersonalData.DigitalInfos.Count() > 0)
                {
                    Image profileImage = new Bitmap(CameraHelper.ByteArrayToImage(LoanClass.LoanApplication.PersonalData.DigitalInfos.FirstOrDefault().Photo), new System.Drawing.Size(110, 110));

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
                firstTable.Rows[2].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[2].Cells[1].Range.Text = "Name of Borrower:";
                firstTable.Rows[2].Cells[2].Range.Text = string.Format("{0}, {1}", LoanClass.LoanApplication.PersonalData.LastName, LoanClass.LoanApplication.PersonalData.FirstName);
                firstTable.Rows[2].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[2].Cells[4].Range.Text = "Amortization:";
                firstTable.Rows[2].Cells[5].Range.Text = string.Format("{0}", MoneyConverter.ConvertDoubleToMoney(LoanClass.Amortization));



                //Row 3
                firstTable.Rows[3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[3].Cells[1].Range.Text = "Date Release:";
                firstTable.Rows[3].Cells[2].Range.Text = LoanClass.ReleaseDate.ToShortDateString();
                firstTable.Rows[3].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[3].Cells[4].Range.Text = "Terms Of Credit:";
                firstTable.Rows[3].Cells[5].Range.Text = LoanClass.LoanApplication.CreditTerm.DisplayName;

                //Row 4
                firstTable.Rows[4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[4].Cells[1].Range.Text = "First Due Date:";
                firstTable.Rows[4].Cells[2].Range.Text = LoanClass.FirstDueDate.ToShortDateString();
                firstTable.Rows[4].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[4].Cells[4].Range.Text = "Interest Rate:";
                firstTable.Rows[4].Cells[5].Range.Text = string.Format("{0}% per Month", LoanClass.InterestRate);

                //Row 5
                firstTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[5].Cells[1].Range.Text = "Maturity:";
                firstTable.Rows[5].Cells[2].Range.Text = LoanClass.MaturityDate.ToShortDateString();
                firstTable.Rows[5].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[5].Cells[4].Range.Text = "Principal:";
                firstTable.Rows[5].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal);

                //Row 6
                firstTable.Rows[6].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[6].Cells[1].Range.Text = "Address:";
                firstTable.Rows[6].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.HomeAddress;
                firstTable.Rows[6].Cells[4].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[6].Cells[4].Range.Text = "Interest:";
                firstTable.Rows[6].Cells[5].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Interest);

                //Row 7
                firstTable.Rows[7].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                firstTable.Rows[7].Cells[1].Range.Text = "Contact Number:";
                firstTable.Rows[7].Cells[2].Range.Text = LoanClass.LoanApplication.PersonalData.ContactNumber;
                firstTable.Rows[1].Cells[6].Merge(firstTable.Rows[7].Cells[6]);

                var para1 = document.Content.Paragraphs.Add(ref missing);
                para1.Range.Text = "LEDGER CARD";
                para1.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                para1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.InsertParagraphAfter();

                Word.Table secondTable = document.Tables.Add(para1.Range, LoanClass.PaymentSchedules.Count() + 4, 11, ref missing, ref missing);
                //secondTable.Borders.Enable = 1;
                secondTable.Range.Paragraphs.SpaceAfter = 0;

                var schedules = LoanClass.PaymentSchedules.OrderBy(x => x.Schedule).ToList();

                List<string> headers = new List<string>(new string[]
                {
                "SHEDULE OF PAYMENT",
                "DATE OF PAYMENTS",
                "OR NO.",
                "PRINCIPAL AMOUNT",
                "INTEREST",
                "REBATES",
                "CHARGES",
                "TOTAL AMOUNT PAID",
                "BALANCE"
                });

                secondTable.AllowAutoFit = true;

                int row_count = -1;
                foreach (Word.Row row in secondTable.Rows)
                {
                    if (row_count < schedules.Count())
                    {
                        int col_count = 0;
                        foreach (Word.Cell cell in row.Cells)
                        {
                            if (cell.RowIndex == 1)
                            {
                                if (col_count == 0)
                                {
                                    cell.Merge(row.Cells[3]);
                                }
                                cell.Range.Text = headers[col_count];
                                cell.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray25;
                            }
                            else
                            {
                                if (col_count == 0)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].Schedule.ToShortDateString());
                                }
                                if (col_count == 1)
                                {
                                    cell.Range.Text = string.Format("{0}", (row_count + 1));
                                }
                                if (col_count == 2)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].InstallmentAmount);
                                }
                            }
                            cell.Range.Font.Name = "Calibri";
                            cell.Range.Font.Size = 8;
                            cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            col_count++;
                        }
                    }

                    row_count++;
                }

                secondTable.Rows[schedules.Count() + 3].Cells[1].Range.Text = "Total";
                secondTable.Rows[schedules.Count() + 3].Cells[1].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                secondTable.Rows[schedules.Count() + 3].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                secondTable.Rows[schedules.Count() + 3].Cells[3].Range.Text = MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal + LoanClass.Interest);
                secondTable.Rows[schedules.Count() + 3].Cells[3].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                secondTable.Rows[schedules.Count() + 3].Cells[3].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Text = string.Format("{0} Pesos", MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal + LoanClass.Interest));
                secondTable.Rows[schedules.Count() + 4].Cells.Merge();
                secondTable.Rows[schedules.Count() + 4].Cells[1].VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                secondTable.Rows[schedules.Count() + 4].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                secondTable.Rows[schedules.Count() + 4].Cells[1].Range.Bold = 0;
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

        public static void LoadAmortizationSchedule(DBModel.Loan LoanClass)
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
                document.PageSetup.LeftMargin = Convert.ToSingle(20);
                document.PageSetup.RightMargin = Convert.ToSingle(20);
                document.PageSetup.TopMargin = Convert.ToSingle(20);
                document.PageSetup.BottomMargin = Convert.ToSingle(20);

                var para0 = document.Content.Paragraphs.Add(ref missing);
                para0.Range.Text = "GENSAN G&F LENDING, INC.";
                para0.Range.InsertParagraphAfter();

                var para1 = document.Content.Paragraphs.Add(ref missing);
                para1.Range.Text = "AMORTIZATION SCHEDULE";
                para1.Range.InsertParagraphAfter();
                para1.Range.Bold = 1;

                Word.Table secondTable = document.Tables.Add(para1.Range, LoanClass.PaymentSchedules.Count()+2, 6, ref missing, ref missing);
                secondTable.Borders.Enable = 1;
                secondTable.AllowAutoFit = true;

                var schedules = LoanClass.PaymentSchedules.OrderBy(x => x.Schedule).ToList();

                List<string> headers = new List<string>(new string[]
                {
                "Installment",
                "Loan",
                "Principal",
                "Interest",
                "Total",
                "O/S Balance"
                });
                

                int row_count = -1;
                double amountpaid = 0;
                var amounttotal = (LoanClass.Principal + LoanClass.Interest);
                foreach (Word.Row row in secondTable.Rows)
                {
                    if (row_count < schedules.Count())
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
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].Schedule.ToShortDateString());
                                }
                                if (col_count == 1)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].InstallmentAmount);
                                }
                                if (col_count == 2)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].Principal);
                                }
                                if (col_count == 3)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].Interest);
                                }
                                if (col_count == 4)
                                {
                                    cell.Range.Text = string.Format("{0}", schedules[row_count].InstallmentAmount);
                                }
                                if (col_count == 5)
                                {
                                    amountpaid += schedules[row_count].InstallmentAmount;
                                    cell.Range.Text = string.Format("{0}", (amounttotal- amountpaid));
                                }
                                
                            }
                            cell.Range.Font.Name = "Calibri";
                            cell.Range.Font.Size = 8;
                            cell.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                            col_count++;
                        }
                    }

                    row_count++;
                }

                secondTable.Rows[schedules.Count() + 2].Cells[1].Range.Text = "Total";
                secondTable.Rows[schedules.Count() + 2].Cells[2].Range.Text = string.Format("{0}", (amounttotal));
                secondTable.Rows[schedules.Count() + 2].Cells[3].Range.Text = string.Format("{0}", (LoanClass.Principal));
                secondTable.Rows[schedules.Count() + 2].Cells[4].Range.Text = string.Format("{0}", (LoanClass.Interest));
                secondTable.Rows[schedules.Count() + 2].Cells[5].Range.Text = string.Format("{0}", (amounttotal));

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

        public static void LoadCashVoucher(DBModel.Loan LoanClass, string document)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.docx", temppath, filename);

            System.IO.File.Copy(string.Format(@"{0}\Documents\{1}", dir, document), currentPath);

            var application = new Word.Application();
            if (application != null)
            {
                application.Documents.Open(currentPath);

                Word.Range range = application.ActiveDocument.Content;
                range.Find.ClearFormatting();
                if(LoanClass.LoanApplication.PersonalData.PersonalDataAccounts != null && LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.Count() > 0)
                {
                    range.Find.Execute(FindText: "{AccountNumber}", ReplaceWith: LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.FirstOrDefault().AccountNumber, Replace: Word.WdReplace.wdReplaceAll);
                }
                
                range.Find.Execute(FindText: "{DisplayName}", ReplaceWith: LoanClass.LoanApplication.PersonalData.DisplayName, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm}", ReplaceWith: LoanClass.LoanTerm, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.LoanTerm), Replace: Word.WdReplace.wdReplaceAll);

                

                range.Find.Execute(FindText: "{AccountCode}", ReplaceWith: LoanClass.AccountCode, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{ReleaseDate}", ReplaceWith: LoanClass.ReleaseDate.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);

                var prinLessMisc = (LoanClass.Principal - LoanClass.Miscellaneous);
                range.Find.Execute(FindText: "{Principal-Misc}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(prinLessMisc), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal-Misc.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(prinLessMisc), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Misc}", ReplaceWith: LoanClass.Miscellaneous, Replace: Word.WdReplace.wdReplaceAll);

                range.Find.Execute(FindText: "{MonthlyInterest}", ReplaceWith: LoanClass.LoanApplication.CreditTerm.MonthlyInterest, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{MonthlyInterest.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.LoanApplication.CreditTerm.MonthlyInterest), Replace: Word.WdReplace.wdReplaceAll);

                application.ActiveDocument.Close();
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Word is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public static void LoadPassbook(DBModel.Loan LoanClass, string document)
        {

            var dir = System.IO.Directory.GetCurrentDirectory();

            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.docx", temppath, filename);

            System.IO.File.Copy(string.Format(@"{0}\Documents\{1}", dir, document), currentPath);

            var application = new Word.Application();
            if (application != null)
            {
                application.Documents.Open(currentPath);

                Word.Range range = application.ActiveDocument.Content;
                range.Find.ClearFormatting();
                range.Find.Execute(FindText: "{DisplayName}", ReplaceWith: LoanClass.LoanApplication.PersonalData.DisplayName, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{HomeAddress}", ReplaceWith: LoanClass.LoanApplication.PersonalData.HomeAddress, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{ContactNumber}", ReplaceWith: LoanClass.LoanApplication.PersonalData.ContactNumber, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{InterestRate}", ReplaceWith: LoanClass.InterestRate, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Interest}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Interest), Replace: Word.WdReplace.wdReplaceAll);

                var PaymentCharge = PaymentChargeManager.Get();
                var latePaymentCharge = (LoanClass.Principal * (PaymentCharge.Percentage / 100));
                range.Find.Execute(FindText: "{LatePaymentFee}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(latePaymentCharge), Replace: Word.WdReplace.wdReplaceAll);

                var schedType = LoanClass.ScheduleTypes.First();
                string paymode = string.Empty;
                if (schedType.Type == 0)//Perday
                {
                    paymode = "DAILY";
                }
                else if (schedType.Type == 2)
                {
                    paymode = "SEMI MONTHLY";
                }
                else
                {
                    paymode = "MONTHLY";
                }
                range.Find.Execute(FindText: "{PaymentMode}", ReplaceWith: paymode, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal+Interest}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal+LoanClass.Interest), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm}", ReplaceWith: LoanClass.LoanTerm, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LoanTerm.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.LoanTerm), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{AccountCode}", ReplaceWith: LoanClass.AccountCode, Replace: Word.WdReplace.wdReplaceAll);
                //if (LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.Count() > 0)
                //{
                //    range.Find.Execute(FindText: "{AccountCode}", ReplaceWith: LoanClass.LoanApplication.PersonalData.PersonalDataAccounts.First().AccountNumber, Replace: Word.WdReplace.wdReplaceAll);
                //}
                
                range.Find.Execute(FindText: "{ReleaseDate}", ReplaceWith: LoanClass.ReleaseDate.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{MaturityDate}", ReplaceWith: LoanClass.MaturityDate.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{FirstDueDate}", ReplaceWith: LoanClass.FirstDueDate.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);

                application.ActiveDocument.Close();
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Word is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public static void LoadDisclosure(DBModel.Loan LoanClass, string document)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.docx", temppath, filename);

            System.IO.File.Copy(string.Format(@"{0}\Documents\{1}", dir, document), currentPath);

            var application = new Word.Application();
            if (application != null)
            {
                //LatePaymentCharge
                var latePaymentCharge = PaymentChargeManager.Get();
                application.Documents.Open(currentPath);

                Word.Range range = application.ActiveDocument.Content;
                range.Find.ClearFormatting();
                range.Find.Execute(FindText: "{DisplayName}", ReplaceWith: LoanClass.LoanApplication.PersonalData.DisplayName, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{HomeAddress}", ReplaceWith: LoanClass.LoanApplication.PersonalData.HomeAddress, Replace: Word.WdReplace.wdReplaceAll);
                
                range.Find.Execute(FindText: "{Principal}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Miscellaneous}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Miscellaneous), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{MiscInterest}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Miscellaneous-150), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal-Misc}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal - LoanClass.Miscellaneous), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{ReleaseDate}", ReplaceWith: LoanClass.ReleaseDate.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LatePaymentCharge}", ReplaceWith: latePaymentCharge.Percentage, Replace: Word.WdReplace.wdReplaceAll);
                var creditor = string.Empty;
                var creditors = LoanClass.LoanApplication.PersonalData.CreditReferences.ToList();
                if(creditors.Count() > 0)
                {
                    creditor = creditors.FirstOrDefault().Creditor;
                }
                range.Find.Execute(FindText: "{Creditor}", ReplaceWith: creditor, Replace: Word.WdReplace.wdReplaceAll);
                application.ActiveDocument.Close();
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Word is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public static void LoadReminder(DBModel.Loan LoanClass, string document)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();

            var temppath = System.IO.Path.GetTempPath();
            var filename = System.IO.Path.GetRandomFileName();
            var currentPath = string.Format("{0}{1}.doc", temppath, filename);

            System.IO.File.Copy(string.Format(@"{0}\Documents\{1}", dir, document), currentPath);

            var application = new Word.Application();
            if (application != null)
            {
                application.Documents.Open(currentPath);

                Word.Range range = application.ActiveDocument.Content;
                range.Find.ClearFormatting();

                range.Find.Execute(FindText: "{DateTime.Now}", ReplaceWith: DateTime.Now.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{LastName}", ReplaceWith: LoanClass.LoanApplication.PersonalData.LastName, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{HomeAddress}", ReplaceWith: LoanClass.LoanApplication.PersonalData.HomeAddress, Replace: Word.WdReplace.wdReplaceAll);

                var addressing = LoanClass.LoanApplication.PersonalData.Gender.Equals("Male") ? "Mr" : "Ms";
                range.Find.Execute(FindText: "{Addressing}", ReplaceWith: addressing, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{DisplayName.Capital}", ReplaceWith: LoanClass.LoanApplication.PersonalData.DisplayName.ToUpper(), Replace: Word.WdReplace.wdReplaceAll);

                range.Find.Execute(FindText: "{Principal}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Principal.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.Principal), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{ReleaseDate}", ReplaceWith: LoanClass.ReleaseDate.ToLongDateString(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{InterestRate}", ReplaceWith: LoanClass.InterestRate, Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{InterestRate.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.InterestRate), Replace: Word.WdReplace.wdReplaceAll);

                range.Find.Execute(FindText: "{ScheduleCount}", ReplaceWith: LoanClass.PaymentSchedules.Count(), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{ScheduleCount.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(LoanClass.PaymentSchedules.Count()), Replace: Word.WdReplace.wdReplaceAll);
                //Principal+Interest-Payments

                //Fix
                double index = LoanClass.Payments.Sum(p => p.Amount) / LoanClass.Amortization;
                var current_index = (int)index;
                var min_sched = LoanClass.PaymentSchedules.OrderBy(ps => ps.Schedule).ToList()[current_index];

                var lessAmount = 0.0;
                if ((index % 1) != 0)
                {
                    var ExactAmount = (current_index) * LoanClass.Amortization;
                    lessAmount = ExactAmount - LoanClass.Payments.Sum(p => p.Amount);
                }
                var balance = 0.0;
                var duedates = LoanClass.PaymentSchedules.Where(psched => psched.Schedule >= min_sched.Schedule && psched.Schedule <= DateTime.Now);
                if (duedates.Count() > 0)
                {
                    balance = (LoanClass.Amortization * duedates.Count()) + lessAmount;
                }

                //var balance = (LoanClass.Principal + LoanClass.Interest) - LoanClass.Payments.Sum(x => x.Amount);
                range.Find.Execute(FindText: "{Balance}", ReplaceWith: MoneyConverter.ConvertDoubleToMoney(balance), Replace: Word.WdReplace.wdReplaceAll);
                range.Find.Execute(FindText: "{Balance.Word}", ReplaceWith: MoneyConverter.ConvertDoubleToMoneyWord(balance), Replace: Word.WdReplace.wdReplaceAll);
                application.ActiveDocument.Close();
                System.Diagnostics.Process.Start(currentPath);
            }
            else
            {
                MessageBox.Show("Word is not propertly installed!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        //private object GetPropValue(object source, string propertyName)
        //{
        //    return source.GetType().GetProperty(propertyName).GetValue(source, null);
        //}


    }
}
