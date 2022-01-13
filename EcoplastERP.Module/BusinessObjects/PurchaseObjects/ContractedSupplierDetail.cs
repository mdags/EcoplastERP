using System.ComponentModel;
using DevExpress.Xpo;
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
    [DefaultProperty("Product.Name")]
    [NavigationItem(false)]
    public class ContractedSupplierDetail : BaseObject
    {
        public ContractedSupplierDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private decimal _Price;
        private decimal _ExchangeRate;
        private decimal _CurrencyPrice;
        private Currency _Currency;
        private Product _Product;

        [Association]
        public ContractedSupplier ContractedSupplier { get; set; }

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
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public Currency Currency
        {
            get
            {
                return _Currency;
            }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
            }
        }

        [Appearance("ContractedSupplierDetail.CurrencyPrice", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal CurrencyPrice
        {
            get
            {
                return _CurrencyPrice;
            }
            set
            {
                SetPropertyValue("CurrencyPrice", ref _CurrencyPrice, value);
            }
        }

        [Appearance("ContractedSupplierDetail.ExchangeRate", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Currency.IsDefault != false")]
        public decimal ExchangeRate
        {
            get
            {
                return _ExchangeRate;
            }
            set
            {
                SetPropertyValue("ExchangeRate", ref _ExchangeRate, value);
            }
        }

        public decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {
                SetPropertyValue("Price", ref _Price, value);
            }
        }
    }
}
