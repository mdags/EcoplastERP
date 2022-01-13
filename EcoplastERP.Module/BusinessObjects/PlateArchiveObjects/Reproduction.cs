using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PlateArchiveObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contract")]
    [DefaultProperty("ReproductionNumber")]
    [NavigationItem("PlateArchiveManagement")]
    public class Reproduction : BaseObject
    {
        public Reproduction(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReproductionNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            ReproductionDate = DateTime.Now;
            ReproductionStatus = ReproductionStatus.InSaleDepartment;
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        // Fields...
        private Employee _CreatedBy;
        private string _DesignedBy;
        private string _Instructions;
        private bool _OFax;
        private bool _OGmg;
        private bool _OModel;
        private bool _OOneToOne;
        private bool _OEmail;
        private bool _OA4;
        private PrintWorkStatus _PrintWorkStatus;
        private bool _REmail;
        private bool _RFax;
        private bool _RDia;
        private bool _ROffset;
        private bool _RColor;
        private bool _RSample;
        private bool _RFtp;
        private bool _RCd;
        private PrintForm _PrintForm;
        private PrintSide _PrintSide;
        private bool _ManufacturerLogo;
        private string _RollerLength;
        private Int16 _BandCount;
        private string _MachineName;
        private MaterialFilmingType _MaterialFilmingType;
        private MaterialType _MaterialType;
        private string _Barcode;
        private string _Colors;
        private MaterialColor _MaterialColor;
        private ShippingFilmType _ShippingFilmType;
        private string _WorkSize;
        private string _WorkName;
        private Contact _Contact;
        private Priority _Delivery;
        private DateTime _ReproductionDate;
        private int _ReproductionNumber;
        private ReproductionStatus _ReproductionStatus;

        [NonCloneable]
        public ReproductionStatus ReproductionStatus
        {
            get
            {
                return _ReproductionStatus;
            }
            set
            {
                SetPropertyValue("ReproductionStatus", ref _ReproductionStatus, value);
            }
        }

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int ReproductionNumber
        {
            get
            {
                return _ReproductionNumber;
            }
            set
            {
                SetPropertyValue("ReproductionNumber", ref _ReproductionNumber, value);
            }
        }

        [NonCloneable]
        public DateTime ReproductionDate
        {
            get
            {
                return _ReproductionDate;
            }
            set
            {
                SetPropertyValue("ReproductionDate", ref _ReproductionDate, value);
            }
        }

        public Priority Delivery
        {
            get
            {
                return _Delivery;
            }
            set
            {
                SetPropertyValue("Delivery", ref _Delivery, value);
            }
        }

        [RuleRequiredField]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
            }
        }

        [RuleRequiredField]
        public string WorkName
        {
            get
            {
                return _WorkName;
            }
            set
            {
                SetPropertyValue("WorkName", ref _WorkName, value);
            }
        }

        [RuleRequiredField]
        public string WorkSize
        {
            get
            {
                return _WorkSize;
            }
            set
            {
                SetPropertyValue("WorkSize", ref _WorkSize, value);
            }
        }

        public ShippingFilmType ShippingFilmType
        {
            get
            {
                return _ShippingFilmType;
            }
            set
            {
                SetPropertyValue("ShippingFilmType", ref _ShippingFilmType, value);
            }
        }

        public MaterialColor MaterialColor
        {
            get
            {
                return _MaterialColor;
            }
            set
            {
                SetPropertyValue("MaterialColor", ref _MaterialColor, value);
            }
        }

        public string Colors
        {
            get
            {
                return _Colors;
            }
            set
            {
                SetPropertyValue("Colors", ref _Colors, value);
            }
        }

        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                SetPropertyValue("Barcode", ref _Barcode, value);
            }
        }

        public MaterialType MaterialType
        {
            get
            {
                return _MaterialType;
            }
            set
            {
                SetPropertyValue("MaterialType", ref _MaterialType, value);
            }
        }

        [RuleRequiredField]
        public MaterialFilmingType MaterialFilmingType
        {
            get
            {
                return _MaterialFilmingType;
            }
            set
            {
                SetPropertyValue("MaterialFilmingType", ref _MaterialFilmingType, value);
            }
        }

        public string MachineName
        {
            get
            {
                return _MachineName;
            }
            set
            {
                SetPropertyValue("MachineName", ref _MachineName, value);
            }
        }

        [ModelDefault("MinValue", "0"), ModelDefault("MaxValue", "100")]
        public Int16 BandCount
        {
            get
            {
                return _BandCount;
            }
            set
            {
                SetPropertyValue("BandCount", ref _BandCount, value);
            }
        }

        public string RollerLength
        {
            get
            {
                return _RollerLength;
            }
            set
            {
                SetPropertyValue("RollerLength", ref _RollerLength, value);
            }
        }

        [RuleRequiredField]
        public bool ManufacturerLogo
        {
            get
            {
                return _ManufacturerLogo;
            }
            set
            {
                SetPropertyValue("ManufacturerLogo", ref _ManufacturerLogo, value);
            }
        }

        [RuleRequiredField]
        public PrintSide PrintSide
        {
            get
            {
                return _PrintSide;
            }
            set
            {
                SetPropertyValue("PrintSide", ref _PrintSide, value);
            }
        }

        [RuleRequiredField]
        public PrintForm PrintForm
        {
            get
            {
                return _PrintForm;
            }
            set
            {
                SetPropertyValue("PrintForm", ref _PrintForm, value);
            }
        }

        public bool RCd
        {
            get
            {
                return _RCd;
            }
            set
            {
                SetPropertyValue("RCd", ref _RCd, value);
            }
        }

        public bool RFtp
        {
            get
            {
                return _RFtp;
            }
            set
            {
                SetPropertyValue("RFtp", ref _RFtp, value);
            }
        }

        public bool RSample
        {
            get
            {
                return _RSample;
            }
            set
            {
                SetPropertyValue("RSample", ref _RSample, value);
            }
        }

        public bool RColor
        {
            get
            {
                return _RColor;
            }
            set
            {
                SetPropertyValue("RColor", ref _RColor, value);
            }
        }

        public bool ROffset
        {
            get
            {
                return _ROffset;
            }
            set
            {
                SetPropertyValue("ROffset", ref _ROffset, value);
            }
        }

        public bool RDia
        {
            get
            {
                return _RDia;
            }
            set
            {
                SetPropertyValue("RDia", ref _RDia, value);
            }
        }

        public bool RFax
        {
            get
            {
                return _RFax;
            }
            set
            {
                SetPropertyValue("RFax", ref _RFax, value);
            }
        }

        public bool REmail
        {
            get
            {
                return _REmail;
            }
            set
            {
                SetPropertyValue("REmail", ref _REmail, value);
            }
        }

        [RuleRequiredField]
        public PrintWorkStatus PrintWorkStatus
        {
            get
            {
                return _PrintWorkStatus;
            }
            set
            {
                SetPropertyValue("PrintWorkStatus", ref _PrintWorkStatus, value);
            }
        }

        public bool OA4
        {
            get
            {
                return _OA4;
            }
            set
            {
                SetPropertyValue("OA4", ref _OA4, value);
            }
        }

        public bool OEmail
        {
            get
            {
                return _OEmail;
            }
            set
            {
                SetPropertyValue("OEmail", ref _OEmail, value);
            }
        }

        public bool OOneToOne
        {
            get
            {
                return _OOneToOne;
            }
            set
            {
                SetPropertyValue("OOneToOne", ref _OOneToOne, value);
            }
        }

        public bool OModel
        {
            get
            {
                return _OModel;
            }
            set
            {
                SetPropertyValue("OModel", ref _OModel, value);
            }
        }

        public bool OGmg
        {
            get
            {
                return _OGmg;
            }
            set
            {
                SetPropertyValue("OGmg", ref _OGmg, value);
            }
        }

        public bool OFax
        {
            get
            {
                return _OFax;
            }
            set
            {
                SetPropertyValue("OFax", ref _OFax, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Instructions
        {
            get
            {
                return _Instructions;
            }
            set
            {
                SetPropertyValue("Instructions", ref _Instructions, value);
            }
        }

        [VisibleInDetailView(false)]
        public string DesignedBy
        {
            get
            {
                return _DesignedBy;
            }
            set
            {
                SetPropertyValue("DesignedBy", ref _DesignedBy, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }

        [Association("Reproduction-ReproductionAccepts")]
        public XPCollection<ReproductionAccept> ReproductionAccepts
        {
            get
            {
                return GetCollection<ReproductionAccept>("ReproductionAccepts");
            }
        }

        [Association("Reproduction-Plates")]
        public XPCollection<Plate> Plates
        {
            get
            {
                return GetCollection<Plate>("Plates");
            }
        }
    }
}
