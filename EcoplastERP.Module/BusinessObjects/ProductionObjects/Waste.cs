using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("Action_ModelDifferences_Import")]
    [DefaultProperty("WasteCode.Name")]
    [NavigationItem("ProductionManagement")]
    public class Waste : BaseObject
    {
        public Waste(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            WasteDate = Helpers.GetSystemDate(Session);
        }
        // Fields...        
        private decimal _Quantity;
        private WasteCode _WasteCode;
        private Station _Station;
        private string _Barcode;
        private DateTime _WasteDate;

        [RuleRequiredField]
        public DateTime WasteDate
        {
            get
            {
                return _WasteDate;
            }
            set
            {
                SetPropertyValue("WasteDate", ref _WasteDate, value);
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

        public Station Station
        {
            get
            {
                return _Station;
            }
            set
            {
                SetPropertyValue("Station", ref _Station, value);
            }
        }

        public WasteCode WasteCode
        {
            get
            {
                return _WasteCode;
            }
            set
            {
                SetPropertyValue("WasteCode", ref _WasteCode, value);
            }
        }

        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
            }
        }
    }
}
