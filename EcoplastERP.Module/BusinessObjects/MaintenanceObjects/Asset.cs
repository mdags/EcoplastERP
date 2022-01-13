using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.PurchaseObjects;

namespace EcoplastERP.Module.BusinessObjects.MaintenanceObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Workflow_ShowWorkflowInstances")]
    [DefaultProperty("Name")]
    [NavigationItem("MaintenanceManagement")]
    public class Asset : HCategory
    {
        public Asset(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _Note;
        private DateTime _WarrantyEndDate;
        private DateTime _WarrantyStartDate;
        private DateTime _InstallationDate;
        private DateTime _ProductionDate;
        private decimal _BuyingPrice;
        private string _Barcode;
        private string _SerialNumber;
        private string _Model;
        private string _Trademark;
        private ExpenseCenter _ExpenseCenter;
        private Product _Product;
        private Machine _Machine;
        private AssetType _AssetType;
        private string _Code;

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
        public AssetType AssetType
        {
            get
            {
                return _AssetType;
            }
            set
            {
                SetPropertyValue("AssetType", ref _AssetType, value);
            }
        }

        public Machine Machine
        {
            get
            {
                return _Machine;
            }
            set
            {
                SetPropertyValue("Machine", ref _Machine, value);
            }
        }

        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
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

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Trademark
        {
            get
            {
                return _Trademark;
            }
            set
            {
                SetPropertyValue("Trademark", ref _Trademark, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Model
        {
            get
            {
                return _Model;
            }
            set
            {
                SetPropertyValue("Model", ref _Model, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string SerialNumber
        {
            get
            {
                return _SerialNumber;
            }
            set
            {
                SetPropertyValue("SerialNumber", ref _SerialNumber, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
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

        public decimal BuyingPrice
        {
            get
            {
                return _BuyingPrice;
            }
            set
            {
                SetPropertyValue("BuyingPrice", ref _BuyingPrice, value);
            }
        }

        public DateTime ProductionDate
        {
            get
            {
                return _ProductionDate;
            }
            set
            {
                SetPropertyValue("ProductionDate", ref _ProductionDate, value);
            }
        }

        public DateTime InstallationDate
        {
            get
            {
                return _InstallationDate;
            }
            set
            {
                SetPropertyValue("InstallationDate", ref _InstallationDate, value);
            }
        }

        public DateTime WarrantyStartDate
        {
            get
            {
                return _WarrantyStartDate;
            }
            set
            {
                SetPropertyValue("WarrantyStartDate", ref _WarrantyStartDate, value);
            }
        }

        public DateTime WarrantyEndDate
        {
            get
            {
                return _WarrantyEndDate;
            }
            set
            {
                SetPropertyValue("WarrantyEndDate", ref _WarrantyEndDate, value);
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
        public XPCollection<AssetPortfolio> AssetPortfolios
        {
            get { return GetCollection<AssetPortfolio>("AssetPortfolios"); }
        }
    }
}
