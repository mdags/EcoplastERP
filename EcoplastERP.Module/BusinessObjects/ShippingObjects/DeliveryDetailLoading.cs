using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ProductionObjects;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace EcoplastERP.Module.BusinessObjects.ShippingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Expedition.ExpeditionNumber")]
    [NavigationItem(false)]
    public class DeliveryDetailLoading : BaseObject
    {
        public DeliveryDetailLoading(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            LoadingDate = Helpers.GetSystemDate(Session);
        }
        // Fields...
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
        private int _LineNumber;
        private DeliveryDetail _DeliveryDetail;

        [NonCloneable]
        [Association]
        public DeliveryDetail DeliveryDetail
        {
            get { return _DeliveryDetail; }
            set
            {
                DeliveryDetail prevHome = _DeliveryDetail;
                if (SetPropertyValue("DeliveryDetail", ref _DeliveryDetail, value) && !IsLoading)
                {
                    if (!IsLoading && _DeliveryDetail != null)
                    {
                        LineNumber = (_DeliveryDetail.DeliveryDetailLoadings.Count + 1);
                    }
                    if (!IsLoading && prevHome != null)
                    {
                        prevHome.RecalculateNumbers();
                    }
                }
            }
        }

        [NonCloneable]
        [VisibleInDetailView(false)]
        public int LineNumber
        {
            get
            {
                return _LineNumber;
            }
            set
            {
                SetPropertyValue("LineNumber", ref _LineNumber, value);
            }
        }

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

        //[RuleValueComparison("DeliveryDetailLoading.Quantity", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
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

        [VisibleInDetailView(false)]
        public decimal PaletteLastWeight
        {
            get
            {
                ProductionPalette productionPalette = Session.FindObject<ProductionPalette>(new BinaryOperator("PaletteNumber", PaletteNumber));
                return productionPalette != null ? productionPalette.LastWeight : 0;
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

        [Appearance("Delivery.DeliveryLoadingType", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
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
