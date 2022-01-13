using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.ProductionObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("ProductionBarcode")]
    [NavigationItem(false)]
    public class ReadResourceMovement : BaseObject
    {
        public ReadResourceMovement(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
        private DateTime _CreatedDate;
        private decimal _Quantity;
        private Unit _Unit;
        private string _ProductionBarcode;
        private Guid _HeaderId;

        [Association]
        public ReadResource ReadResource { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Guid HeaderId
        {
            get
            {
                return _HeaderId;
            }
            set
            {
                SetPropertyValue("HeaderId", ref _HeaderId, value);
            }
        }

        public string ProductionBarcode
        {
            get
            {
                return _ProductionBarcode;
            }
            set
            {
                SetPropertyValue("ProductionBarcode", ref _ProductionBarcode, value);
            }
        }
        
        public Unit Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                SetPropertyValue("Unit", ref _Unit, value);
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
    }
}
