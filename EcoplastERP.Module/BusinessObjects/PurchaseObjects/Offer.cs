using System;
using System.Windows.Forms;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using EcoplastERP.Module.BusinessObjects.SystemObjects;
using EcoplastERP.Module.BusinessObjects.ProductObjects;
using EcoplastERP.Module.BusinessObjects.MarketingObjects;
using EcoplastERP.Module.BusinessObjects.HumanResourceObjects;

namespace EcoplastERP.Module.BusinessObjects.PurchaseObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Invoice")]
    [DefaultProperty("OfferNumber")]
    [NavigationItem("PurchaseManagement")]
    public class Offer : BaseObject
    {
        public Offer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OfferDate = Helpers.GetSystemDate(Session);
            OfferNumber = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName);
            CreatedBy = Session.FindObject<Employee>(new BinaryOperator("UserName.UserName", SecuritySystem.CurrentUserName));
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            foreach (var item in OfferDetails)
            {
                if (item.DemandDetail != null)
                {
                    item.DemandDetail.OfferDetail = item;
                    if (item.DemandDetail.DemandStatus == DemandStatus.WaitingForPurchase)
                        item.DemandDetail.DemandStatus = DemandStatus.WaitingForOrder;
                }
            }
            if (Session.Connection != null && CreatePurchaseOrder)
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder(Session)
                {
                    Contact = Contact,
                    DistributionChannel = DistributionChannel
                };
                foreach (var item in OfferDetails)
                {
                    PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail(Session)
                    {
                        PurchaseOrder = purchaseOrder,
                        Product = Session.FindObject<Product>(new BinaryOperator("Oid", item.Product.Oid)),
                        Unit = Session.FindObject<Unit>(new BinaryOperator("Oid", item.Unit.Oid)),
                        Quantity = item.Quantity,
                        Currency = Session.FindObject<Currency>(new BinaryOperator("Oid", item.Currency.Oid)),
                        CurrencyPrice = item.CurrencyPrice,
                        ExchangeRate = item.ExchangeRate,
                        Price = item.Price,
                        CurrencyTotal = item.CurrencyTotal,
                        Total = item.Total,
                        TaxRate = item.TaxRate,
                        CurrencyTax = item.CurrencyTax,
                        Tax = item.Tax,
                        LineInstruction = item.LineInstruction,
                        LineDeliveryDate = item.LineDeliveryDate,
                        DemandDetail = Session.FindObject<DemandDetail>(new BinaryOperator("Oid", item.DemandDetail.Oid)),
                        OfferDetail = item
                    };
                }
                XtraMessageBox.Show(string.Format("{0} nolu tedarikçi sipariþi oluþturuldu.", purchaseOrder.OrderNumber), "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            foreach (var item in OfferDetails)
            {
                var purchaseOrderDetail = Session.FindObject<PurchaseOrderDetail>(new BinaryOperator("OfferDetail", item.Oid));
                if (purchaseOrderDetail != null) throw new Exception("Bu teklifin kalemlerinden en az biri için tedarikçi sipariþi oluþturulmuþ. Kaydý silmek için öncelikle tedarikçi sipariþinin silinmesi gerek.");
                else
                {
                    if (item.DemandDetail != null) item.DemandDetail.DemandStatus = DemandStatus.WaitingForPurchase;
                }
            }
        }
        // Fields...
        private Employee _CreatedBy;
        private decimal _Total;
        private decimal _TaxTotal;
        private decimal _SubTotal;
        private bool _CreatePurchaseOrder;
        private PaymentMethod _PaymentMethod;
        private DateTime _ExpirationDate;
        private DateTime _TermDate;
        private DistributionChannel _DistributionChannel;
        private string _AuthorizedPerson;
        private Contact _Contact;
        private DateTime _OfferDate;
        private int _OfferNumber;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public int OfferNumber
        {
            get
            {
                return _OfferNumber;
            }
            set
            {
                SetPropertyValue("OfferNumber", ref _OfferNumber, value);
            }
        }

        [RuleRequiredField]
        public DateTime OfferDate
        {
            get
            {
                return _OfferDate;
            }
            set
            {
                SetPropertyValue("OfferDate", ref _OfferDate, value);
            }
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                SetPropertyValue("Contact", ref _Contact, value);
                GeContact();
            }
        }

        public string AuthorizedPerson
        {
            get
            {
                return _AuthorizedPerson;
            }
            set
            {
                SetPropertyValue("AuthorizedPerson", ref _AuthorizedPerson, value);
            }
        }

        [RuleRequiredField]
        public DistributionChannel DistributionChannel
        {
            get
            {
                return _DistributionChannel;
            }
            set
            {
                SetPropertyValue("DistributionChannel", ref _DistributionChannel, value);
            }
        }

        public DateTime TermDate
        {
            get
            {
                return _TermDate;
            }
            set
            {
                SetPropertyValue("TermDate", ref _TermDate, value);
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

        public PaymentMethod PaymentMethod
        {
            get
            {
                return _PaymentMethod;
            }
            set
            {
                SetPropertyValue("PaymentMethod", ref _PaymentMethod, value);
            }
        }

        public bool CreatePurchaseOrder
        {
            get
            {
                return _CreatePurchaseOrder;
            }
            set
            {
                SetPropertyValue("CreatePurchaseOrder", ref _CreatePurchaseOrder, value);
            }
        }

        [Appearance("Offer.SubTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal SubTotal
        {
            get
            {
                return _SubTotal;
            }
            set
            {
                SetPropertyValue("SubTotal", ref _SubTotal, value);
            }
        }

        [Appearance("Offer.TaxTotal", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal TaxTotal
        {
            get
            {
                return _TaxTotal;
            }
            set
            {
                SetPropertyValue("TaxTotal", ref _TaxTotal, value);
            }
        }

        [Appearance("Offer.Total", Context = "DetailView", Enabled = false, Criteria = "1 = 1")]
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }

        [Association, Aggregated]
        public XPCollection<OfferDetail> OfferDetails
        {
            get { return GetCollection<OfferDetail>("OfferDetails"); }
        }

        #region functions
        void GeContact()
        {
            if (IsLoading) return;
            if (Contact != null)
            {
                DistributionChannel = Contact.DistributionChannel;
                PaymentMethod = Contact.PaymentMethod;
            }
        }
        public void UpdateTotals()
        {
            if (IsLoading) return;
            decimal subTotal = 0, taxTotal = 0;
            foreach (var detail in OfferDetails)
            {
                subTotal += detail.Total;
                taxTotal += detail.Tax;
            }
            SubTotal = subTotal;
            TaxTotal = taxTotal;
            Total = SubTotal + TaxTotal;
        }
        #endregion
    }
}
