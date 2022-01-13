using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Barcode")]
    [NavigationItem("ShippingManagement")]
    public class FastShipping : BaseObject
    {
        public FastShipping(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            LoadingDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
        private DateTime _ClosedDate;
        private SalesOrderDetail _SalesOrderDetail;
        private Production _Production;
        private DeliveryLoadingType _DeliveryLoadingType;
        private ShippingUser _ShippingUser;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private string _PaletteNumber;
        private string _Barcode;
        private DateTime _LoadingDate;

        public DateTime LoadingDate
        {
            get
            {
                return _LoadingDate;
            }
            set
            {
                SetPropertyValue("LoadingDate", ref _LoadingDate, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
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
                GetBarcode();
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PaletteNumber
        {
            get
            {
                return _PaletteNumber;
            }
            set
            {
                SetPropertyValue("PaletteNumber", ref _PaletteNumber, value);
            }
        }

        [RuleRequiredField]
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

        [RuleValueComparison("FastShipping.Quantity", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        public Unit cUnit
        {
            get
            {
                return _CUnit;
            }
            set
            {
                SetPropertyValue("cUnit", ref _CUnit, value);
            }
        }

        public decimal cQuantity
        {
            get
            {
                return _CQuantity;
            }
            set
            {
                SetPropertyValue("cQuantity", ref _CQuantity, value);
            }
        }

        [RuleRequiredField]
        public ShippingUser ShippingUser
        {
            get
            {
                return _ShippingUser;
            }
            set
            {
                SetPropertyValue("ShippingUser", ref _ShippingUser, value);
            }
        }

        [Appearance("FastShipping.DeliveryLoadingType", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public DeliveryLoadingType DeliveryLoadingType
        {
            get
            {
                return _DeliveryLoadingType;
            }
            set
            {
                SetPropertyValue("DeliveryLoadingType", ref _DeliveryLoadingType, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Production Production
        {
            get
            {
                return _Production;
            }
            set
            {
                SetPropertyValue("Production", ref _Production, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public SalesOrderDetail SalesOrderDetail
        {
            get
            {
                return _SalesOrderDetail;
            }
            set
            {
                SetPropertyValue("SalesOrderDetail", ref _SalesOrderDetail, value);
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ClosedDate
        {
            get
            {
                return _ClosedDate;
            }
            set
            {
                SetPropertyValue("ClosedDate", ref _ClosedDate, value);
            }
        }

        #region functions
        void GetBarcode()
        {
            if (IsLoading) return;
            if (!string.IsNullOrEmpty(Barcode))
            {
                Production = Session.FindObject<Production>(new BinaryOperator("Barcode", Barcode));
            }
        }
        #endregion
    }
}
