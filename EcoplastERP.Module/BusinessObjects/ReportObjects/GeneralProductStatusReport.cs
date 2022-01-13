using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace EcoplastERP.Module.BusinessObjects.ReportObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Report")]
    [DefaultProperty("ProductKind")]
    [NavigationItem(false)]
    public class GeneralProductStatusReport : BaseObject
    {
        public GeneralProductStatusReport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _FilmingProductionQuantity;
        private decimal _CastQuantity;
        private decimal _LaminationQuantity;
        private decimal _SlicingQuantity;
        private decimal _CuttingQuantity;
        private decimal _PrintingQuantity;
        private decimal _StorecQuantity;
        private decimal _WaitingcQuantity;
        private decimal _ShippedcQuantity;
        private decimal _SalesOrdercQuantity;
        private string _ProductKindGroup;
        private string _ProductKind;
        private string _ProductType;
        private string _ProductGroup;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductGroup
        {
            get
            {
                return _ProductGroup;
            }
            set
            {
                SetPropertyValue("ProductGroup", ref _ProductGroup, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductType
        {
            get
            {
                return _ProductType;
            }
            set
            {
                SetPropertyValue("ProductType", ref _ProductType, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductKind
        {
            get
            {
                return _ProductKind;
            }
            set
            {
                SetPropertyValue("ProductKind", ref _ProductKind, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductKindGroup
        {
            get
            {
                return _ProductKindGroup;
            }
            set
            {
                SetPropertyValue("ProductKindGroup", ref _ProductKindGroup, value);
            }
        }

        public decimal SalesOrdercQuantity
        {
            get
            {
                return _SalesOrdercQuantity;
            }
            set
            {
                SetPropertyValue("SalesOrdercQuantity", ref _SalesOrdercQuantity, value);
            }
        }

        public decimal ShippedcQuantity
        {
            get
            {
                return _ShippedcQuantity;
            }
            set
            {
                SetPropertyValue("ShippedcQuantity", ref _ShippedcQuantity, value);
            }
        }

        public decimal WaitingcQuantity
        {
            get
            {
                return _WaitingcQuantity;
            }
            set
            {
                SetPropertyValue("WaitingcQuantity", ref _WaitingcQuantity, value);
            }
        }

        public decimal StorecQuantity
        {
            get
            {
                return _StorecQuantity;
            }
            set
            {
                SetPropertyValue("StorecQuantity", ref _StorecQuantity, value);
            }
        }

        public decimal PrintingQuantity
        {
            get
            {
                return _PrintingQuantity;
            }
            set
            {
                SetPropertyValue("PrintingQuantity", ref _PrintingQuantity, value);
            }
        }

        public decimal CuttingQuantity
        {
            get
            {
                return _CuttingQuantity;
            }
            set
            {
                SetPropertyValue("CuttingQuantity", ref _CuttingQuantity, value);
            }
        }

        public decimal SlicingQuantity
        {
            get
            {
                return _SlicingQuantity;
            }
            set
            {
                SetPropertyValue("SlicingQuantity", ref _SlicingQuantity, value);
            }
        }

        public decimal LaminationQuantity
        {
            get
            {
                return _LaminationQuantity;
            }
            set
            {
                SetPropertyValue("LaminationQuantity", ref _LaminationQuantity, value);
            }
        }

        public decimal CastQuantity
        {
            get
            {
                return _CastQuantity;
            }
            set
            {
                SetPropertyValue("CastQuantity", ref _CastQuantity, value);
            }
        }

        public decimal FilmingProductionQuantity
        {
            get
            {
                return _FilmingProductionQuantity;
            }
            set
            {
                SetPropertyValue("FilmingProductionQuantity", ref _FilmingProductionQuantity, value);
            }
        }
    }
}
