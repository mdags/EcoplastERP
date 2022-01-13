using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security.Strategy;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.BusinessObjects.HumanResourceObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Employee")]
    [DefaultProperty("NameSurname")]
    [NavigationItem("HumanResourceManagement")]
    public class Employee : BaseObject
    {
        public Employee(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Address;
        private ServiceStation _ServiceStation;
        private ServiceRoute _ServiceRoute;
        private int _CigaretCount;
        private string _CellPhone;
        private int _ShoeNumber;
        private int _PantSize;
        private int _DressSize;
        private BloodGroup _BloodGroup;
        private MaritalStatus _MaritalStatus;
        private int _ChildCount;
        private Gender _Gender;
        private Country _Nationality;
        private string _BirthPlace;
        private DateTime _BirthDate;
        private string _MotherName;
        private string _FatherName;
        private decimal _PerformancePoint;
        private decimal _TrialIntegrationPoint;
        private string _DisabledReason;
        private decimal _DisabledRate;
        private WorkStatus _WorkStatus;
        private EmployeeGroup _EmployeeGroup;
        private string _LeaveReason;
        private DateTime _LeaveDate;
        private DateTime _EntryDate;
        private GraduationClass _GraduationClass;
        private Graduation _Graduation;
        private decimal _ExperienceScore;
        private WorkDifficult _WorkDifficult;
        private EmployeeTask _EmployeeTask;
        private WorkPlace _WorkPlace;
        private WorkPosition _WorkPosition;
        private WorkShift _WorkShift;
        private WorkType _WorkType;
        private WorkClass _WorkClass;
        private ExpenseCenter _ExpenseCenter;
        private Qualification _Qualification;
        private FactoryShare _FactoryShare;
        private BusinessShare _BusinessShare;
        private Company _Company;
        private SalesGroup _SalesGroup;
        private SalesOffice _SalesOffice;
        private SalesOrganization _SalesOrganization;
        private DepartmentPart _DepartmentPart;
        private Department _Department;
        private string _NameSurname;
        private string _IdentityNumber;
        private string _QDMSNumber;
        private string _RegistrationNumber;
        private SecuritySystemUser _UserName;

        public SecuritySystemUser UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                SetPropertyValue("UserName", ref _UserName, value);
            }
        }

        [RuleUniqueValue(TargetCriteria = "LeaveDate is null")]
        [RuleRequiredField]
        public string RegistrationNumber
        {
            get
            {
                return _RegistrationNumber;
            }
            set
            {
                SetPropertyValue("RegistrationNumber", ref _RegistrationNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string QDMSNumber
        {
            get
            {
                return _QDMSNumber;
            }
            set
            {
                SetPropertyValue("QDMSNumber", ref _QDMSNumber, value);
            }
        }

        [Size(11)]
        public string IdentityNumber
        {
            get
            {
                return _IdentityNumber;
            }
            set
            {
                SetPropertyValue("IdentityNumber", ref _IdentityNumber, value);
            }
        }

        [RuleRequiredField]
        public string NameSurname
        {
            get
            {
                return _NameSurname;
            }
            set
            {
                SetPropertyValue("NameSurname", ref _NameSurname, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public Department Department
        {
            get
            {
                return _Department;
            }
            set
            {
                SetPropertyValue("Department", ref _Department, value);
            }
        }

        [RuleRequiredField]
        public DepartmentPart DepartmentPart
        {
            get
            {
                return _DepartmentPart;
            }
            set
            {
                SetPropertyValue("DepartmentPart", ref _DepartmentPart, value);
            }
        }

        //[Appearance("Employee.SalesOrganization", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Department.Name != 'SATIÞ'")]
        //[RuleRequiredField(DefaultContexts.Save, TargetCriteria = "Department.Code = 'D003'")]
        public SalesOrganization SalesOrganization
        {
            get
            {
                return _SalesOrganization;
            }
            set
            {
                SetPropertyValue("SalesOrganization", ref _SalesOrganization, value);
            }
        }

        //[Appearance("Employee.SalesOffice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Department.Name != 'SATIÞ'")]
        //[RuleRequiredField(DefaultContexts.Save, TargetCriteria = "Department.Code = 'D003'")]
        [DataSourceCriteria("SalesOrganization = '@this.SalesOrganization'")]
        public SalesOffice SalesOffice
        {
            get
            {
                return _SalesOffice;
            }
            set
            {
                SetPropertyValue("SalesOffice", ref _SalesOffice, value);
            }
        }

        //[Appearance("Employee.SalesGroup", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Department.Name != 'SATIÞ'")]
        //[RuleRequiredField(DefaultContexts.Save, TargetCriteria = "Department.Code = 'D003'")]
        [DataSourceCriteria("SalesOffice = '@this.SalesOffice'")]
        public SalesGroup SalesGroup
        {
            get
            {
                return _SalesGroup;
            }
            set
            {
                SetPropertyValue("SalesGroup", ref _SalesGroup, value);
            }
        }

        public Company Company
        {
            get
            {
                return _Company;
            }
            set
            {
                SetPropertyValue("Company", ref _Company, value);
            }
        }

        public BusinessShare BusinessShare
        {
            get
            {
                return _BusinessShare;
            }
            set
            {
                SetPropertyValue("BusinessShare", ref _BusinessShare, value);
            }
        }

        public FactoryShare FactoryShare
        {
            get
            {
                return _FactoryShare;
            }
            set
            {
                SetPropertyValue("FactoryShare", ref _FactoryShare, value);
            }
        }

        [DataSourceCriteria("Company = '@this.Company'")]
        public Qualification Qualification
        {
            get
            {
                return _Qualification;
            }
            set
            {
                SetPropertyValue("Qualification", ref _Qualification, value);
            }
        }

        public ExpenseCenter ExpenseCenter
        {
            get
            {
                return _ExpenseCenter;
            }
            set
            {
                SetPropertyValue("ExpenseCenter", ref _ExpenseCenter, value);
            }
        }

        public WorkClass WorkClass
        {
            get
            {
                return _WorkClass;
            }
            set
            {
                SetPropertyValue("WorkClass", ref _WorkClass, value);
            }
        }

        public WorkType WorkType
        {
            get
            {
                return _WorkType;
            }
            set
            {
                SetPropertyValue("WorkType", ref _WorkType, value);
            }
        }

        public WorkShift WorkShift
        {
            get
            {
                return _WorkShift;
            }
            set
            {
                SetPropertyValue("WorkShift", ref _WorkShift, value);
            }
        }

        public WorkPosition WorkPosition
        {
            get
            {
                return _WorkPosition;
            }
            set
            {
                SetPropertyValue("WorkPosition", ref _WorkPosition, value);
            }
        }

        public WorkPlace WorkPlace
        {
            get
            {
                return _WorkPlace;
            }
            set
            {
                SetPropertyValue("WorkPlace", ref _WorkPlace, value);
            }
        }

        public EmployeeTask EmployeeTask
        {
            get
            {
                return _EmployeeTask;
            }
            set
            {
                SetPropertyValue("EmployeeTask", ref _EmployeeTask, value);
            }
        }

        public WorkDifficult WorkDifficult
        {
            get
            {
                return _WorkDifficult;
            }
            set
            {
                SetPropertyValue("WorkDifficult", ref _WorkDifficult, value);
            }
        }

        public decimal ExperienceScore
        {
            get
            {
                return _ExperienceScore;
            }
            set
            {
                SetPropertyValue("ExperienceScore", ref _ExperienceScore, value);
            }
        }

        public Graduation Graduation
        {
            get
            {
                return _Graduation;
            }
            set
            {
                SetPropertyValue("Graduation", ref _Graduation, value);
            }
        }

        public GraduationClass GraduationClass
        {
            get
            {
                return _GraduationClass;
            }
            set
            {
                SetPropertyValue("GraduationClass", ref _GraduationClass, value);
            }
        }

        public DateTime EntryDate
        {
            get
            {
                return _EntryDate;
            }
            set
            {
                SetPropertyValue("EntryDate", ref _EntryDate, value);
            }
        }

        public DateTime LeaveDate
        {
            get
            {
                return _LeaveDate;
            }
            set
            {
                SetPropertyValue("LeaveDate", ref _LeaveDate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LeaveReason
        {
            get
            {
                return _LeaveReason;
            }
            set
            {
                SetPropertyValue("LeaveReason", ref _LeaveReason, value);
            }
        }

        public EmployeeGroup EmployeeGroup
        {
            get
            {
                return _EmployeeGroup;
            }
            set
            {
                SetPropertyValue("EmployeeGroup", ref _EmployeeGroup, value);
            }
        }

        public WorkStatus WorkStatus
        {
            get
            {
                return _WorkStatus;
            }
            set
            {
                SetPropertyValue("WorkStatus", ref _WorkStatus, value);
            }
        }

        public decimal DisabledRate
        {
            get
            {
                return _DisabledRate;
            }
            set
            {
                SetPropertyValue("DisabledRate", ref _DisabledRate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DisabledReason
        {
            get
            {
                return _DisabledReason;
            }
            set
            {
                SetPropertyValue("DisabledReason", ref _DisabledReason, value);
            }
        }

        public decimal TrialIntegrationPoint
        {
            get
            {
                return _TrialIntegrationPoint;
            }
            set
            {
                SetPropertyValue("TrialIntegrationPoint", ref _TrialIntegrationPoint, value);
            }
        }

        public decimal PerformancePoint
        {
            get
            {
                return _PerformancePoint;
            }
            set
            {
                SetPropertyValue("PerformancePoint", ref _PerformancePoint, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FatherName
        {
            get
            {
                return _FatherName;
            }
            set
            {
                SetPropertyValue("FatherName", ref _FatherName, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string MotherName
        {
            get
            {
                return _MotherName;
            }
            set
            {
                SetPropertyValue("MotherName", ref _MotherName, value);
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                SetPropertyValue("BirthDate", ref _BirthDate, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BirthPlace
        {
            get
            {
                return _BirthPlace;
            }
            set
            {
                SetPropertyValue("BirthPlace", ref _BirthPlace, value);
            }
        }
        
        public Country Nationality
        {
            get
            {
                return _Nationality;
            }
            set
            {
                SetPropertyValue("Nationality", ref _Nationality, value);
            }
        }

        public Gender Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                SetPropertyValue("Gender", ref _Gender, value);
            }
        }

        public int ChildCount
        {
            get
            {
                return _ChildCount;
            }
            set
            {
                SetPropertyValue("ChildCount", ref _ChildCount, value);
            }
        }

        public MaritalStatus MaritalStatus
        {
            get
            {
                return _MaritalStatus;
            }
            set
            {
                SetPropertyValue("MaritalStatus", ref _MaritalStatus, value);
            }
        }

        public BloodGroup BloodGroup
        {
            get
            {
                return _BloodGroup;
            }
            set
            {
                SetPropertyValue("BloodGroup", ref _BloodGroup, value);
            }
        }

        public int DressSize
        {
            get
            {
                return _DressSize;
            }
            set
            {
                SetPropertyValue("DressSize", ref _DressSize, value);
            }
        }

        public int PantSize
        {
            get
            {
                return _PantSize;
            }
            set
            {
                SetPropertyValue("PantSize", ref _PantSize, value);
            }
        }

        public int ShoeNumber
        {
            get
            {
                return _ShoeNumber;
            }
            set
            {
                SetPropertyValue("ShoeNumber", ref _ShoeNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CellPhone
        {
            get
            {
                return _CellPhone;
            }
            set
            {
                SetPropertyValue("CellPhone", ref _CellPhone, value);
            }
        }

        public int CigaretCount
        {
            get
            {
                return _CigaretCount;
            }
            set
            {
                SetPropertyValue("CigaretCount", ref _CigaretCount, value);
            }
        }

        public ServiceRoute ServiceRoute
        {
            get
            {
                return _ServiceRoute;
            }
            set
            {
                SetPropertyValue("ServiceRoute", ref _ServiceRoute, value);
            }
        }

        public ServiceStation ServiceStation
        {
            get
            {
                return _ServiceStation;
            }
            set
            {
                SetPropertyValue("ServiceStation", ref _ServiceStation, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                SetPropertyValue("Address", ref _Address, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<EmployeePortfolio> EmployeePortfolios
        {
            get { return GetCollection<EmployeePortfolio>("EmployeePortfolios"); }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Association("MaintenanceTeam-Employees")]
        public XPCollection<MaintenanceTeam> MaintenanceTeams
        {
            get
            {
                return GetCollection<MaintenanceTeam>("MaintenanceTeams");
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Association("MaintenanceWorkOrder-Employees")]
        public XPCollection<MaintenanceWorkOrder> MaintenanceWorkOrders
        {
            get
            {
                return GetCollection<MaintenanceWorkOrder>("MaintenanceWorkOrders");
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Association("PeriodicMaintenance-Employees")]
        public XPCollection<PeriodicMaintenance> PeriodicMaintenances
        {
            get
            {
                return GetCollection<PeriodicMaintenance>("PeriodicMaintenances");
            }
        }
    }
}
