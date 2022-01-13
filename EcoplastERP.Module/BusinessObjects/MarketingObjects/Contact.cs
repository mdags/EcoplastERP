using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;
using EcoplastERP.Module.BusinessObjects.MaintenanceObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Department")]
    [DefaultProperty("Name")]
    [NavigationItem("MarketingManagement")]
    [Appearance("Contact.SalesOrderBlock", TargetItems = "Name", Criteria = "SalesOrderBlock = true", BackColor = "MistyRose")]
    public class Contact : BaseObject
    {
        public Contact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Country = Session.FindObject<Country>(new BinaryOperator("Name", "TÜRKÝYE"));
            LegalEntity = true;
            CreatedDate = Helpers.GetSystemDate(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (LegalEntity)
            {
                if (DistributionChannel != null)
                {
                    if (DistributionChannel.Code != "YD")
                    {
                        if (TaxNumber.Length != 10)
                        {
                            throw new UserFriendlyException("Vergi numarasý 10 hane olmalýdýr.");
                        }
                    }
                }
            }
            else if (!LegalEntity)
            {
                if (DistributionChannel != null)
                {
                    if (DistributionChannel.Code != "YD" && ContactType.Code != "S")
                    {
                        if (IdentityNumber.Length != 11)
                        {
                            throw new UserFriendlyException("TC Kimlik No 11 hane olmalýdýr.");
                        }
                    }
                }
            }
        }
        // Fields...
        private TruckDriver _FastShippingTruckDriver;
        private Truck _FastShippingTruck;
        private Warehouse _FastShippingWarehouse;
        private bool _TruckBlock;
        private bool _SalesOrderBlock;
        private string _Note;
        private DateTime _CreatedDate;
        private decimal _RiskAmount;
        private PaymentMethod _PaymentMethod;
        private Employee _CardFollower;
        private Salesman _Salesman;
        private string _Web;
        private string _Email;
        private string _IdentityNumber;
        private string _TaxNumber;
        private string _TaxOffice;
        private string _Fax;
        private string _Phone;
        private string _Address;
        private District _District;
        private City _City;
        private Country _Country;
        private Contact _MainContact;
        private ContactGroup5 _ContactGroup5;
        private ContactGroup4 _ContactGroup4;
        private ContactGroup3 _ContactGroup3;
        private ContactGroup2 _ContactGroup2;
        private ContactGroup1 _ContactGroup1;
        private DistributionChannel _DistributionChannel;
        private bool _LegalEntity;
        private ContactType _ContactType;
        private string _Name;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "MainContact is null")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        [RuleRequiredField]
        public ContactType ContactType
        {
            get
            {
                return _ContactType;
            }
            set
            {
                SetPropertyValue("ContactType", ref _ContactType, value);
            }
        }

        [ImmediatePostData]
        public bool LegalEntity
        {
            get
            {
                return _LegalEntity;
            }
            set
            {
                SetPropertyValue("LegalEntity", ref _LegalEntity, value);
            }
        }

        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "MainContact is null")]
        public DistributionChannel DistributionChannel
        {
            get
            {
                return _DistributionChannel;
            }
            set
            {
                SetPropertyValue("DistributionChannel", ref _DistributionChannel, value);
            }
        }

        public ContactGroup1 ContactGroup1
        {
            get
            {
                return _ContactGroup1;
            }
            set
            {
                SetPropertyValue("ContactGroup1", ref _ContactGroup1, value);
            }
        }

        public ContactGroup2 ContactGroup2
        {
            get
            {
                return _ContactGroup2;
            }
            set
            {
                SetPropertyValue("ContactGroup2", ref _ContactGroup2, value);
            }
        }

        public ContactGroup3 ContactGroup3
        {
            get
            {
                return _ContactGroup3;
            }
            set
            {
                SetPropertyValue("ContactGroup3", ref _ContactGroup3, value);
            }
        }

        public ContactGroup4 ContactGroup4
        {
            get
            {
                return _ContactGroup4;
            }
            set
            {
                SetPropertyValue("ContactGroup4", ref _ContactGroup4, value);
            }
        }

        public ContactGroup5 ContactGroup5
        {
            get
            {
                return _ContactGroup5;
            }
            set
            {
                SetPropertyValue("ContactGroup5", ref _ContactGroup5, value);
            }
        }

        public Contact MainContact
        {
            get
            {
                return _MainContact;
            }
            set
            {
                SetPropertyValue("MainContact", ref _MainContact, value);
            }
        }

        public Country Country
        {
            get
            {
                return _Country;
            }
            set
            {
                SetPropertyValue("Country", ref _Country, value);
            }
        }

        [DataSourceCriteria("Country = '@this.Country'")]
        public City City
        {
            get
            {
                return _City;
            }
            set
            {
                SetPropertyValue("City", ref _City, value);
            }
        }

        [DataSourceCriteria("City = '@this.City'")]
        public District District
        {
            get
            {
                return _District;
            }
            set
            {
                SetPropertyValue("District", ref _District, value);
            }
        }

        [RuleRequiredField]
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

        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "MainContact is null")]
        [Size(16)]
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                SetPropertyValue("Phone", ref _Phone, value);
            }
        }

        [Size(16)]
        public string Fax
        {
            get
            {
                return _Fax;
            }
            set
            {
                SetPropertyValue("Fax", ref _Fax, value);
            }
        }

        [Appearance("Contact.TaxOffice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "LegalEntity = false")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "LegalEntity = true and DistributionChannel.Name = 'Yurt Ýçi' and MainContact is null")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TaxOffice
        {
            get
            {
                return _TaxOffice;
            }
            set
            {
                SetPropertyValue("TaxOffice", ref _TaxOffice, value);
            }
        }

        [Appearance("Contact.TaxNumber", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "LegalEntity = false")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "LegalEntity = true and DistributionChannel.Name = 'Yurt Ýçi' and MainContact is null")]
        [RuleUniqueValue(TargetCriteria = "IsNewObject(this) and LegalEntity = true and DistributionChannel.Name != 'Yurt Dýþý'")]
        [Size(10)]
        public string TaxNumber
        {
            get
            {
                return _TaxNumber;
            }
            set
            {
                SetPropertyValue("TaxNumber", ref _TaxNumber, value);
            }
        }

        [Appearance("Contact.IdentityNumber", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "LegalEntity = true and MainContact is null")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "LegalEntity = false and MainContact is null")]
        [RuleUniqueValue(TargetCriteria = "IsNewObject(this) and LegalEntity = false")]
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

        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "MainContact is null")]
        [RuleRegularExpression(DefaultContexts.Save, Helpers.EmailRegularExpression, CustomMessageTemplate = "Geçersiz eposta adresi!", TargetCriteria = "MainContact is null")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetPropertyValue("Email", ref _Email, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Web
        {
            get
            {
                return _Web;
            }
            set
            {
                SetPropertyValue("Web", ref _Web, value);
            }
        }

        public Salesman Salesman
        {
            get
            {
                return _Salesman;
            }
            set
            {
                SetPropertyValue("Salesman", ref _Salesman, value);
            }
        }

        public Employee CardFollower
        {
            get
            {
                return _CardFollower;
            }
            set
            {
                SetPropertyValue("CardFollower", ref _CardFollower, value);
            }
        }

        public PaymentMethod PaymentMethod
        {
            get
            {
                return _PaymentMethod;
            }
            set
            {
                SetPropertyValue("PaymentMethod", ref _PaymentMethod, value);
            }
        }

        public decimal RiskAmount
        {
            get
            {
                return _RiskAmount;
            }
            set
            {
                SetPropertyValue("RiskAmount", ref _RiskAmount, value);
            }
        }

        public bool SalesOrderBlock
        {
            get
            {
                return _SalesOrderBlock;
            }
            set
            {
                SetPropertyValue("SalesOrderBlock", ref _SalesOrderBlock, value);
            }
        }

        public bool TruckBlock
        {
            get
            {
                return _TruckBlock;
            }
            set
            {
                SetPropertyValue("TruckBlock", ref _TruckBlock, value);
            }
        }

        public Warehouse FastShippingWarehouse
        {
            get
            {
                return _FastShippingWarehouse;
            }
            set
            {
                SetPropertyValue("FastShippingWarehouse", ref _FastShippingWarehouse, value);
            }
        }

        public Truck FastShippingTruck
        {
            get
            {
                return _FastShippingTruck;
            }
            set
            {
                SetPropertyValue("FastShippingTruck", ref _FastShippingTruck, value);
            }
        }

        public TruckDriver FastShippingTruckDriver
        {
            get
            {
                return _FastShippingTruckDriver;
            }
            set
            {
                SetPropertyValue("FastShippingTruckDriver", ref _FastShippingTruckDriver, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue("CreatedDate", ref _CreatedDate, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                SetPropertyValue("Note", ref _Note, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<ContactPerson> ContactPersons
        {
            get { return GetCollection<ContactPerson>("ContactPersons"); }
        }

        [Association, Aggregated]
        public XPCollection<ContactPortfolio> ContactPortfolios
        {
            get { return GetCollection<ContactPortfolio>("ContactPortfolios"); }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Association("MaintenanceWorkOrder-Subcontractors")]
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
        [Association("PeriodicMaintenance-Subcontractors")]
        public XPCollection<PeriodicMaintenance> PeriodicMaintenances
        {
            get
            {
                return GetCollection<PeriodicMaintenance>("PeriodicMaintenances");
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [Association("DemandDetail-OfferRequests")]
        public XPCollection<DemandDetail> DemandDetails
        {
            get
            {
                return GetCollection<DemandDetail>("DemandDetails");
            }
        }
    }
}