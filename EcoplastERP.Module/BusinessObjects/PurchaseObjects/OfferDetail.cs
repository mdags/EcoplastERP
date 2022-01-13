using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_List")]
    [DefaultProperty("Offer.OfferNumber")]
    [NavigationItem("PurchaseManagement")]
    public class OfferDetail : BaseObject
    {
        public OfferDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OfferStatus = OfferStatus.WaitingForOrder;
            Currency = Session.FindObject<Currency>(new BinaryOperator("IsDefault", true));
            LineDeliveryDate = Helpers.GetSystemDate(Session).AddDays(1);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //if (Session.IsNewObject(this))
            //{
            //    if (OfferStatus != OfferStatus.WaitingForOrder) OfferStatus = OfferStatus.WaitingForOrder;
            //}
            if (Offer != null) Offer.UpdateTotals();
        }
        // Fields...
        private PurchaseWaybillDetail _PurchaseWaybillDetail;
        private PurchaseOrderDetail _PurchaseOrderDetail;
        private DemandDetail _DemandDetail;
        private DateTime _ConfirmDate;
        private string _ConfirmedBy;
        private string _LineInstruction;
        private DateTime _LineDeliveryDate;
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
        private Product _Product;
        private OfferStatus _OfferStatus;
        private int _LineNumber;
        private Offer _Offer;

        [Association]
        public Offer Offer
        {
            get { return _Offer; }
            set
            {
                Offer prevHome = _Offer;
                if (SetPropertyValue("Offer", ref _Offer, value) && !IsLoading)
                {
                    if (!IsLoading && _Offer != null)
                    {
                        int lineNumber = 10;
                        if (_Offer.OfferDetails.Count > 0)
                        {
                            _Offer.OfferDetails.Sorting.Add(new SortProperty("LineNumber", DevExpress.Xpo.DB.SortingDirection.Descending));
                            lineNumber = _Offer.OfferDetails[0].LineNumber + 10;
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
        public OfferStatus OfferStatus
        {
            get
            {
                return _OfferStatus;
            }
            set
            {
                SetPropertyValue("OfferStatus", ref _OfferStatus, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        [Association("Product-OfferDetails")]
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
        [Appearance("OfferDetail.CurrencyPrice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
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
        [Appearance("OfferDetail.ExchangeRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
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
        [Appearance("OfferDetail.CurrencyTotal", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
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

        [Appearance("OfferDetail.CurrencyTax", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
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

        [RuleRequiredField]
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

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ConfirmedBy
        {
            get
            {
                return _ConfirmedBy;
            }
            set
            {
                SetPropertyValue("ConfirmedBy", ref _ConfirmedBy, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ConfirmDate
        {
            get
            {
                return _ConfirmDate;
            }
            set
            {
                SetPropertyValue("ConfirmDate", ref _ConfirmDate, value);
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

        [Action(PredefinedCategory.View, Caption = "Teklifi Onayla", AutoCommit = true, ImageName = "Action_Grant", TargetObjectsCriteria = "OfferStatus = 'WaitingForConfirm'", ToolTip = "Bu iþlem seçili teklifin kalemine onay verir.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void ConfirmOffer()
        {
            OfferStatus = OfferStatus.WaitingForOrder;
            var employee = Session.FindObject<Employee>(new BinaryOperator("UserName", SecuritySystem.CurrentUserId));
            if (employee != null) ConfirmedBy = employee.NameSurname;
            ConfirmDate = Helpers.GetSystemDate(Session);
        }

        [Action(PredefinedCategory.View, Caption = "Ýptal Et", AutoCommit = true, ImageName = "Action_Cancel", ConfirmationMessage = "Bu iþlem seçili teklifin kalemini iptal edecektir. Devam etmek istiyor musunuz?", TargetObjectsCriteria = "OfferStatus != 'Canceled'", ToolTip = "Bu iþlem seçili teklifin kalemini iptal eder.", SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]
        public void CancelOffer()
        {
            OfferStatus = OfferStatus.Canceled;
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
                    ExchangeRate = Helpers.GetExchangeRate(Session, Offer != null ? Offer.OfferDate : Helpers.GetSystemDate(Session), Currency, currencyType);
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
