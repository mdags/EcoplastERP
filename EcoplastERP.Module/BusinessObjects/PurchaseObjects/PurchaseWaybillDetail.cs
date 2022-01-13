using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("PurchaseWaybill.WaybillNumber")]
    [NavigationItem(false)]
    public class PurchaseWaybillDetail : BaseObject
    {
        public PurchaseWaybillDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Currency  = Session.FindObject<Currency>(new BinaryOperator("IsDefault", true));
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (PurchaseWaybill != null) PurchaseWaybill.UpdateTotals();
        }
        // Fields...
        private PurchaseOrderDetail _PurchaseOrderDetail;
        private OfferDetail _OfferDetail;
        private DemandDetail _DemandDetail;
        private decimal _Tax;
        private decimal _CurrencyTax;
        private decimal _TaxRate;
        private bool _IncludeTax;
        private decimal _Total;
        private decimal _CurrencyTotal;
        private decimal _Price;
        private decimal _ExchangeRate;
        private decimal _CurrencyPrice;
        private Currency _Currency;
        private decimal _Quantity;
        private Unit _Unit;
        private string _PartyNumber;
        private Warehouse _Warehouse;
        private Product _Product;
        private int _LineNumber;
        private PurchaseWaybill _PurchaseWaybill;

        [Association]
        public PurchaseWaybill PurchaseWaybill
        {
            get { return _PurchaseWaybill; }
            set
            {
                PurchaseWaybill prevHome = _PurchaseWaybill;
                if (SetPropertyValue("PurchaseWaybill", ref _PurchaseWaybill, value) && !IsLoading)
                {
                    if (!IsLoading && _PurchaseWaybill != null)
                    {
                        int lineNumber = 10;
                        if (_PurchaseWaybill.PurchaseWaybillDetails.Count > 0)
                        {
                            _PurchaseWaybill.PurchaseWaybillDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _PurchaseWaybill.PurchaseWaybillDetails[0].LineNumber + 10;
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

        [Appearance("PurchaseWaybillDetail.Product", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
        [ImmediatePostData]
        [RuleRequiredField]
        [Association("Product-PurchaseWaybillDetails")]
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

        [Appearance("PurchaseWaybillDetail.Warehouse", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.PartyNumber", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Unit", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Quantity", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Currency", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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
        [Appearance("PurchaseWaybillDetail.CurrencyPrice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        [Appearance("PurchaseWaybillDetail.CurrencyPrice1", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.ExchangeRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        [Appearance("PurchaseWaybillDetail.ExchangeRate1", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Price", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.CurrencyTotal", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        [Appearance("PurchaseWaybillDetail.CurrencyTotal1", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Total", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.IncludeTax", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
        public bool IncludeTax
        {
            get
            {
                return _IncludeTax;
            }
            set
            {
                SetPropertyValue("IncludeTax", ref _IncludeTax, value);
            }
        }

        [Appearance("PurchaseWaybillDetail.TaxRate", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.CurrencyTax", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        [Appearance("PurchaseWaybillDetail.CurrencyTax1", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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

        [Appearance("PurchaseWaybillDetail.Tax", Context = "DetailView", Enabled = false, Criteria = "PurchaseWaybill.PurchaseWaybillStatus = 1")]
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
        public PurchaseOrderDetail PurchaseOrderDetail
        {
            get
            {
                return _PurchaseOrderDetail;
            }
            set
            {
                SetPropertyValue("PurchaseOrderDetail", ref _PurchaseOrderDetail, value);
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
                Warehouse = Product.Warehouse;
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
                    ExchangeRate = Helpers.GetExchangeRate(Session, PurchaseWaybill != null ? PurchaseWaybill.WaybillDate : Helpers.GetSystemDate(Session), Currency, currencyType);
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
