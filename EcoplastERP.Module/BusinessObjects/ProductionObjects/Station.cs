using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Project")]
    [DefaultProperty("Code")]
    [NavigationItem("ProductionManagement")]
    public class Station : BaseObject
    {
        public Station(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private string _TableName;
        private bool _IsReadResource;
        private bool _IsLastStation;
        private decimal _PaletteOption;
        private SalesOrderStatus _SalesOrderStatus;
        private Warehouse _QuarantineWarehouse;
        private Warehouse _WastageWarehouse;
        private Warehouse _SourceWarehouse;
        private string _Name;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
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

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
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
        public Warehouse SourceWarehouse
        {
            get
            {
                return _SourceWarehouse;
            }
            set
            {
                SetPropertyValue("SourceWarehouse", ref _SourceWarehouse, value);
            }
        }

        [RuleRequiredField]
        public Warehouse WastageWarehouse
        {
            get
            {
                return _WastageWarehouse;
            }
            set
            {
                SetPropertyValue("WastageWarehouse", ref _WastageWarehouse, value);
            }
        }

        public Warehouse QuarantineWarehouse
        {
            get
            {
                return _QuarantineWarehouse;
            }
            set
            {
                SetPropertyValue("QuarantineWarehouse", ref _QuarantineWarehouse, value);
            }
        }

        public SalesOrderStatus SalesOrderStatus
        {
            get
            {
                return _SalesOrderStatus;
            }
            set
            {
                SetPropertyValue("SalesOrderStatus", ref _SalesOrderStatus, value);
            }
        }

        [Appearance("Station.PaletteOption", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Name != 'Son Üretim'")]
        public decimal PaletteOption
        {
            get
            {
                return _PaletteOption;
            }
            set
            {
                SetPropertyValue("PaletteOption", ref _PaletteOption, value);
            }
        }

        public bool IsLastStation
        {
            get
            {
                return _IsLastStation;
            }
            set
            {
                SetPropertyValue("IsLastStation", ref _IsLastStation, value);
            }
        }

        public bool IsReadResource
        {
            get
            {
                return _IsReadResource;
            }
            set
            {
                SetPropertyValue("IsReadResource", ref _IsReadResource, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                SetPropertyValue("TableName", ref _TableName, value);
            }
        }
    }
}
