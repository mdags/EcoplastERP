using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.SystemObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("PurchaseOrder.OrderNumber")]
    [NavigationItem("PurchaseManagement")]
    public class PurchaseOrderDetail : BaseObject
    {
        public PurchaseOrderDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            var currency = Session.FindObject<Currency>(CriteriaOperator.Parse("IsDefault = true"));
            if (currency != null) Currency = currency;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                if (Product.ProductGroup.SupplierEvaluation & DemandDetail.Demand.InputControlPerson.Department.Name.Contains("KALÝTE GÜVENCE"))
                {
                    PurchaseOrderStatus = PurchaseOrderStatus.WaitingForSupplierEvaluation;
                }
                else PurchaseOrderStatus = PurchaseOrderStatus.WaitingForWaybill;
            }
            if (PurchaseOrder != null) PurchaseOrder.UpdateTotals();
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (PurchaseOrder != null)
            {
                var purchaseWaybillDetail = Session.FindObject<PurchaseWaybillDetail>(new BinaryOperator("PurchaseOrderDetail", Oid));
                if (purchaseWaybillDetail != null) throw new Exception("Bu tedarikçi sipariþi için irsaliye giriþi yapýlmýþ. Kaydý silmek için öncelikle irsaliyesini silmeniz gerekmektedir.");
                else
                {
                    if (OfferDetail != null) OfferDetail.OfferStatus = OfferStatus.WaitingForOrder;
                    if (DemandDetail != null) DemandDetail.DemandStatus = DemandStatus.WaitingForOrder;

                    //var myEvent = Session.FindObject<MyEvent>(CriteriaOperator.Parse(String.Format("Contains([Subject], '{0}/{1}')", PurchaseOrder.OrderNumber, LineNumber)));
                    //if (myEvent != null) myEvent.Delete();
                }
            }
        }
        // Fields...
        private PurchaseWaybillDetail _PurchaseWaybillDetail;
        private OfferDetail _OfferDetail;
        private DemandDetail _DemandDetail;
        private DateTime _LineDeliveryDate;
        private string _LineInstruction;
        private decimal _Tax;
        private decimal _CurrencyTax;
        private decimal _TaxRate;
        private decimal _Total;
        private decimal _CurrencyTotal;
        private decimal _Price;
        private decimal _ExchangeRate;
        private decimal _CurrencyPrice;
        private Currency _Currency;
        private decimal _Quantity;
        private Unit _Unit;
        private Warehouse _Warehouse;
        private Product _Product;
        private PurchaseOrderStatus _PurchaseOrderStatus;
        private int _LineNumber;
        private PurchaseOrder _PurchaseOrder;

        [Association]
        public PurchaseOrder PurchaseOrder
        {
            get { return _PurchaseOrder; }
            set
            {
                PurchaseOrder prevHome = _PurchaseOrder;
                if (SetPropertyValue("PurchaseOrder", ref _PurchaseOrder, value) && !IsLoading)
                {
                    if (!IsLoading && _PurchaseOrder != null)
                    {
                        int lineNumber = 10;
                        if (_PurchaseOrder.PurchaseOrderDetails.Count > 0)
                        {
                            _PurchaseOrder.PurchaseOrderDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _PurchaseOrder.PurchaseOrderDetails[0].LineNumber + 10;
                        }
                        LineNumber = lineNumber;
                    }
                }
            }
        }

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

        [NonCloneable]
        [VisibleInDetailView(false)]
        public PurchaseOrderStatus PurchaseOrderStatus
        {
            get
            {
                return _PurchaseOrderStatus;
            }
            set
            {
                SetPropertyValue("PurchaseOrderStatus", ref _PurchaseOrderStatus, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        [Association("Product-PurchaseOrderDetails")]
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

        [ImmediatePostData]
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

        [ImmediatePostData]
        [RuleValueComparison(DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
                UpdateCurrencyTotal();
                UpdateTotal();
            }
        }

        [VisibleInDetailView(false)]
        public decimal WaybillQuantity
        {
            get
            {
                return Convert.ToDecimal(Session.Evaluate(typeof(PurchaseWaybillDetail), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("PurchaseOrderDetail = ?", Oid)));
            }
        }

        [VisibleInDetailView(false)]
        public decimal RemainingQuantity
        {
            get
            {
                return Quantity - Convert.ToDecimal(Session.Evaluate(typeof(PurchaseWaybillDetail), CriteriaOperator.Parse("Sum(Quantity)"), CriteriaOperator.Parse("PurchaseOrderDetail = ?", Oid)));
            }
        }

        [ImmediatePostData]
        public Currency Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
                GetExchangeRate();
            }
        }

        [ImmediatePostData]
        [Appearance("PurchaseOrderDetail.CurrencyPrice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyPrice
        {
            get
            {
                return _CurrencyPrice;
            }
            set
            {
                SetPropertyValue("CurrencyPrice", ref _CurrencyPrice, value);
                UpdatePrice();
                UpdateCurrencyTotal();
            }
        }

        [ImmediatePostData]
        [Appearance("PurchaseOrderDetail.ExchangeRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal ExchangeRate
        {
            get
            {
                return _ExchangeRate;
            }
            set
            {
                SetPropertyValue("ExchangeRate", ref _ExchangeRate, value);
                UpdatePrice();
            }
        }

        [ImmediatePostData]
        public decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {
                SetPropertyValue("Price", ref _Price, value);
                UpdateTotal();
            }
        }

        [ImmediatePostData]
        [Appearance("PurchaseOrderDetail.CurrencyTotal", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyTotal
        {
            get
            {
                return _CurrencyTotal;
            }
            set
            {
                SetPropertyValue("CurrencyTotal", ref _CurrencyTotal, value);
                UpdateCurrencyTax();
            }
        }

        [ImmediatePostData]
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
                UpdateTax();
            }
        }

        [ImmediatePostData]
        public decimal TaxRate
        {
            get
            {
                return _TaxRate;
            }
            set
            {
                SetPropertyValue("TaxRate", ref _TaxRate, value);
                UpdateCurrencyTax();
                UpdateTax();
            }
        }

        [Appearance("PurchaseOrderDetail.CurrencyTax", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyTax
        {
            get
            {
                return _CurrencyTax;
            }
            set
            {
                SetPropertyValue("CurrencyTax", ref _CurrencyTax, value);
            }
        }

        public decimal Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                SetPropertyValue("Tax", ref _Tax, value);
            }
        }

        public string LineInstruction
        {
            get
            {
                return _LineInstruction;
            }
            set
            {
                SetPropertyValue("LineInstruction", ref _LineInstruction, value);
            }
        }

        public DateTime LineDeliveryDate
        {
            get
            {
                return _LineDeliveryDate;
            }
            set
            {
                SetPropertyValue("LineDeliveryDate", ref _LineDeliveryDate, value);
            }
        }

        [VisibleInDetailView(false)]
        public DemandDetail DemandDetail
        {
            get
            {
                return _DemandDetail;
            }
            set
            {
                SetPropertyValue("DemandDetail", ref _DemandDetail, value);
            }
        }

        [VisibleInDetailView(false)]
        public OfferDetail OfferDetail
        {
            get
            {
                return _OfferDetail;
            }
            set
            {
                SetPropertyValue("OfferDetail", ref _OfferDetail, value);
            }
        }

        [VisibleInDetailView(false)]
        public PurchaseWaybillDetail PurchaseWaybillDetail
        {
            get
            {
                return _PurchaseWaybillDetail;
            }
            set
            {
                SetPropertyValue("PurchaseWaybillDetail", ref _PurchaseWaybillDetail, value);
            }
        }

        #region functions
        void GetProduct()
        {
            if (IsLoading) return;
            if (Product != null)
            {
                Unit = Product.Unit;
                TaxRate = Product.TaxRate;
            }
        }
        void GetExchangeRate()
        {
            if (IsLoading) return;
            if (Currency != null)
            {
                if (Currency.IsDefault == false)
                {
                    var currencyType = Session.FindObject<CurrencyType>(new BinaryOperator("ForSales", true));
                    ExchangeRate = Helpers.GetExchangeRate(Session, PurchaseOrder != null ? PurchaseOrder.OrderDate : Helpers.GetSystemDate(Session), Currency, currencyType);
                }
            }
        }
        void UpdatePrice()
        {
            if (IsLoading) return;
            if (Currency != null)
            {
                if (Currency.IsDefault == false) Price = CurrencyPrice * ExchangeRate;
            }
        }
        void UpdateCurrencyTotal()
        {
            if (IsLoading) return;
            CurrencyTotal = Quantity * CurrencyPrice;
        }
        void UpdateTotal()
        {
            if (IsLoading) return;
            Total = Quantity * Price;
        }
        void UpdateCurrencyTax()
        {
            if (IsLoading) return;
            if (TaxRate != 0) CurrencyTax = CurrencyTotal * TaxRate / 100;
        }
        void UpdateTax()
        {
            if (IsLoading) return;
            if (TaxRate != 0) Tax = Total * TaxRate / 100;
        }
        #endregion
    }
}
