using View = Alkambia.WPF.LoanMonitoring.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Alkambia.App.LoanMonitoring.Model;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows;
using Alkambia.App.LoanMonitoring.Helper;
using Alkambia.App.LoanMonitoring.Enums;
using System.Windows.Controls;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class ReleaseController
    {
        View.Releasing.Releasing ReleasingForm { get; set; }
        View.Loan.LoanMain LoanMainForm { get; set; }
        List<Model.Approval> Approvals { get; set; }
        Model.Status StatusApproved { get; set; }
        Model.Status StatusReleased { get; set; }
        Model.Status StatusNew { get; set; }
        Model.Status StatusCurrent { get; set; }
        Model.LoanPercentage LoanPercentage { get; set; }
        Model.Loan LoanEntry { get; set; }
        Model.ScheduleType ScheduleType { get; set; }
        List<Model.PaymentSchedule> PaymentSchedules { get; set; }
        Model.Miscellaneous miscClass { get; set; }
        public ReleaseController(View.Releasing.Releasing form)
        {
            ReleasingForm = form;
            DataInit();
            Init();
            events();
        }

        private void DataInit()
        {
            StatusNew = StatusManager.GetName("Status.New");
            StatusCurrent = StatusManager.GetName("Loan.Current");
            StatusReleased = StatusManager.GetName("Status.Released");
            StatusApproved = StatusManager.GetName("Status.Approved");
            Approvals = ApprovalManager.GetUnReleasedApprovals().ToList();
            LoanPercentage = LoanPercentageManager.Get();
            miscClass = MiscellaneousManager.Get();
            PaymentSchedules = new List<Model.PaymentSchedule>();
        }
        private void Init()
        {
            ReleasingForm.RealesingDG.ItemsSource = Approvals;
        }
        private void events()
        {
            //ReleasingForm.print_buton.Click += Print_buton_Click;
            ReleasingForm.release_buton.Click += Release_buton_Click;
            ReleasingForm.reapprove_button.Click += Reapprove_button_Click;
            ReleasingForm.profile_button.Click += Profile_button_Click;
        }


        private void Profile_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ReleasingForm.RealesingDG.SelectedItem != null)
            {
                var person = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval).LoanApplication.PersonalData;
                ReleasingForm.Hide();
                var dummy = new Views.LoanApplication.LoanApplicationMain(person, CrudEnums.Read);
                dummy.ShowDialog();
                ReleasingForm.Show();
            }
        }

        private void Reapprove_button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ReleasingForm.RealesingDG.SelectedItem != null)
            {
                var person = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval).LoanApplication.PersonalData;

                if (MessageBox.Show("Are you sure you want to re-approve the loan?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var approval = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval);
                    //Re set loan application status
                    var applicationLoan = approval.LoanApplication;
                    applicationLoan.StatusID = StatusNew.StatusID;
                    LoanApplicationManager.SaveorUpdate(applicationLoan);

                    ApprovalManager.Delete(approval.ApprovalID);
                    Approvals.Remove(approval);
                    ReleasingForm.RealesingDG.Items.Refresh();
                    MessageBox.Show("Loan Application is send to re-approve", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Release_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ReleasingForm.RealesingDG.SelectedItem != null)
            {
                var person = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval).LoanApplication.PersonalData;
                var approval = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval);

                //Additional logic:
                //Create Loan Entry
                
                int term = approval.LoanApplication.CreditTerm.Term;
                double principal = approval.LoanApplication.LoanAmount;
                double interest = (approval.LoanApplication.CreditTerm.TotalInterest * (principal / 100));
                //(principal * (LoanPercentage.Percentage / 100));

                var accCode = AccountCode();
                var miscComputation = ((miscClass.Percentage * (principal / 100)) + miscClass.AdditionalCharge);

                LoanEntry = new Model.Loan() {
                    LoanID = Guid.NewGuid(),
                    AccountCode = accCode,
                    ReleaseDate = DateTime.Now,
                    //FirstDueDate = DateTime.Now,
                    //MaturityDate = DateTime.Now,
                    LoanTerm = term,
                    //Amortization = ((principal + interest)/ term),
                    //InterestRate = (LoanPercentage.Percentage / term),
                    InterestRate = approval.LoanApplication.CreditTerm.MonthlyInterest,
                    Principal = principal,
                    Interest = interest,
                    LoanApplicationID = approval.LoanApplicationID,
                    LoanApplication = approval.LoanApplication,
                    StatusID = StatusCurrent.StatusID,
                    ScheduleTypes = new List<Model.ScheduleType>(),
                    Releases = new List<Model.Release>(),
                    Miscellaneous = miscComputation,
                    CashProceeds = (principal - miscComputation)
                };

                ScheduleType = new Model.ScheduleType() { ScheduleTypeID = Guid.NewGuid(), LoanID = LoanEntry.LoanID, Type = 2 };

                LoanMainForm = new View.Loan.LoanMain();
                LoanMainForm.DataContext = LoanEntry;
                LoanMainForm.dailyRB.Checked += DailyRB_Checked;
                LoanMainForm.semiMonthlyRB.Checked += SemiMonthlyRB_Checked;
                LoanMainForm.monthlyRB.Checked += MonthlyRB_Checked;
                LoanMainForm.firstdueDP.SelectedDateChanged += FirstdueDP_SelectedDateChanged;
                LoanMainForm.CancelBtn.Click += LoanMainCancelBtn_Click;
                LoanMainForm.SaveBTn.Click += LoanMainSaveBTn_Click;

                LoanMainForm.firstSchedDP.SelectedDateChanged += SchedDP1_SelectedDateChanged;
                LoanMainForm.secondSchedDP.SelectedDateChanged += SchedDP2_SelectedDateChanged;

                LoanMainForm.ShowDialog();
            }
        }
        private string AccountCode()
        {
            var newcode = LoanManager.GetMaxCode() + 1;
            var accCode = AccountCodeGenerator.GenerateCode(10, newcode);
            //if (LoanManager.Exist(accCode))
            //{
            //    accCode = AccountCode();
            //}
            return accCode;
        }

        private void SchedDP1_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                LoanMainForm.secondSchedDP.SelectedDate = LoanMainForm.firstSchedDP.SelectedDate.Value.AddDays(15);
                ScheduleType.FirstSched = LoanMainForm.firstSchedDP.SelectedDate.Value.Day;
                ScheduleType.SecondSched = LoanMainForm.secondSchedDP.SelectedDate.Value.Day;
                
                if (DateTime.Now.Day < LoanMainForm.firstSchedDP.SelectedDate.Value.Day)
                {
                    //LoanMainForm.firstdueDP.SelectedDate = LoanMainForm.firstSchedDP.SelectedDate.Value;
                    LoanEntry.FirstDueDate = LoanMainForm.firstSchedDP.SelectedDate.Value;
                }
                else if(DateTime.Now.Day > LoanMainForm.firstSchedDP.SelectedDate.Value.Day && DateTime.Now.Day < LoanMainForm.secondSchedDP.SelectedDate.Value.Day)
                {
                    //LoanMainForm.firstdueDP.SelectedDate = LoanMainForm.secondSchedDP.SelectedDate.Value;
                    LoanEntry.FirstDueDate = LoanMainForm.secondSchedDP.SelectedDate.Value;;
                }
                else
                {
                    //LoanMainForm.firstdueDP.SelectedDate = LoanMainForm.firstSchedDP.SelectedDate.Value.AddMonths(1);
                    LoanEntry.FirstDueDate = LoanMainForm.firstSchedDP.SelectedDate.Value.AddMonths(1);
                }
                LoanMainForm.firstdueDP.Text = LoanEntry.FirstDueDate.ToLongDateString();
                //LoanMainForm.firstdueDP.DisplayDate = LoanEntry.FirstDueDate.Date;
                //SetFirstDueDateRequirements();
                //var date1 = LoanEntry.FirstDueDate.AddMonths(-1);
                //var date2 = LoanEntry.FirstDueDate.AddMonths(1);
                //var datecount = date2.Subtract(date1).Days;
                //for (double y = 0; y < datecount; y++)
                //{
                //    var dummydate = date1.AddDays(y);
                //    if (dummydate.Day != ScheduleType.FirstSched && dummydate.Day != ScheduleType.SecondSched)
                //    {
                //        CalendarDateRange range = new CalendarDateRange(dummydate);
                //        LoanMainForm.firstdueDP.BlackoutDates.Add(range);
                //    }
                //}


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }
        }

        private void SchedDP2_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (LoanMainForm.secondSchedDP.SelectedDate.Value.Day < LoanMainForm.firstSchedDP.SelectedDate.Value.Day)
                {
                    MessageBox.Show("Please set the first day of schedule of payment lower that the second day of schedule of payment.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoanMainSaveBTn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoanEntry.ScheduleTypes.Add(ScheduleType);

                //Create PaymentSchedule Entry
                //var schedules = ScheduleCreator.GenerateSchedules(LoanEntry, ScheduleType);
                //PaymentSchedules = ScheduleCreator.GenerateSchedules(LoanEntry, ScheduleType);
                LoanEntry.PaymentSchedules = PaymentSchedules;//schedules;


                //Release
                var release = new Model.Release()
                {
                    ReleaseID = Guid.NewGuid(),
                    LoanApplicationID = LoanEntry.LoanApplicationID,
                    LoanID = LoanEntry.LoanID,
                    DateReleased = LoanEntry.ReleaseDate
                };

                LoanEntry.MaturityDate = LoanEntry.PaymentSchedules.Max(x => x.Schedule);//LoanEntry.FirstDueDate.AddMonths(LoanEntry.LoanTerm);

            LoanEntry.LoanApplication = null;
                LoanEntry.Releases.Add(release);
                LoanManager.Add(LoanEntry);

                var approval = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval);
                approval.StatusID = StatusReleased.StatusID;
                ApprovalManager.SaveorUpdate(approval);
                Approvals.Remove(approval);
                ReleasingForm.RealesingDG.Items.Refresh();
                MessageBox.Show("Loan Application is set to release", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                LoanMainForm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void LoanMainCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            LoanMainForm.Close();
        }

        private void FirstdueDP_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SetFirstDueDateRequirements();
        }

        private void SetFirstDueDateRequirements()
        {
            SetAmoritization();
            if (PaymentSchedules.Count() > 0)
            {
                LoanMainForm.MaturityTB.Text = PaymentSchedules.Max(x => x.Schedule).ToShortDateString();
                LoanEntry.MaturityDate = PaymentSchedules.Max(x => x.Schedule);
            }
        }

        private void SetAmoritization()
        {
            var approval = (ReleasingForm.RealesingDG.SelectedItem as Model.Approval);
            PaymentSchedules = ScheduleCreator.GenerateSchedules(LoanEntry, ScheduleType);
            var total = (LoanEntry.Principal + LoanEntry.Interest);
            var amortization = Math.Round(total/ PaymentSchedules.Count(),12);
            LoanEntry.Amortization = amortization;
            LoanMainForm.AmortizationTB.Text = amortization.ToString();

            LoanMainForm.ScheduleDG.ItemsSource = PaymentSchedules;
        }

        private void MonthlyRB_Checked(object sender, RoutedEventArgs e)
        {
            LoanMainForm.firstSchedDP.IsEnabled = true;
            LoanMainForm.secondSchedDP.IsEnabled = false;

            ScheduleType.Type = 1;
            if (LoanMainForm.firstSchedDP.SelectedDate.HasValue)
            {
                ScheduleType.FirstSched = LoanMainForm.firstSchedDP.SelectedDate.Value.Day;
            }
            if (LoanMainForm.secondSchedDP.SelectedDate.HasValue)
            {
                ScheduleType.SecondSched = LoanMainForm.secondSchedDP.SelectedDate.Value.Day;
            }
            SetAmoritization();

        }

        private void SemiMonthlyRB_Checked(object sender, RoutedEventArgs e)
        {
            LoanMainForm.firstSchedDP.IsEnabled = true;
            LoanMainForm.secondSchedDP.IsEnabled = true;

            ScheduleType.Type = 2;
            if(LoanMainForm.firstSchedDP.SelectedDate.HasValue)
            {
                ScheduleType.FirstSched = LoanMainForm.firstSchedDP.SelectedDate.Value.Day;
            }
            ScheduleType.SecondSched = 0;
            SetAmoritization();
        }

        private void DailyRB_Checked(object sender, RoutedEventArgs e)
        {
            LoanMainForm.firstSchedDP.IsEnabled = false;
            LoanMainForm.secondSchedDP.IsEnabled = false;

            ScheduleType.Type = 0;
            ScheduleType.FirstSched = 0;
            ScheduleType.SecondSched = 0;
            SetAmoritization();
        }

        private void Print_buton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //AccountCode
            //Open documents for the applicant and print
        }


    }
}
