using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.ShippingObjects;

namespace EcoplastERP.Module.BusinessObjects.MarketingObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Sale_Item")]
    [DefaultProperty("SalesWaybill.WaybillNumber")]
    [NavigationItem(false)]
    public class SalesWaybillDetail : BaseObject
    {
        public SalesWaybillDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private SalesOrderDetail _SalesOrderDetail;
        private DeliveryDetail _DeliveryDetail;
        private ExpeditionDetail _ExpeditionDetail;
        private string _PartyNumber;
        private decimal _CQuantity;
        private Unit _CUnit;
        private decimal _Quantity;
        private Unit _Unit;
        private Warehouse _Warehouse;
        private Product _Product;
        private int _LineNumber;
        private SalesWaybill _SalesWaybill;

        [NonCloneable]
        [Association]
        public SalesWaybill SalesWaybill
        {
            get { return _SalesWaybill; }
            set
            {
                SalesWaybill prevHome = _SalesWaybill;
                if (SetPropertyValue("SalesWaybill", ref _SalesWaybill, value) && !IsLoading)
                {
                    if (!IsLoading && _SalesWaybill != null)
                    {
                        LineNumber = (_SalesWaybill.SalesWaybillDetails.Count + 1) * 10;
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

        [RuleRequiredField]
        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                SetPropertyValue("Product", ref _Product, value);
                GetProduct();
            }
        }

        [RuleRequiredField]
        public Warehouse Warehouse
        {
            get
            {
                return _Warehouse;
            }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
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

        [RuleRequiredField]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
                //CalculatecQuantity();
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
                //CalculatecQuantity();
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

        public string PartyNumber
        {
            get
            {
                return _PartyNumber;
            }
            set
            {
                SetPropertyValue("PartyNumber", ref _PartyNumber, value);
            }
        }

        [VisibleInDetailView(false)]
        public ExpeditionDetail ExpeditionDetail
        {
            get
            {
                return _ExpeditionDetail;
            }
            set
            {
                SetPropertyValue("ExpeditionDetail", ref _ExpeditionDetail, value);
            }
        }

        [VisibleInDetailView(false)]
        public DeliveryDetail DeliveryDetail
        {
            get
            {
                return _DeliveryDetail;
            }
            set
            {
                SetPropertyValue("DeliveryDetail", ref _DeliveryDetail, value);
            }
        }

        [VisibleInDetailView(false)]
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

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Warehouse = Product.Warehouse;
                Unit = Product.Unit;
            }
        }
        void CalculatecQuantity()
        {
            if (IsLoading) return;
            if (cUnit != null)
            {
                if (cUnit == Unit) cQuantity = Quantity;
                else
                {
                    var cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", Unit, cUnit, Product));
                    if (cunit != null) cQuantity = (Quantity * cunit.cQuantity) / cunit.BaseQuantity;
                    else
                    {
                        cunit = Session.FindObject<ConversionUnit>(CriteriaOperator.Parse("pUnit = ? and cUnit = ? and Product = ?", cUnit, Unit, Product));
                        if (cunit != null) cQuantity = (cunit.BaseQuantity * Quantity) / cunit.cQuantity;
                    }
                }
            }
        }
        #endregion
    }
}
