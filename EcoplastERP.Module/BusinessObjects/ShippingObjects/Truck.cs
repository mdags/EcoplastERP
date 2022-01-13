using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Vendor")]
    [DefaultProperty("PlateNumber")]
    [NavigationItem("ShippingManagement")]
    public class Truck : BaseObject
    {
        public Truck(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            PlateNumber = PlateNumber.ToUpper().Trim().Replace(" ", "");
        }
        // Fields...
        private DateTime _AdministrationDate;
        private DateTime _ExpirationDate;
        private string _KDocument;
        private decimal _Payload;
        private DateTime _PermitDate;
        private string _PermitNumber;
        private decimal _Tare;
        private string _Model;
        private string _DorsePlate;
        private string _PlateNumber;
        private TruckType _TruckType;

        [RuleRequiredField]
        [Size(10)]
        public string PlateNumber
        {
            get
            {
                return _PlateNumber;
            }
            set
            {
                SetPropertyValue("PlateNumber", ref _PlateNumber, value);
            }
        }

        [VisibleInDetailView(false)]
        public TruckType TruckType
        {
            get
            {
                return _TruckType;
            }
            set
            {
                SetPropertyValue("TruckType", ref _TruckType, value);
            }
        }

        [Size(10)]
        public string DorsePlate
        {
            get
            {
                return _DorsePlate;
            }
            set
            {
                SetPropertyValue("DorsePlate", ref _DorsePlate, value);
            }
        }

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

        public decimal Tare
        {
            get
            {
                return _Tare;
            }
            set
            {
                SetPropertyValue("Tare", ref _Tare, value);
            }
        }
        
        public string PermitNumber
        {
            get
            {
                return _PermitNumber;
            }
            set
            {
                SetPropertyValue("PermitNumber", ref _PermitNumber, value);
            }
        }
        
        public DateTime PermitDate
        {
            get
            {
                return _PermitDate;
            }
            set
            {
                SetPropertyValue("PermitDate", ref _PermitDate, value);
            }
        }

        public decimal Payload
        {
            get
            {
                return _Payload;
            }
            set
            {
                SetPropertyValue("Payload", ref _Payload, value);
            }
        }

        public string KDocument
        {
            get
            {
                return _KDocument;
            }
            set
            {
                SetPropertyValue("KDocument", ref _KDocument, value);
            }
        }

        public DateTime ExpirationDate
        {
            get
            {
                return _ExpirationDate;
            }
            set
            {
                SetPropertyValue("ExpirationDate", ref _ExpirationDate, value);
            }
        }

        public DateTime AdministrationDate
        {
            get
            {
                return _AdministrationDate;
            }
            set
            {
                SetPropertyValue("AdministrationDate", ref _AdministrationDate, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<TruckDriver> TruckDrivers
        {
            get { return GetCollection<TruckDriver>("TruckDrivers"); }
        }

        [Association, Aggregated]
        public XPCollection<TruckPortfolio> TruckPortfolios
        {
            get { return GetCollection<TruckPortfolio>("TruckPortfolios"); }
        }
    }
}
