using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.WPF.LoanMonitoring.Views.LoanApplication;
using Model = Alkambia.App.LoanMonitoring.Model;
using System.Windows;
using Alkambia.App.LoanMonitoring.BusinessTransactions;
using System.Windows.Controls;
using Alkambia.App.LoanMonitoring.Enums;
using Alkambia.WPF.LoanMonitoring.Views.Camera;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Alkambia.App.LoanMonitoring.Helper;

namespace Alkambia.WPF.LoanMonitoring.Controller
{
    public class LoanApplicationController
    {
        LoanApplicationMain Main { get; set; }
        EmployerForm employerForm { get; set; }
        SourceOfIncomeForm sourceOfIncomeForm { get; set; }
        CreditorForm creditorForm { get; set; }
        PropertyForm propertyForm { get; set; }
        PersonalRefferenceForm personalRefferenceForm { get; set; }
        Model.PersonalData person { get; set; }
        Model.LoanApplication loanApplication { get; set; }

        Model.Status StatusNew { get; set; }

        List<Model.Status> employerStatus { get; set; }
        List<Model.Status> creditRefStatus { get; set; }
        List<Model.Relation> relations { get; set; }
        List<Model.Kind> kinds { get; set; }
        List<Model.CreditTerm> terms { get; set; }
        string currentAccountNumber { get; set; }

        CrudEnums Crud { get; set; }

        WebCamController WebCam { get; set; }
        CameraCapture Capture { get; set; }

        public LoanApplicationController(LoanApplicationMain main, Model.PersonalData person, CrudEnums crud)
        {
            this.Crud = crud;
            this.Main = main;
            this.person = person;
            DataInit();
            events();
            Init();
        }
        private void DataInit()
        {
            StatusNew = StatusManager.GetName("Status.New");
            employerStatus = StatusManager.GetByEntityName("Employer").ToList();
            creditRefStatus = StatusManager.GetByEntityName("CreditReference").ToList();
            relations = RelationManager.Get().ToList();
            kinds = KindManager.Get().ToList();
            terms = CreditTermManager.Get().ToList();
        }

        private void events()
        {
            Main.submit_button.Click += submit_button_Click;


            if(!Crud.Equals(CrudEnums.Read))
            {
                //PersonalData
                Main.PersonalData0.DateOfBirthDP.SelectedDateChanged += DateOfBirthDP_SelectedDateChanged;
                Main.PersonalData0.ChangePhoto_btn.Click += ChangePhoto_btn_Click;
                Main.PersonalData0.generate_account_number_btn.Click += gen_account_num_btn_Click;

                //Employers
                Main.PersonalData1.AddEmployerBtn.Click += AddEmployerBtn_Click;
                Main.PersonalData1.EditEmployerBtn.Click += EditEmployerBtn_Click;
                Main.PersonalData1.DeleteEmployerBtn.Click += DeleteEmployerBtn_Click;

                //IncomeSource
                Main.PersonalData2.IncomeSoureAddBtn.Click += IncomeSoureAddBtn_Click;
                Main.PersonalData2.IncomeSoureDeleteBtn.Click += IncomeSoureDeleteBtn_Click;
                Main.PersonalData2.IncomeSoureEditBtn.Click += IncomeSoureEditBtn_Click;

                //CreditReference
                Main.PersonalData3.CreditRefAddBtn.Click += CreditRefAddBtn_Click;
                Main.PersonalData3.CreditRefEditBtn.Click += CreditRefEditBtn_Click;
                Main.PersonalData3.CreditRefDelBtn.Click += CreditRefDelBtn_Click;

                //PersonalRefferences
                Main.PersonalData4.PersonalRefAddBtn.Click += PersonalRefAddBtn_Click;
                Main.PersonalData4.PersonalRefEditBtn.Click += PersonalRefEditBtn_Click;
                Main.PersonalData4.PersonalRefDelBtn.Click += PersonalRefDelBtn_Click;

                //Properties
                Main.PersonalData5.PropertyAddBtn.Click += PropertyAddBtn_Click;
                Main.PersonalData5.PropertyEditBtn.Click += PropertyEditBtn_Click;
                Main.PersonalData5.PropertyDelBtn.Click += PropertyDelBtn_Click;
            }
            
        }

        private void gen_account_num_btn_Click(object sender, RoutedEventArgs e)
        {
            var generated_Acc_Number = GenerateAccountNumber();
            person.PersonalDataAccounts.FirstOrDefault().AccountNumber = generated_Acc_Number;
            Main.PersonalData0.accountNumber.Text = generated_Acc_Number;
        }

        private string GenerateAccountNumber()
        {
            var newcode = PersonalDataAccountManager.GetMaxCode() + 1;
            var accNumber = AccountCodeGenerator.GenerateCode(6, newcode);
            return accNumber;
        }

        private void ChangePhoto_btn_Click(object sender, RoutedEventArgs e)
        {
            Capture = new CameraCapture();
            WebCam = new WebCamController();
            WebCam.InitializeWebCam(ref Capture.CameraImage);
            WebCam.Start();

            Capture.capture_btn.Click += Capture_btn_Click;
            Capture.upload_btn.Click += Upload_btn_Click;
            Capture.video_format_btn.Click += Video_format_btn_Click;
            Capture.video_src_btn.Click += Video_src_btn_Click;
            Capture.close_btn.Click += Close_btn_Click;
            Capture.ShowDialog();
            if (Capture.CameraCaptureImg.Source != null)
            {
                Main.PersonalData0.PersonalDataPhoto.Source = Capture.CameraCaptureImg.Source;
            }
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Capture.Close();
            WebCam.Stop();
        }

        private void Video_src_btn_Click(object sender, RoutedEventArgs e)
        {
            WebCam.AdvanceSetting();
        }

        private void Video_format_btn_Click(object sender, RoutedEventArgs e)
        {
            WebCam.ResolutionSetting();
        }

        private void Upload_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png|"+
              "Bitmap (*.bmp)|*.bmp";
            if (op.ShowDialog() == true)
            {
                Capture.CameraCaptureImg.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void Capture_btn_Click(object sender, RoutedEventArgs e)
        {
            Capture.CameraCaptureImg.Source = Capture.CameraImage.Source;
        }

        private void DateOfBirthDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Main.PersonalData0.AgeTB.Text = string.Format("{0}", new DateTime(DateTime.Now.Subtract(Main.PersonalData0.DateOfBirthDP.SelectedDate.Value).Ticks).Year - 1);
        }

        private void PropertyDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData5.PropertyDG.SelectedIndex > -1)
            {
                var item = Main.PersonalData5.PropertyDG.SelectedItem as Model.Property;
                if (MessageBox.Show("Are you sure you want to remove this object?", "Removing Income Source object", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    person.Properties.Remove(item);
                    Main.PersonalData5.PropertyDG.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void PropertyEditBtn_Click(object sender, RoutedEventArgs e)
        {
            //propertyForm
            if (Main.PersonalData5.PropertyDG.SelectedIndex > -1)
            {
                
                var entity = Main.PersonalData5.PropertyDG.SelectedItem as Model.Property;
                var localKind = kinds.Single(x => x.KindID == entity.KindID);
                if (entity.Kind != localKind)
                {
                    entity.Kind = localKind;
                }
                PropertyCommon();
                propertyForm.AddBtn.Content = "Edit";
                propertyForm.DataContext = entity;
                propertyForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void PropertyAddBtn_Click(object sender, RoutedEventArgs e)
        {
            PropertyCommon();
            propertyForm.DataContext = new Model.Property { PersonalDataID = person.PersonalDataID, PropertyID = Guid.NewGuid() };
            propertyForm.ShowDialog();
        }

        private void PropertyFormCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            propertyForm.Close();
        }

        private void PropertyFormAddBTn_Click(object sender, RoutedEventArgs e)
        {
            var item = propertyForm.DataContext as Model.Property;
            item.KindID = (propertyForm.KindCB.SelectedItem as Model.Kind).KindID;
            if (!(sender as Button).Content.Equals("Edit"))
            {
                person.Properties.Add(item);
                Main.PersonalData5.PropertyDG.Items.Refresh();
            }
            propertyForm.Close();
        }

        private void PropertyCommon()
        {
            propertyForm = new PropertyForm();
            propertyForm.KindCB.ItemsSource = kinds;
            propertyForm.AddBtn.Click += PropertyFormAddBTn_Click;
            propertyForm.CancelBtn.Click += PropertyFormCancelBtn_Click;
        }


        //PersonalRefferences
        private void PersonalRefDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData4.PersonalRefDG.SelectedIndex > -1)
            {
                var item = Main.PersonalData4.PersonalRefDG.SelectedItem as Model.PersonalReference;
                if (MessageBox.Show("Are you sure you want to remove this object?", "Removing Income Source object", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    person.PersonalReferences.Remove(item);
                    Main.PersonalData4.PersonalRefDG.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void PersonalRefEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData4.PersonalRefDG.SelectedIndex > -1)
            {
                var entity = Main.PersonalData4.PersonalRefDG.SelectedItem as Model.PersonalReference;
                var localRelation = relations.Single(x => x.RelationID == entity.RelationID);
                if (entity.Relation != localRelation)
                {
                    entity.Relation = localRelation;
                }
                PersonalRefCommon();
                personalRefferenceForm.AddBtn.Content = "Edit";
                personalRefferenceForm.DataContext = entity;
                personalRefferenceForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void PersonalRefAddBtn_Click(object sender, RoutedEventArgs e)
        {
            PersonalRefCommon();
            personalRefferenceForm.DataContext = new Model.PersonalReference { PersonalDataID = person.PersonalDataID, PersonalReferenceID = Guid.NewGuid() };
            personalRefferenceForm.ShowDialog();
        }

        private void PersonalRefferenceFormCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            personalRefferenceForm.Close();
        }

        private void PersonalRefferenceFormAddBTn_Click(object sender, RoutedEventArgs e)
        {
            var item = personalRefferenceForm.DataContext as Model.PersonalReference;
            item.RelationID = (personalRefferenceForm.RelationCB.SelectedItem as Model.Relation).RelationID;
            //item.Relation = (personalRefferenceForm.RelationCB.SelectedItem as Model.Relation);
            if (!(sender as Button).Content.Equals("Edit"))
            {
                person.PersonalReferences.Add(item);
                Main.PersonalData4.PersonalRefDG.Items.Refresh();
            }
            personalRefferenceForm.Close();
        }

        private void PersonalRefCommon()
        {
            personalRefferenceForm = new PersonalRefferenceForm();
            personalRefferenceForm.RelationCB.ItemsSource = relations;
            personalRefferenceForm.AddBtn.Click += PersonalRefferenceFormAddBTn_Click;
            personalRefferenceForm.CancelBtn.Click += PersonalRefferenceFormCancelBtn_Click;
        }

        //Credit Reffences
        private void CreditRefDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData3.CreditRefDG.SelectedIndex > -1)
            {
                var creditref = Main.PersonalData3.CreditRefDG.SelectedItem as Model.CreditReference;
                if (MessageBox.Show("Are you sure you want to remove this object?", "Removing Income Source object", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    person.CreditReferences.Remove(creditref);
                    Main.PersonalData3.CreditRefDG.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void CreditRefEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData3.CreditRefDG.SelectedIndex > -1)
            {
                var entity = Main.PersonalData3.CreditRefDG.SelectedItem as Model.CreditReference;
                var localStatus = creditRefStatus.Single(x => x.StatusID == entity.StatusID);
                if (entity.Status != localStatus)
                {
                    entity.Status = localStatus;
                }
                CreditFormCommon();
                creditorForm.AddBTn.Content = "Edit";
                creditorForm.DataContext = entity;
                creditorForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        
        private void CreditRefAddBtn_Click(object sender, RoutedEventArgs e)
        {
            CreditFormCommon();
            creditorForm.DataContext = new Model.CreditReference { PersonalDataID = person.PersonalDataID, CreditReferenceID = Guid.NewGuid() };
            creditorForm.ShowDialog();
        }

        private void creditorFormCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            creditorForm.Close();
        }

        private void CreditorFormAddBTn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = creditorForm.DataContext as Model.CreditReference;
                item.StatusID = (creditorForm.CreditRefStatusCB.SelectedItem as Model.Status).StatusID;
                //item.Status = (creditorForm.CreditRefStatusCB.SelectedItem as Model.Status);
                if (!(sender as Button).Content.Equals("Edit"))
                {
                    person.CreditReferences.Add(item);
                    Main.PersonalData3.CreditRefDG.Items.Refresh();
                }

                creditorForm.Close();

            }
            catch(Exception ee)
            {
                var innerMessage = ee.InnerException != null ? ee.InnerException.Message : string.Empty;
                var message = string.Format("Error Message: {0} \n Inner Error Message: {1}", ee.Message, innerMessage);
                MessageBox.Show(message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void CreditFormCommon()
        {
            creditorForm = new CreditorForm();
            creditorForm.CreditRefStatusCB.ItemsSource = creditRefStatus;
            creditorForm.AddBTn.Click += CreditorFormAddBTn_Click;
            creditorForm.CancelBtn.Click += creditorFormCancelBtn_Click;
        }

        //Income Source
        private void IncomeSoureEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData2.IncomeSourceDG.SelectedIndex > -1)
            {
                sourceOfIncomeForm = new SourceOfIncomeForm();
                sourceOfIncomeForm.AddBTn.Click += IncomeSoureFormAddBTn_Click;
                sourceOfIncomeForm.CancelBtn.Click += IncomeSoureFormCancelBtn_Click;
                sourceOfIncomeForm.AddBTn.Content = "Edit";
                sourceOfIncomeForm.DataContext = Main.PersonalData2.IncomeSourceDG.SelectedItem as Model.IncomeSource;
                sourceOfIncomeForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void IncomeSoureDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData2.IncomeSourceDG.SelectedIndex > -1)
            {
                var incomeSource = Main.PersonalData2.IncomeSourceDG.SelectedItem as Model.IncomeSource;
                if (MessageBox.Show("Are you sure you want to remove this object?", "Removing Income Source object", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    person.IncomeSources.Remove(incomeSource);
                    Main.PersonalData2.IncomeSourceDG.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void IncomeSoureAddBtn_Click(object sender, RoutedEventArgs e)
        {
            //sourceOfIncomeForm
            sourceOfIncomeForm = new SourceOfIncomeForm();
            sourceOfIncomeForm.AddBTn.Click += IncomeSoureFormAddBTn_Click;
            sourceOfIncomeForm.CancelBtn.Click += IncomeSoureFormCancelBtn_Click;

            sourceOfIncomeForm.DataContext = new Model.IncomeSource { PersonalDataID = person.PersonalDataID, IncomeSourceID = Guid.NewGuid() };
            sourceOfIncomeForm.ShowDialog();
        }

        private void IncomeSoureFormCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            sourceOfIncomeForm.Close();
        }

        private void IncomeSoureFormAddBTn_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender as Button).Content.Equals("Edit"))
            {
                var incomeSource = sourceOfIncomeForm.DataContext as Model.IncomeSource;
                person.IncomeSources.Add(incomeSource);
                Main.PersonalData2.IncomeSourceDG.Items.Refresh();
            }
            sourceOfIncomeForm.Close();
        }


        //Employees
        private void DeleteEmployerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PersonalData1.EmployerDG.SelectedIndex > -1)
            {
                var employer = Main.PersonalData1.EmployerDG.SelectedItem as Model.Employer;
                if(MessageBox.Show("Are you sure you want to remove this employer?","Removing Employer", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    person.Employers.Remove(employer);
                    Main.PersonalData1.EmployerDG.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void EditEmployerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Main.PersonalData1.EmployerDG.SelectedIndex > -1)
            {
                var entity = Main.PersonalData1.EmployerDG.SelectedItem as Model.Employer;
                var localStatus = entity.Status = employerStatus.Single(x => x.StatusID == entity.StatusID);
                if (entity.Status != localStatus)
                {
                    entity.Status = localStatus;
                }
                EmployerFormCommon();
                employerForm.EmployerAddBtn.Content = "Edit";
                employerForm.DataContext = entity;
                employerForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select one object on the Grid Table");
            }
        }

        private void AddEmployerBtn_Click(object sender, RoutedEventArgs e)
        {
            EmployerFormCommon();
            employerForm.DataContext = new Model.Employer() { PersonalDataID = person.PersonalDataID, EmployerID = Guid.NewGuid() };
            employerForm.ShowDialog();
        }

        private void EmployerFormCommon()
        {
            employerForm = new EmployerForm();
            employerForm.EmploymentStatusCB.ItemsSource = employerStatus;
            employerForm.EmployerAddBtn.Click += EmployerFormAddBtn_Click;
            employerForm.EmployerCancelBtn.Click += EmployerFormCancelBtn_Click;
        }

        private void EmployerFormCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            employerForm.Close();
        }

        private void EmployerFormAddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employer = employerForm.DataContext as Model.Employer;
                employer.StatusID = (employerForm.EmploymentStatusCB.SelectedItem as Model.Status).StatusID;
                //employer.Status = (employerForm.EmploymentStatusCB.SelectedItem as Model.Status);
                if (!(sender as Button).Content.Equals("Edit"))
                {
                    person.Employers.Add(employer);
                    Main.PersonalData1.EmployerDG.Items.Refresh();
                }

                employerForm.Close();
            }
            catch(Exception ee)
            {
                string addMsg = "Error 2: Probably field are empty on saving and needed to fill, please check the input field again";
                var innerMessage = ee.InnerException != null ? ee.InnerException.Message : string.Empty;
                var message = string.Format("{0} \n Error Message: {1} \n Inner Error Message: {2}", addMsg, ee.Message, innerMessage);
                MessageBox.Show(message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Init()
        {
            if (Crud.Equals(CrudEnums.Read))
            {
                Main.submit_button.Content = "Close";
            }
            if (Crud.Equals(CrudEnums.Edit))
            {
                Main.submit_button.Content = "Edit";
                Main.PersonalData0.loanSP.IsEnabled = false;
                person.LoanApplications.Clear();
            }
            if (person.PersonalDataID.Equals(Guid.Empty))
            {
                person.PersonalDataID = Guid.NewGuid();
                person.Employers = new List<Model.Employer>();
                person.IncomeSources = new List<Model.IncomeSource>();
                person.CreditReferences = new List<Model.CreditReference>();
                person.PersonalReferences = new List<Model.PersonalReference>();
                person.Properties = new List<Model.Property>();
                person.DigitalInfos = new List<Model.DigitalInfo>();
                person.LoanApplications = new List<Model.LoanApplication>();
                person.PersonalDataAccounts = new List<Model.PersonalDataAccount>();
                //person.Loans = new List<Model.Loan>();
            }
            if(person.PersonalDataAccounts.Count() < 1)
            {
                currentAccountNumber = string.Empty;
                person.PersonalDataAccounts.Add(new Model.PersonalDataAccount() {
                    PersonalDataAccountID = Guid.NewGuid(),
                    AccountNumber = GenerateAccountNumber(),
                    IsApproved = false,
                    PersonalDataID = person.PersonalDataID
                });
            }
            else
            {
                currentAccountNumber = person.PersonalDataAccounts.First().AccountNumber;
            }
            

            if (person.DigitalInfos.Count() > 0)
            {
                var imageSource = CameraHelper.ConvertToBitmapSource(person.DigitalInfos.FirstOrDefault().Photo);
                Main.PersonalData0.PersonalDataPhoto.Source = imageSource;
            }

            loanApplication = new Model.LoanApplication()
            {
                LoanApplicationID = Guid.NewGuid(),
                PersonalDataID = person.PersonalDataID,
                StatusID = StatusNew.StatusID,
                ApplicationDate = DateTime.Now
                
            };

            if(!Crud.Equals(CrudEnums.Edit))
            {
                person.LoanApplications.Add(loanApplication);
            }

            Main.PersonalData0.DataContext = new {
                PersonalDataAccount = person.PersonalDataAccounts.First(),
                PersonalDataContext = person,
                LoanApplicationContext = loanApplication
            };
            
            Main.PersonalData0.LoanTermCB.ItemsSource = terms;

            Main.PersonalData1.EmployerDG.ItemsSource = person.Employers;
            Main.PersonalData2.IncomeSourceDG.ItemsSource = person.IncomeSources;
            Main.PersonalData3.CreditRefDG.ItemsSource = person.CreditReferences;
            Main.PersonalData4.PersonalRefDG.ItemsSource = person.PersonalReferences;
            Main.PersonalData5.PropertyDG.ItemsSource = person.Properties;
        }

        private void submit_button_Click(object sender, RoutedEventArgs e)
        {
            //Add validation
            //Save to DB
            try
            {
                if(!Crud.Equals(CrudEnums.Read))
                {
                    if(!Crud.Equals(CrudEnums.Edit))
                    {
                        loanApplication.CreditTermID = (Main.PersonalData0.LoanTermCB.SelectedItem as Model.CreditTerm).CreditTermID;
                    }

                    //check Account number
                    if (person.PersonalDataAccounts.First().AccountNumber != currentAccountNumber 
                        && PersonalDataAccountManager.IsAccountNumberExist(person.PersonalDataAccounts.First().AccountNumber))
                    {
                        throw new Exception("Account Number Already Exist please generate again.");
                    }

                    if(person.DigitalInfos.Count() == 0 && Main.PersonalData0.PersonalDataPhoto.Source != null)
                    {
                        person.DigitalInfos = new List<Model.DigitalInfo>();
                        person.DigitalInfos.Add(new Model.DigitalInfo()
                        {
                            PersonalDataID = person.PersonalDataID,
                            DigitalInfoID = Guid.NewGuid(),
                            Photo = CameraHelper.ConvertToByteArray((Main.PersonalData0.PersonalDataPhoto.Source as BitmapSource))
                        });
                    }
                    else if(Main.PersonalData0.PersonalDataPhoto.Source != null)
                    {
                        person.DigitalInfos.FirstOrDefault().Photo = CameraHelper.ConvertToByteArray((Main.PersonalData0.PersonalDataPhoto.Source as BitmapSource));
                    }
                    
                    person.DisplayName = string.Format("{0} {1} {2}", person.FirstName, person.MiddleName, person.LastName);
                    person.Gender = Main.PersonalData0.GenderCB.Text;

                    if (PersonalDataManager.Exist(person.PersonalDataID))
                    {
                        person.LastUpdatedDate = DateTime.Now;
                        PersonalDataManager.SaveorUpdate(person);
                    }
                    else
                    {
                        clearDependents();
                        person.CreatedDate = DateTime.Now;
                        PersonalDataManager.Add(person);
                    }
                    MessageBox.Show("Successfully Save Transaction...", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                Main.Close();
            }
            catch(Exception ee)
            {
                string addMsg = "Error 1: Probably field are empty on saving and needed to fill, please check the input field again";
                var innerMessage = ee.InnerException != null ? ee.InnerException.Message : string.Empty;
                var message = string.Format("{0} \n Error Message: {1} \n Inner Error Message: {2}", addMsg, ee.Message, innerMessage);
                MessageBox.Show(message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void clearDependents()
        {
            foreach(var entity in person.Employers)
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
                entity.Status =null;
            }
            foreach (var entity in person.PersonalDataAccounts)
            {
                entity.PersonalData = null;
            }
        }
    }
}
